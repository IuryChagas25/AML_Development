using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aml.Engine.CAEX;

namespace WindowsFormsApplication1.Model
{
    /// <summary>
    /// Contrato para operações de I/O, validação, construção da árvore e edição de nós CAEX.
    /// </summary>
    public interface ICAEXService
    {
        /// <summary>Exibe o OpenFileDialog e retorna o caminho selecionado ou null.</summary>
        string ShowOpenDialog();

        /// <summary>Exibe o SaveFileDialog e retorna o caminho selecionado ou null.</summary>
        string ShowSaveDialog();

        /// <summary>Carrega assincronamente o documento CAEX de um arquivo.</summary>
        Task<CAEXDocument> LoadDocumentAsync(string path, CancellationToken token);

        /// <summary>Salva assincronamente o documento CAEX em um arquivo.</summary>
        Task SaveDocumentAsync(CAEXDocument doc, string path, CancellationToken token);

        /// <summary>Valida assincronamente o documento CAEX e retorna resultado.</summary>
        Task<(bool IsValid, string[] Errors)> ValidateDocumentAsync(CAEXDocument doc, CancellationToken token);

        /// <summary>Constrói os TreeNodes representando toda a hierarquia do CAEXDocument.</summary>
        IEnumerable<TreeNode> BuildTreeNodes(CAEXDocument doc);

        /// <summary>Atualiza o valor ou nome do nó CAEX correspondente ao TreeNode fornecido.</summary>
        void EditNodeValue(CAEXDocument doc, TreeNode node, string newValue);
    }
}