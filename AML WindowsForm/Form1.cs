// File: Form1.cs
using System;
using System.Windows.Forms;
using WindowsFormsApplication1.Presenter;
using WindowsFormsApplication1.Model;
using WindowsFormsApplication1.View;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form, IMainView
    {
        private readonly MainPresenter _presenter;
        public Form1()
        {
            InitializeComponent();
            _presenter = new MainPresenter(this, new CAEXService());
        }
        public string FileName { set => lbl_FileName.Text = value; }
        public bool IsBusy { set => SetUiEnabled(!value); }
        public TreeNode SelectedNode { get => CAEXTreeView.SelectedNode; set => CAEXTreeView.SelectedNode = value; }
        public event EventHandler OpenClicked;
        public event EventHandler SaveClicked;
        public event EventHandler ValidateClicked;
        public event EventHandler<TreeNodeMouseClickEventArgs> TreeNodeRightClicked;
        public event EventHandler EditRequested;
        private void Btn_OpenCAEX_Click(object sender, EventArgs e) => OpenClicked?.Invoke(this, EventArgs.Empty);
        private void Btn_SaveCAEX_Click(object sender, EventArgs e) => SaveClicked?.Invoke(this, EventArgs.Empty);
        private void Btn_ValidateCAEXFile_Click(object sender, EventArgs e) => ValidateClicked?.Invoke(this, EventArgs.Empty);
        private void CAEXTreeView_NodeMouseClick(object s, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                TreeNodeRightClicked?.Invoke(this, e);
        }
        private void EditMenuItem_Click(object s, EventArgs e) => EditRequested?.Invoke(this, EventArgs.Empty);

        private void CAEXTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // garante que foi clique esquerdo
            if (e.Button == MouseButtons.Left)
                EditRequested?.Invoke(this, EventArgs.Empty);
        }
        public void ClearTree() => CAEXTreeView.Nodes.Clear();
        public void AddTreeNode(TreeNode node) => CAEXTreeView.Nodes.Add(node);
        public void ClearErrors() => myErrorListBox.Items.Clear();
        public void AddError(string message) => myErrorListBox.Items.Add(message);
        public void ShowMessage(string message, string title, MessageBoxIcon icon)
            => MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        private void SetUiEnabled(bool enabled)
        {
            Btn_OpenCAEX.Enabled = enabled;
            Btn_SaveCAEX.Enabled = enabled;
            Btn_ValidateCAEXFile.Enabled = enabled;
        }
    }
}