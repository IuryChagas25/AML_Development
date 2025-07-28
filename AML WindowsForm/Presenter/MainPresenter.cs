using Aml.Engine.CAEX;
using System;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApplication1.Model;
using WindowsFormsApplication1.View;

namespace WindowsFormsApplication1.Presenter
{
    /// <summary>
    /// Presenter principal, controla interação entre View e Model (ICAEXService).
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly ICAEXService _service;
        private CAEXDocument _currentDoc;
        private CancellationTokenSource _cts;

        public MainPresenter(IMainView view, ICAEXService service)
        {
            _view = view;
            _service = service;

            // Assina eventos da View
            _view.OpenClicked += OnOpenClicked;
            _view.SaveClicked += OnSaveClicked;
            _view.ValidateClicked += OnValidateClicked;
            _view.TreeNodeRightClicked += OnTreeNodeRightClicked;
            _view.EditRequested += OnEditRequested;
        }

        private async void OnOpenClicked(object sender, EventArgs e)
        {
            _view.ClearErrors();
            _view.IsBusy = true;
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                var path = _service.ShowOpenDialog();
                if (path == null) return;

                _currentDoc = await _service.LoadDocumentAsync(path, _cts.Token);
                _view.FileName = path;
                BuildTree();
            }
            catch (OperationCanceledException)
            {
                // Operação cancelada
            }
            catch (Exception ex)
            {
                _view.AddError(ex.Message);
            }
            finally
            {
                _view.IsBusy = false;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (_currentDoc == null)
            {
                _view.ShowMessage("Nenhum documento carregado.", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            _view.ClearErrors();
            _view.IsBusy = true;

            try
            {
                var target = _service.ShowSaveDialog();
                if (target == null) return;

                await _service.SaveDocumentAsync(_currentDoc, target, CancellationToken.None);
                _view.FileName = target;
                BuildTree();
                _view.ShowMessage("Arquivo salvo com sucesso!", "Salvar", MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _view.AddError(ex.Message);
            }
            finally
            {
                _view.IsBusy = false;
            }
        }

        private async void OnValidateClicked(object sender, EventArgs e)
        {
            if (_currentDoc == null)
            {
                _view.ShowMessage("Nenhum documento carregado.", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            _view.ClearErrors();
            _view.IsBusy = true;
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                var result = await _service.ValidateDocumentAsync(_currentDoc, _cts.Token);
                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        _view.AddError(err);
                }
                else
                {
                    _view.ShowMessage("Validação concluída sem erros.", "Validar", MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                // Cancelado
            }
            catch (Exception ex)
            {
                _view.AddError(ex.Message);
            }
            finally
            {
                _view.IsBusy = false;
            }
        }

        private void OnTreeNodeRightClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Repassa o nó selecionado para a View exibir menu de contexto
            _view.SelectedNode = e.Node;
        }

        private void OnEditRequested(object sender, EventArgs e)
        {
            if (_currentDoc == null || _view.SelectedNode == null)
                return;

            // Abre a InputBox para o novo valor
            var newValue = Microsoft.VisualBasic.Interaction.InputBox(
                $"Novo valor para '{_view.SelectedNode.Text}':",
                "Editar Nó",
                "");

            if (string.IsNullOrWhiteSpace(newValue))
                return;

            // Delegamos ao Model a edição do objeto e do TreeNode.Text
            _service.EditNodeValue(_currentDoc, _view.SelectedNode, newValue);
        }


        private void BuildTree()
        {
            _view.ClearTree();
            foreach (var node in _service.BuildTreeNodes(_currentDoc))
            {
                _view.AddTreeNode(node);
            }
        }
    }
}