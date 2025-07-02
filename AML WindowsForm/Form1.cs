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

        private async void Btn_CreateCAEX_Click(object sender, EventArgs e)
        {
            myErrorListBox.Items.Clear();
            CAEXTreeView.Nodes.Clear();

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            SetUiEnabled(false);
            try
            {
                CAEXDocument newDoc = await Task.Run(() =>
                {
                    token.ThrowIfCancellationRequested();
                    var doc = CAEXDocument.New_CAEXDocument();

                    // Exemplo de criação (igual ao anterior)
                    var sucLib1 = doc.CAEXFile.SystemUnitClassLib.Append();
                    sucLib1.Name = "MySUCLib";
                    var sucLib2 = doc.CAEXFile.SystemUnitClassLib.Append();
                    sucLib2.Name = "MySUCLib1";
                    var sucLib3 = doc.CAEXFile.SystemUnitClassLib.Append();
                    sucLib3.Name = "MySUCLib2";

                    var suc = sucLib1.SystemUnitClass.Append();
                    suc.Name = "Class MyTank";
                    var suc1 = sucLib1.SystemUnitClass.Append();
                    suc1.Name = "Class MyLid";

                    var sucIntElem1 = suc.InternalElement.Append();
                    sucIntElem1.Name = "IE MyGround";
                    var sucIntElem2 = suc1.CreateClassInstance();
                    sucIntElem2.Name = "Instance of class MyLid";
                    suc.InternalElement.Insert(sucIntElem2);

                    var roleClassLib = doc.CAEXFile.RoleClassLib.Append();
                    roleClassLib.Name = "MyRoleLib";
                    var roleClass = roleClassLib.RoleClass.Append();
                    roleClass.Name = "Role Lid";

                    var insHierarchy = doc.CAEXFile.InstanceHierarchy.Append();
                    insHierarchy.Name = "MyHierarchy";
                    insHierarchy.ChangeMode = ChangeMode.Change;

                    var intElem = insHierarchy.InternalElement.Append();
                    intElem.Name = "IE";
                    intElem.ID = "GUID1";
                    intElem.Description = "Another description";
                    var roleReq = intElem.RoleRequirements.Append();
                    roleReq.RefBaseRoleClassPath = roleClass.GetFullNodePath();
                    intElem.SetAttributeValue("Weight", "21");
                    intElem.SetAttributeValue("Mass1", "11");

                    var intElem2 = insHierarchy.InternalElement.Append();
                    intElem2.Name = "IE2";
                    var att2 = intElem2.Attribute.Append();
                    att2.Name = "Color";
                    att2.Value = "Yellow";
                    att2.Remove();
                    var intElem5 = intElem2.InternalElement.Append();
                    intElem5.Name = "IE5";
                    intElem5.Remove();

                    var sourceDocInfo = doc.CAEXFile.SourceDocumentInformation.FirstOrDefault();
                    if (sourceDocInfo != null)
                    {
                        sourceDocInfo.OriginName = "WriterName";
                        sourceDocInfo.OriginID = "WriterID";
                        sourceDocInfo.OriginVendor = "WriterVendor";
                        sourceDocInfo.OriginVendorURL = "www.writervendor.com";
                        sourceDocInfo.OriginVersion = "WriterVersion";
                        sourceDocInfo.OriginRelease = "WriterRelease";
                        sourceDocInfo.OriginProjectID = "WriterProjectID";
                        sourceDocInfo.OriginProjectTitle = "WriterProjectTitle";
                        sourceDocInfo.LastWritingDateTime = DateTime.Now;
                    }

                    var rev = intElem.New_Revision(DateTime.Now, "Peter Mustermann");
                    if (rev != null)
                    {
                        rev.RevisionDate = DateTime.Now;
                        rev.AuthorName = "Ender";
                        rev.NewVersion = "1.2";
                        rev.OldVersion = "1.1";
                        rev.Comment = "A comment";
                    }

                    return doc;
                }, token);

                if (token.IsCancellationRequested)
                    throw new OperationCanceledException();

                _myDoc = newDoc;
                lbl_FileName.Text = "Documento CAEX (exemplo)";

                CAEXTreeView.Nodes.Clear();
                ShowMyTree(CAEXTreeView, _myDoc);

                MessageBox.Show("Documento CAEX de exemplo criado e exibido.", "Criar CAEX", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                myErrorListBox.Items.Add("Operação de criação cancelada.");
            }
            catch (Exception ex)
            {
                myErrorListBox.Items.Add($"Erro ao criar documento CAEX: {ex.Message}");
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

        private void Btn_EtikettCAEXFile_Click(object sender, EventArgs e)
        {
            myErrorListBox.Items.Clear();
            if (_myDoc == null)
            {
                MessageBox.Show("Nenhum documento carregado para etiquetar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var sourceDocInfo = _myDoc.CAEXFile.SourceDocumentInformation.FirstOrDefault();
                if (sourceDocInfo == null)
                {
                    myErrorListBox.Items.Add("SourceDocumentInformation não encontrado.");
                    return;
                }
                sourceDocInfo.OriginName = "A source tool";
                sourceDocInfo.OriginID = "A source tool";
                sourceDocInfo.OriginVendor = "A company";
                sourceDocInfo.OriginVendorURL = "www.acompany.com";
                sourceDocInfo.OriginVersion = "1.0";
                sourceDocInfo.OriginRelease = "1.0.1";
                sourceDocInfo.LastWritingDateTime = DateTime.Today;
                sourceDocInfo.OriginProjectTitle = "DemoProject";
                sourceDocInfo.OriginProjectID = "DemoProject";

                MessageBox.Show("SourceDocumentInformation atualizado.", "Etiquetar CAEX", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                myErrorListBox.Items.Add($"Erro ao etiquetar: {ex.Message}");
                Debug.WriteLine(ex);
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
            Btn_CreateCAEX.Enabled = enabled;
            Btn_SaveCAEX.Enabled = enabled && _myDoc != null;
            Btn_ValidateCAEXFile.Enabled = enabled && _myDoc != null;
            Btn_EtikettCAEXFile.Enabled = enabled && _myDoc != null;
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
