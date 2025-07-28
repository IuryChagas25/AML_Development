using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1.View
{
    public interface IMainView
    {
        string FileName { set; }
        bool IsBusy { set; }
        TreeNode SelectedNode { get; set; }
        event EventHandler OpenClicked;
        event EventHandler SaveClicked;
        event EventHandler ValidateClicked;
        event EventHandler<TreeNodeMouseClickEventArgs> TreeNodeRightClicked;
        event EventHandler EditRequested;
        void ClearTree();
        void AddTreeNode(TreeNode node);
        void ClearErrors();
        void AddError(string message);
        void ShowMessage(string message, string title, MessageBoxIcon icon);
    }
}