namespace WindowsFormsApplication1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button Btn_OpenCAEX;
        private System.Windows.Forms.Button Btn_SaveCAEX;
        private System.Windows.Forms.Button Btn_ValidateCAEXFile;
        private System.Windows.Forms.TreeView CAEXTreeView;
        private System.Windows.Forms.ListBox myErrorListBox;
        private System.Windows.Forms.Label lbl_FileName;
        private System.Windows.Forms.ContextMenuStrip treeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Btn_OpenCAEX = new System.Windows.Forms.Button();
            this.Btn_SaveCAEX = new System.Windows.Forms.Button();
            this.Btn_ValidateCAEXFile = new System.Windows.Forms.Button();
            this.CAEXTreeView = new System.Windows.Forms.TreeView();
            this.myErrorListBox = new System.Windows.Forms.ListBox();
            this.lbl_FileName = new System.Windows.Forms.Label();
            this.treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();

            // 
            // Btn_OpenCAEX
            // 
            this.Btn_OpenCAEX.Location = new System.Drawing.Point(12, 12);
            this.Btn_OpenCAEX.Name = "Btn_OpenCAEX";
            this.Btn_OpenCAEX.Size = new System.Drawing.Size(120, 30);
            this.Btn_OpenCAEX.TabIndex = 0;
            this.Btn_OpenCAEX.Text = "Abrir CAEX";
            this.Btn_OpenCAEX.UseVisualStyleBackColor = true;
            this.Btn_OpenCAEX.Click += new System.EventHandler(this.Btn_OpenCAEX_Click);

            // 
            // Btn_SaveCAEX
            // 
            this.Btn_SaveCAEX.Location = new System.Drawing.Point(138, 12);
            this.Btn_SaveCAEX.Name = "Btn_SaveCAEX";
            this.Btn_SaveCAEX.Size = new System.Drawing.Size(120, 30);
            this.Btn_SaveCAEX.TabIndex = 1;
            this.Btn_SaveCAEX.Text = "Salvar CAEX";
            this.Btn_SaveCAEX.UseVisualStyleBackColor = true;
            this.Btn_SaveCAEX.Click += new System.EventHandler(this.Btn_SaveCAEX_Click);

            // 
            // Btn_ValidateCAEXFile
            // 
            this.Btn_ValidateCAEXFile.Location = new System.Drawing.Point(264, 12);
            this.Btn_ValidateCAEXFile.Name = "Btn_ValidateCAEXFile";
            this.Btn_ValidateCAEXFile.Size = new System.Drawing.Size(120, 30);
            this.Btn_ValidateCAEXFile.TabIndex = 2;
            this.Btn_ValidateCAEXFile.Text = "Validar CAEX";
            this.Btn_ValidateCAEXFile.UseVisualStyleBackColor = true;
            this.Btn_ValidateCAEXFile.Click += new System.EventHandler(this.Btn_ValidateCAEXFile_Click);

            // 
            // CAEXTreeView
            // 
            this.CAEXTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)));
            this.CAEXTreeView.Location = new System.Drawing.Point(12, 60);
            this.CAEXTreeView.Name = "CAEXTreeView";
            this.CAEXTreeView.Size = new System.Drawing.Size(400, 400);
            this.CAEXTreeView.TabIndex = 3;
            this.CAEXTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.CAEXTreeView_NodeMouseClick);

            // 
            // myErrorListBox
            // 
            this.myErrorListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.myErrorListBox.FormattingEnabled = true;
            this.myErrorListBox.HorizontalScrollbar = true;
            this.myErrorListBox.ItemHeight = 15;
            this.myErrorListBox.Location = new System.Drawing.Point(418, 60);
            this.myErrorListBox.Name = "myErrorListBox";
            this.myErrorListBox.Size = new System.Drawing.Size(411, 404);
            this.myErrorListBox.TabIndex = 4;

            // 
            // lbl_FileName
            // 
            this.lbl_FileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_FileName.AutoSize = true;
            this.lbl_FileName.Location = new System.Drawing.Point(12, 470);
            this.lbl_FileName.Name = "lbl_FileName";
            this.lbl_FileName.Size = new System.Drawing.Size(191, 15);
            this.lbl_FileName.TabIndex = 5;
            this.lbl_FileName.Text = "Nenhum arquivo carregado";

            // treeContextMenu
           
            this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editMenuItem});
            this.treeContextMenu.Name = "treeContextMenu";
            this.treeContextMenu.Size = new System.Drawing.Size(107, 26);
                      // 
                       // editMenuItem
                       // 
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(106, 22);
            this.editMenuItem.Text = "Editar";
            this.editMenuItem.Click += new System.EventHandler(this.EditMenuItem_Click);

            // associar no TreeView
            this.CAEXTreeView.ContextMenuStrip = this.treeContextMenu;
            // usar duplo clique para editar
            this.CAEXTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.CAEXTreeView_NodeMouseDoubleClick);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 500);
            this.Controls.Add(this.lbl_FileName);
            this.Controls.Add(this.myErrorListBox);
            this.Controls.Add(this.CAEXTreeView);
            this.Controls.Add(this.Btn_ValidateCAEXFile);
            this.Controls.Add(this.Btn_SaveCAEX);
            this.Controls.Add(this.Btn_OpenCAEX);
            this.Name = "Form1";
            this.Text = "AML Check";
            this.treeContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}