namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Controles do formulário
        private System.Windows.Forms.Button Btn_OpenCAEX;
        private System.Windows.Forms.Button Btn_CreateCAEX;
        private System.Windows.Forms.Button Btn_SaveCAEX;
        private System.Windows.Forms.Button Btn_ValidateCAEXFile;
        private System.Windows.Forms.Button Btn_EtikettCAEXFile;
        private System.Windows.Forms.TreeView CAEXTreeView;
        private System.Windows.Forms.ListBox myErrorListBox;
        private System.Windows.Forms.Label lbl_FileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;

        /// <summary>
        /// Limpa os recursos que estão sendo usados.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            Btn_OpenCAEX = new Button();
            Btn_CreateCAEX = new Button();
            Btn_SaveCAEX = new Button();
            Btn_ValidateCAEXFile = new Button();
            Btn_EtikettCAEXFile = new Button();
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
            // Manter ancoragem padrão (Top | Left) para botões
            Btn_OpenCAEX.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            Btn_OpenCAEX.TabIndex = 0;
            Btn_OpenCAEX.Text = "Abrir CAEX";
            Btn_OpenCAEX.UseVisualStyleBackColor = true;
            Btn_OpenCAEX.Click += Btn_OpenCAEX_Click;
            // 
            // Btn_CreateCAEX
            // 
            Btn_CreateCAEX.Location = new Point(138, 12);
            Btn_CreateCAEX.Name = "Btn_CreateCAEX";
            Btn_CreateCAEX.Size = new Size(120, 30);
            Btn_CreateCAEX.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            Btn_CreateCAEX.TabIndex = 1;
            Btn_CreateCAEX.Text = "Criar CAEX";
            Btn_CreateCAEX.UseVisualStyleBackColor = true;
            Btn_CreateCAEX.Click += Btn_CreateCAEX_Click;
            // 
            // Btn_SaveCAEX
            // 
            Btn_SaveCAEX.Location = new Point(264, 12);
            Btn_SaveCAEX.Name = "Btn_SaveCAEX";
            Btn_SaveCAEX.Size = new Size(120, 30);
            Btn_SaveCAEX.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            Btn_SaveCAEX.TabIndex = 2;
            Btn_SaveCAEX.Text = "Salvar CAEX";
            Btn_SaveCAEX.UseVisualStyleBackColor = true;
            Btn_SaveCAEX.Click += Btn_SaveCAEX_Click;
            // 
            // Btn_ValidateCAEXFile
            // 
            Btn_ValidateCAEXFile.Location = new Point(390, 12);
            Btn_ValidateCAEXFile.Name = "Btn_ValidateCAEXFile";
            Btn_ValidateCAEXFile.Size = new Size(120, 30);
            Btn_ValidateCAEXFile.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            Btn_ValidateCAEXFile.TabIndex = 3;
            Btn_ValidateCAEXFile.Text = "Validar CAEX";
            Btn_ValidateCAEXFile.UseVisualStyleBackColor = true;
            Btn_ValidateCAEXFile.Click += Btn_ValidateCAEXFile_Click;
            // 
            // Btn_EtikettCAEXFile
            // 
            Btn_EtikettCAEXFile.Location = new Point(516, 12);
            Btn_EtikettCAEXFile.Name = "Btn_EtikettCAEXFile";
            Btn_EtikettCAEXFile.Size = new Size(120, 30);
            Btn_EtikettCAEXFile.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            Btn_EtikettCAEXFile.TabIndex = 4;
            Btn_EtikettCAEXFile.Text = "Etiquetar CAEX";
            Btn_EtikettCAEXFile.UseVisualStyleBackColor = true;
            Btn_EtikettCAEXFile.Click += Btn_EtikettCAEXFile_Click;
            // 
            // CAEXTreeView
            // 
            CAEXTreeView.Location = new Point(12, 60);
            CAEXTreeView.Name = "CAEXTreeView";
            CAEXTreeView.Size = new Size(400, 400);
            // Ancorar topo, esquerdo e fundo para vertical; mas fixar largura ou opcionalmente permitir esticar?
            // Aqui: mantemos largura fixa, mas altura ajustável:
            CAEXTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            CAEXTreeView.TabIndex = 5;
            // 
            // myErrorListBox
            // 
            myErrorListBox = new ListBox();
            myErrorListBox.FormattingEnabled = true;
            myErrorListBox.Location = new Point(418, 60);
            myErrorListBox.Name = "myErrorListBox";
            myErrorListBox.Size = new Size(400, 404);
            // Habilita a barra de rolagem horizontal
            myErrorListBox.HorizontalScrollbar = true;
            // Ancoragem para ampliar com a janela
            myErrorListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            myErrorListBox.TabIndex = 6;
            // 
            // lbl_FileName
            // 
            lbl_FileName.AutoSize = true;
            lbl_FileName.Location = new Point(12, 470);
            lbl_FileName.Name = "lbl_FileName";
            lbl_FileName.Size = new Size(191, 20);
            // Ancorar à esquerda e ao fundo, para que suba/baixe se o form for redimensionado verticalmente
            lbl_FileName.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            lbl_FileName.TabIndex = 7;
            lbl_FileName.Text = "Nenhum arquivo carregado";
            // 
            // Form1
            // 
            ClientSize = new Size(830, 500);
            Controls.Add(lbl_FileName);
            Controls.Add(myErrorListBox);
            Controls.Add(CAEXTreeView);
            Controls.Add(Btn_EtikettCAEXFile);
            Controls.Add(Btn_ValidateCAEXFile);
            Controls.Add(Btn_SaveCAEX);
            Controls.Add(Btn_CreateCAEX);
            Controls.Add(Btn_OpenCAEX);
            Name = "Form1";
            Text = "AML Demonstrator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
