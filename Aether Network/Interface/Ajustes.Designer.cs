namespace aether
{
    partial class Ajustes
    {
        private System.ComponentModel.IContainer components = null;

        // Declarando os controles para o Designer reconhecer
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlCardAparencia;
        private System.Windows.Forms.Label lblAparencia;
        private System.Windows.Forms.Button btnTheme;
        private System.Windows.Forms.Panel pnlCardRede;
        private System.Windows.Forms.Label lblRede;
        private System.Windows.Forms.CheckBox chkAutoConectar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ajustes));
            lblTitulo = new Label();
            pnlCardAparencia = new Panel();
            lblAparencia = new Label();
            btnTheme = new Button();
            pnlCardRede = new Panel();
            lblRede = new Label();
            chkAutoConectar = new CheckBox();
            label6 = new Label();
            panel1 = new Panel();
            btnFazerBackup = new Button();
            label1 = new Label();
            btnImportBackup = new Button();
            panel2 = new Panel();
            lblEmail = new Label();
            lblUsuario = new Label();
            label2 = new Label();
            label5 = new Label();
            pnlCardAparencia.SuspendLayout();
            pnlCardRede.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(12, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(201, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Configurações";
            // 
            // pnlCardAparencia
            // 
            pnlCardAparencia.BorderStyle = BorderStyle.FixedSingle;
            pnlCardAparencia.Controls.Add(lblAparencia);
            pnlCardAparencia.Controls.Add(btnTheme);
            pnlCardAparencia.Location = new Point(24, 182);
            pnlCardAparencia.Name = "pnlCardAparencia";
            pnlCardAparencia.Size = new Size(248, 90);
            pnlCardAparencia.TabIndex = 1;
            // 
            // lblAparencia
            // 
            lblAparencia.AutoSize = true;
            lblAparencia.BackColor = Color.Transparent;
            lblAparencia.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAparencia.ForeColor = Color.White;
            lblAparencia.Location = new Point(15, 15);
            lblAparencia.Name = "lblAparencia";
            lblAparencia.Size = new Size(87, 21);
            lblAparencia.TabIndex = 0;
            lblAparencia.Text = "Aparência";
            // 
            // btnTheme
            // 
            btnTheme.Location = new Point(15, 45);
            btnTheme.Name = "btnTheme";
            btnTheme.Size = new Size(200, 35);
            btnTheme.TabIndex = 1;
            btnTheme.Text = "Alternar Modo Dark/Light";
            btnTheme.Click += btnTheme_Click;
            // 
            // pnlCardRede
            // 
            pnlCardRede.BorderStyle = BorderStyle.FixedSingle;
            pnlCardRede.Controls.Add(lblRede);
            pnlCardRede.Controls.Add(chkAutoConectar);
            pnlCardRede.Controls.Add(label6);
            pnlCardRede.Location = new Point(24, 278);
            pnlCardRede.Name = "pnlCardRede";
            pnlCardRede.Size = new Size(730, 48);
            pnlCardRede.TabIndex = 2;
            // 
            // lblRede
            // 
            lblRede.AutoSize = true;
            lblRede.BackColor = Color.Transparent;
            lblRede.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblRede.ForeColor = Color.White;
            lblRede.Location = new Point(3, 4);
            lblRede.Name = "lblRede";
            lblRede.Size = new Size(48, 21);
            lblRede.TabIndex = 0;
            lblRede.Text = "Rede";
            // 
            // chkAutoConectar
            // 
            chkAutoConectar.AutoSize = true;
            chkAutoConectar.Location = new Point(3, 27);
            chkAutoConectar.Name = "chkAutoConectar";
            chkAutoConectar.Size = new Size(222, 19);
            chkAutoConectar.TabIndex = 1;
            chkAutoConectar.Text = "Conectar automaticamente ao iniciar";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            label6.ForeColor = Color.FromArgb(148, 163, 184);
            label6.Location = new Point(229, 26);
            label6.Name = "label6";
            label6.Size = new Size(486, 21);
            label6.TabIndex = 11;
            label6.Text = "Sincronize com o banco de dados welabs Cloud para manter o inventário de equipamentos atualizado.";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnFazerBackup);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnImportBackup);
            panel1.Location = new Point(278, 182);
            panel1.Name = "panel1";
            panel1.Size = new Size(248, 90);
            panel1.TabIndex = 3;
            // 
            // btnFazerBackup
            // 
            btnFazerBackup.Location = new Point(127, 45);
            btnFazerBackup.Name = "btnFazerBackup";
            btnFazerBackup.Size = new Size(86, 35);
            btnFazerBackup.TabIndex = 2;
            btnFazerBackup.Text = "Fazer Backup";
            btnFazerBackup.Click += btnFazerBackup_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.ImageAlign = ContentAlignment.BottomCenter;
            label1.Location = new Point(15, 15);
            label1.Name = "label1";
            label1.Size = new Size(128, 21);
            label1.TabIndex = 0;
            label1.Text = "Backup da Lista";
            // 
            // btnImportBackup
            // 
            btnImportBackup.Location = new Point(15, 45);
            btnImportBackup.Name = "btnImportBackup";
            btnImportBackup.Size = new Size(106, 35);
            btnImportBackup.TabIndex = 1;
            btnImportBackup.Text = "Importar Backup";
            btnImportBackup.Click += btnImportBackup_Click;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(lblEmail);
            panel2.Controls.Add(lblUsuario);
            panel2.Controls.Add(label2);
            panel2.Location = new Point(24, 61);
            panel2.Name = "panel2";
            panel2.Size = new Size(324, 105);
            panel2.TabIndex = 4;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.BackColor = Color.Transparent;
            lblEmail.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblEmail.ForeColor = Color.White;
            lblEmail.ImageAlign = ContentAlignment.BottomCenter;
            lblEmail.Location = new Point(15, 66);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(41, 17);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "email";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.BackColor = Color.Transparent;
            lblUsuario.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblUsuario.ForeColor = Color.White;
            lblUsuario.ImageAlign = ContentAlignment.BottomCenter;
            lblUsuario.Location = new Point(15, 49);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(53, 17);
            lblUsuario.TabIndex = 5;
            lblUsuario.Text = "Usuario";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.ImageAlign = ContentAlignment.BottomCenter;
            label2.Location = new Point(15, 15);
            label2.Name = "label2";
            label2.Size = new Size(175, 21);
            label2.TabIndex = 0;
            label2.Text = "Informações de Login";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            label5.ForeColor = Color.FromArgb(148, 163, 184);
            label5.Location = new Point(66, 324);
            label5.Name = "label5";
            label5.Size = new Size(649, 64);
            label5.TabIndex = 10;
            label5.Text = resources.GetString("label5.Text");
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Ajustes
            // 
            BackColor = Color.FromArgb(20, 26, 38);
            ClientSize = new Size(797, 370);
            Controls.Add(pnlCardRede);
            Controls.Add(label5);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(lblTitulo);
            Controls.Add(pnlCardAparencia);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Ajustes";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Configurações";
            pnlCardAparencia.ResumeLayout(false);
            pnlCardAparencia.PerformLayout();
            pnlCardRede.ResumeLayout(false);
            pnlCardRede.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel panel1;
        private Button btnFazerBackup;
        private Label label1;
        private Button btnImportBackup;
        private Panel panel2;
        private Label lblEmail;
        private Label lblUsuario;
        private Label label2;
        private Label label5;
        private Label label6;
    }
}