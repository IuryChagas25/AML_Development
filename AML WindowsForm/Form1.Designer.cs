namespace WindowsFormsApplication1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private Button Btn_OpenCAEX;
        private Button Btn_SaveCAEX;
        private Button Btn_ValidateCAEXFile;
        private TreeView CAEXTreeView;
        private ListBox myErrorListBox;
        private Label lbl_FileName;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Btn_OpenCAEX = new Button();
            Btn_SaveCAEX = new Button();
            Btn_ValidateCAEXFile = new Button();
            CAEXTreeView = new TreeView();
            myErrorListBox = new ListBox();
            lbl_FileName = new Label();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            SuspendLayout();

            // 
            // Btn_OpenCAEX
            // 
            Btn_OpenCAEX.Location = new Point(12, 12);
            Btn_OpenCAEX.Name = "Btn_OpenCAEX";
            Btn_OpenCAEX.Size = new Size(120, 30);
            Btn_OpenCAEX.TabIndex = 0;
            Btn_OpenCAEX.Text = "Abrir CAEX";
            Btn_OpenCAEX.UseVisualStyleBackColor = true;
            Btn_OpenCAEX.Click += Btn_OpenCAEX_Click;

            // 
            // Btn_SaveCAEX
            // 
            Btn_SaveCAEX.Location = new Point(138, 12);
            Btn_SaveCAEX.Name = "Btn_SaveCAEX";
            Btn_SaveCAEX.Size = new Size(120, 30);
            Btn_SaveCAEX.TabIndex = 1;
            Btn_SaveCAEX.Text = "Salvar CAEX";
            Btn_SaveCAEX.UseVisualStyleBackColor = true;
            Btn_SaveCAEX.Click += Btn_SaveCAEX_Click;

            // 
            // Btn_ValidateCAEXFile
            // 
            Btn_ValidateCAEXFile.Location = new Point(264, 12);
            Btn_ValidateCAEXFile.Name = "Btn_ValidateCAEXFile";
            Btn_ValidateCAEXFile.Size = new Size(120, 30);
            Btn_ValidateCAEXFile.TabIndex = 2;
            Btn_ValidateCAEXFile.Text = "Validar CAEX";
            Btn_ValidateCAEXFile.UseVisualStyleBackColor = true;
            Btn_ValidateCAEXFile.Click += Btn_ValidateCAEXFile_Click;

            // 
            // CAEXTreeView
            // 
            CAEXTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            CAEXTreeView.Location = new Point(12, 60);
            CAEXTreeView.Name = "CAEXTreeView";
            CAEXTreeView.Size = new Size(400, 400);
            CAEXTreeView.TabIndex = 3;

            // 
            // myErrorListBox
            // 
            myErrorListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            myErrorListBox.FormattingEnabled = true;
            myErrorListBox.HorizontalScrollbar = true;
            myErrorListBox.Location = new Point(418, 60);
            myErrorListBox.Name = "myErrorListBox";
            myErrorListBox.Size = new Size(400, 404);
            myErrorListBox.TabIndex = 4;

            // 
            // lbl_FileName
            // 
            lbl_FileName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lbl_FileName.AutoSize = true;
            lbl_FileName.Location = new Point(12, 470);
            lbl_FileName.Name = "lbl_FileName";
            lbl_FileName.Size = new Size(191, 20);
            lbl_FileName.TabIndex = 5;
            lbl_FileName.Text = "Nenhum arquivo carregado";

            // 
            // Form1
            // 
            ClientSize = new Size(830, 500);
            Controls.Add(lbl_FileName);
            Controls.Add(myErrorListBox);
            Controls.Add(CAEXTreeView);
            Controls.Add(Btn_ValidateCAEXFile);
            Controls.Add(Btn_SaveCAEX);
            Controls.Add(Btn_OpenCAEX);
            Name = "Form1";
            Text = "AML Demonstrator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
