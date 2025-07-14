using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aml.Engine.AmlObjects;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;
using Aml.Engine.Services;
using Aml.Engine.Xml.Extensions;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private CAEXDocument _myDoc;
        private string _fileName;
        private CancellationTokenSource _cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
            ConfigureDialogs();
            SetUiEnabled(true);
        }

        private void ConfigureDialogs()
        {
            openFileDialog.Filter = "Arquivos AML|*.aml;*.xml";
            openFileDialog.Title = "Selecione um arquivo AML/CAEX";
            saveFileDialog.Filter = "Arquivos AML|*.aml";
            saveFileDialog.Title = "Salvar arquivo AML/CAEX";
        }

        #region Eventos dos Botões

        private async void Btn_OpenCAEX_Click(object sender, EventArgs e)
        {
            myErrorListBox.Items.Clear();
            CAEXTreeView.Nodes.Clear();

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            _fileName = openFileDialog.FileName;
            lbl_FileName.Text = _fileName;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            SetUiEnabled(false);
            try
            {
                await LoadAndDisplayCAEXAsync(_fileName, token);
            }
            catch (OperationCanceledException)
            {
                myErrorListBox.Items.Add("Operação de carregamento cancelada.");
            }
            catch (Exception ex)
            {
                myErrorListBox.Items.Add($"Erro inesperado: {ex.Message}");
                Debug.WriteLine(ex);
            }
            finally
            {
                SetUiEnabled(true);
            }
        }


        private async void Btn_SaveCAEX_Click(object sender, EventArgs e)
        {
            myErrorListBox.Items.Clear();
            if (_myDoc == null)
            {
                MessageBox.Show("Nenhum documento carregado para salvar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            var targetFile = saveFileDialog.FileName;
            SetUiEnabled(false);
            try
            {
                await Task.Run(() => _myDoc.SaveToFile(targetFile, true));
                CAEXDocument tempDoc = null;
                await Task.Run(() => tempDoc = CAEXDocument.LoadFromFile(targetFile));
                _myDoc = tempDoc;
                _fileName = targetFile;
                lbl_FileName.Text = _fileName;

                CAEXTreeView.Nodes.Clear();
                ShowMyTree(CAEXTreeView, _myDoc);

                MessageBox.Show("Arquivo salvo e recarregado com sucesso.", "Salvar CAEX", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                myErrorListBox.Items.Add($"Erro ao salvar/recarregar: {ex.Message}");
                Debug.WriteLine(ex);
            }
            finally
            {
                SetUiEnabled(true);
            }
        }

        private async void Btn_ValidateCAEXFile_Click(object sender, EventArgs e)
        {
            myErrorListBox.Items.Clear();
            if (_myDoc == null)
            {
                MessageBox.Show("Nenhum documento carregado para validar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            SetUiEnabled(false);
            try
            {
                var validationResult = await ValidateCAEXAsync(_myDoc, token);
                if (validationResult.IsValid)
                {
                    MessageBox.Show("Validação concluída: Sem erros.", "Validar CAEX", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    foreach (var msg in validationResult.ErrorMessages)
                        myErrorListBox.Items.Add(msg);

                    MessageBox.Show(
                        $"Validação concluída: {validationResult.ErrorMessages.Length} erro(s). Verifique a lista de erros.",
                        "Validar CAEX", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (OperationCanceledException)
            {
                myErrorListBox.Items.Add("Validação cancelada.");
            }
            catch (Exception ex)
            {
                myErrorListBox.Items.Add($"Erro na validação: {ex.Message}");
                Debug.WriteLine(ex);
            }
            finally
            {
                SetUiEnabled(true);
            }
        }


        #endregion

        #region Métodos Assíncronos

        private Task LoadAndDisplayCAEXAsync(string filePath, CancellationToken token)
        {
            return Task.Run(() =>
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    CAEXDocument doc = null;
                    try
                    {
                        doc = CAEXDocument.LoadFromFile(filePath);
                    }
                    catch (System.Xml.XmlException xmlEx)
                    {
                        SafeInvoke(() => myErrorListBox.Items.Add($"Erro de XML ao carregar arquivo: {xmlEx.Message}"));
                        return;
                    }
                    catch (Exception ex)
                    {
                        SafeInvoke(() => myErrorListBox.Items.Add($"Erro ao carregar arquivo: {ex.Message}"));
                        return;
                    }

                    token.ThrowIfCancellationRequested();
                    _myDoc = doc;

                    if (!token.IsCancellationRequested)
                    {
                        SafeInvoke(() =>
                        {
                            CAEXTreeView.Nodes.Clear();
                            ShowMyTree(CAEXTreeView, _myDoc);
                        });
                    }
                }
                catch (OperationCanceledException)
                {
                    SafeInvoke(() => myErrorListBox.Items.Add("Carregamento cancelado."));
                }
                catch (Exception ex)
                {
                    SafeInvoke(() => myErrorListBox.Items.Add($"Erro inesperado durante carregamento: {ex.Message}"));
                    Debug.WriteLine(ex);
                }
            }, token);
        }

        private Task<(bool IsValid, string[] ErrorMessages)> ValidateCAEXAsync(CAEXDocument doc, CancellationToken token)
        {
            return Task.Run(() =>
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    bool valid = doc.Validate(out var errors);
                    token.ThrowIfCancellationRequested();
                    return (valid, errors);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    SafeInvoke(() => myErrorListBox.Items.Add($"Erro durante validação: {ex.Message}"));
                    return (false, new string[] { $"Exceção na validação: {ex.Message}" });
                }
            }, token);
        }

        #endregion

        #region Helpers de UI

        private void SetUiEnabled(bool enabled)
        {
            Btn_OpenCAEX.Enabled = enabled;
            
            Btn_SaveCAEX.Enabled = enabled && _myDoc != null;
            Btn_ValidateCAEXFile.Enabled = enabled && _myDoc != null;
           
        }

        /// <summary>
        /// Invoca no UI thread com verificações para evitar Invoke em formulário descartado.
        /// </summary>
        private void SafeInvoke(Action action)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try
                {
                    this.Invoke(action);
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
            }
        }

        #endregion

        #region Preenchimento TreeView

        private static void ShowMyTree(TreeView treeView, CAEXDocument doc)
        {
            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            // InstanceHierarchy
            foreach (var ih in doc.CAEXFile.InstanceHierarchy)
            {
                var node = new TreeNode(ih.Name) { Tag = ih };
                treeView.Nodes.Add(node);
                foreach (var ie in ih.InternalElement)
                {
                    ShowMyInternalElement(node, ie);
                }
            }

            // RoleClassLib
            foreach (var rl in doc.CAEXFile.RoleClassLib)
            {
                var node = new TreeNode(rl.Name) { Tag = rl };
                treeView.Nodes.Add(node);
                foreach (var rc in rl.RoleClass)
                {
                    ShowMyRoleClasses(node, rc);
                }
            }

            // SystemUnitClassLib
            foreach (var sucl in doc.CAEXFile.SystemUnitClassLib)
            {
                var node = new TreeNode(sucl.Name) { Tag = sucl };
                treeView.Nodes.Add(node);
                foreach (var suc in sucl.SystemUnitClass)
                {
                    ShowMySystemUnitClasses(node, suc);
                }
            }

            treeView.EndUpdate();
        }

        //private static void ShowMyInternalElement(TreeNode parentNode, InternalElementType ie)
        //{
        //    var childNode = new TreeNode(ie.Name) { Tag = ie };
        //    parentNode.Nodes.Add(childNode);
        //    foreach (var childIe in ie.InternalElement)
        //    {
        //        ShowMyInternalElement(childNode, childIe);
        //    }
        //}

        //private static void ShowMyRoleClasses(TreeNode parentNode, RoleFamilyType rc)
        //{
        //    var childNode = new TreeNode(rc.Name) { Tag = rc };
        //    parentNode.Nodes.Add(childNode);
        //    foreach (var childRc in rc.RoleClass)
        //    {
        //        ShowMyRoleClasses(childNode, childRc);
        //    }
        //}

        /// <summary>
        /// Adiciona ao nó pai todos os atributos da coleção, recursivamente.
        /// </summary>
        private static void ShowAttributes(TreeNode parentNode, IEnumerable<AttributeType> attributes)
        {
            foreach (var attr in attributes)
            {
                // Monta o texto do nó
                string unit = string.IsNullOrEmpty(attr.Unit) ? "" : $" [{attr.Unit}]";
                string val = attr.Value ?? "<sem valor>";
                string text = $"{attr.Name}: {val} ({attr.AttributeDataType}){unit}";

                // Cria nó para este atributo
                var attrNode = new TreeNode(text) { Tag = attr };
                parentNode.Nodes.Add(attrNode);

                // Se este atributo tem atributos filhos, exibe-os também
                if (attr.Attribute != null && attr.Attribute.Any())
                {
                    ShowAttributes(attrNode, attr.Attribute);
                }
            }
        }

        private static void ShowMyInternalElement(TreeNode parentNode, InternalElementType ie)
        {
            var childNode = new TreeNode(ie.Name) { Tag = ie };
            parentNode.Nodes.Add(childNode);

            // Em vez de iterar diretamente aqui, chame o helper:
            ShowAttributes(childNode, ie.Attribute);

            foreach (var childIe in ie.InternalElement)
                ShowMyInternalElement(childNode, childIe);
        }

        private static void ShowMyRoleClasses(TreeNode parentNode, RoleFamilyType rc)
        {
            var childNode = new TreeNode(rc.Name) { Tag = rc };
            parentNode.Nodes.Add(childNode);

            ShowAttributes(childNode, rc.Attribute);

            foreach (var childRc in rc.RoleClass)
                ShowMyRoleClasses(childNode, childRc);
        }

        private static void ShowMySystemUnitClasses(TreeNode parentNode, SystemUnitFamilyType suc)
        {
            var childNode = new TreeNode(suc.Name) { Tag = suc };
            parentNode.Nodes.Add(childNode);

            ShowAttributes(childNode, suc.Attribute);

            foreach (var ie in suc.InternalElement)
                ShowMyInternalElement(childNode, ie);

            foreach (var childSuc in suc.SystemUnitClass)
                ShowMySystemUnitClasses(childNode, childSuc);
        }

        //private static void ShowMySystemUnitClasses(TreeNode parentNode, SystemUnitFamilyType suc)
        //{
        //    var childNode = new TreeNode(suc.Name) { Tag = suc };
        //    parentNode.Nodes.Add(childNode);
        //    foreach (var ie in suc.InternalElement)
        //    {
        //        ShowMyInternalElement(childNode, ie);
        //    }
        //    foreach (var childSuc in suc.SystemUnitClass)
        //    {
        //        ShowMySystemUnitClasses(childNode, childSuc);
        //    }
        //}

        #endregion
    }
}
