using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace aether
{
    partial class Ajustes
    {
        private System.ComponentModel.IContainer components = null;

        // Controles atualizados para a suíte Guna UI2
        private System.Windows.Forms.Label lblTitulo;
        private Guna.UI2.WinForms.Guna2Panel pnlCardAparencia;
        private System.Windows.Forms.Label lblAparencia;
        private Guna.UI2.WinForms.Guna2Button btnTheme;
        private Guna.UI2.WinForms.Guna2Panel pnlCardRede;
        private System.Windows.Forms.Label lblRede;
        private Guna.UI2.WinForms.Guna2CheckBox chkAutoConectar;
        private Guna.UI2.WinForms.Guna2Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnFazerBackup;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button btnImportBackup;
        private Guna.UI2.WinForms.Guna2Panel panel2;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblConected;
        private System.Windows.Forms.Label label3;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ajustes));
            lblTitulo = new Label();
            pnlCardAparencia = new Guna2Panel();
            label4 = new Label();
            lblAparencia = new Label();
            btnTheme = new Guna2Button();
            pnlCardRede = new Guna2Panel();
            ckbInicializar = new Guna2CheckBox();
            label7 = new Label();
            label5 = new Label();
            lblRede = new Label();
            lblConected = new Label();
            panel1 = new Guna2Panel();
            btnFazerBackup = new Guna2Button();
            label1 = new Label();
            btnImportBackup = new Guna2Button();
            panel2 = new Guna2Panel();
            lblEmail = new Label();
            lblUsuario = new Label();
            label2 = new Label();
            label3 = new Label();
            panel3 = new Guna2Panel();
            label6 = new Label();
            txtConection = new Guna2TextBox();
            btnConnect = new Guna2Button();
            pnlCardAparencia.SuspendLayout();
            pnlCardRede.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Font = new Font("Microsoft Sans Serif", 22F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(0, 0, 0);
            lblTitulo.Location = new Point(24, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(221, 36);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Configurações";
            // 
            // pnlCardAparencia
            // 
            pnlCardAparencia.BackColor = Color.Transparent;
            pnlCardAparencia.BorderRadius = 10;
            pnlCardAparencia.Controls.Add(label4);
            pnlCardAparencia.Controls.Add(lblAparencia);
            pnlCardAparencia.Controls.Add(btnTheme);
            pnlCardAparencia.CustomizableEdges = customizableEdges3;
            pnlCardAparencia.FillColor = Color.White;
            pnlCardAparencia.Location = new Point(26, 195);
            pnlCardAparencia.Name = "pnlCardAparencia";
            pnlCardAparencia.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlCardAparencia.Size = new Size(248, 95);
            pnlCardAparencia.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Microsoft Sans Serif", 9.5F);
            label4.ForeColor = Color.FromArgb(10, 132, 255);
            label4.Location = new Point(158, 1);
            label4.Name = "label4";
            label4.Size = new Size(88, 16);
            label4.TabIndex = 7;
            label4.Text = "Experimental!";
            // 
            // lblAparencia
            // 
            lblAparencia.AutoSize = true;
            lblAparencia.BackColor = Color.Transparent;
            lblAparencia.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            lblAparencia.ForeColor = Color.FromArgb(0, 0, 0);
            lblAparencia.Location = new Point(16, 16);
            lblAparencia.Name = "lblAparencia";
            lblAparencia.Size = new Size(82, 18);
            lblAparencia.TabIndex = 0;
            lblAparencia.Text = "Aparência";
            // 
            // btnTheme
            // 
            btnTheme.Animated = true;
            btnTheme.BackColor = Color.Transparent;
            btnTheme.BorderRadius = 6;
            btnTheme.Cursor = Cursors.Hand;
            btnTheme.CustomizableEdges = customizableEdges1;
            btnTheme.FillColor = Color.FromArgb(239, 239, 244);
            btnTheme.Font = new Font("Microsoft Sans Serif", 9.5F);
            btnTheme.ForeColor = Color.FromArgb(0, 0, 0);
            btnTheme.HoverState.FillColor = Color.FromArgb(225, 225, 230);
            btnTheme.Location = new Point(16, 48);
            btnTheme.Name = "btnTheme";
            btnTheme.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnTheme.Size = new Size(215, 32);
            btnTheme.TabIndex = 1;
            btnTheme.Text = "Alternar Modo Dark/Light";
            btnTheme.Click += btnTheme_Click;
            // 
            // pnlCardRede
            // 
            pnlCardRede.BackColor = Color.Transparent;
            pnlCardRede.BorderRadius = 10;
            pnlCardRede.Controls.Add(ckbInicializar);
            pnlCardRede.Controls.Add(label7);
            pnlCardRede.CustomizableEdges = customizableEdges5;
            pnlCardRede.FillColor = Color.White;
            pnlCardRede.Location = new Point(26, 296);
            pnlCardRede.Name = "pnlCardRede";
            pnlCardRede.ShadowDecoration.CustomizableEdges = customizableEdges6;
            pnlCardRede.Size = new Size(537, 66);
            pnlCardRede.TabIndex = 2;
            // 
            // ckbInicializar
            // 
            ckbInicializar.AutoSize = true;
            ckbInicializar.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            ckbInicializar.CheckedState.BorderRadius = 0;
            ckbInicializar.CheckedState.BorderThickness = 0;
            ckbInicializar.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            ckbInicializar.Location = new Point(14, 36);
            ckbInicializar.Name = "ckbInicializar";
            ckbInicializar.Size = new Size(158, 19);
            ckbInicializar.TabIndex = 2;
            ckbInicializar.Text = "Inicializar com o sistema.";
            ckbInicializar.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            ckbInicializar.UncheckedState.BorderRadius = 0;
            ckbInicializar.UncheckedState.BorderThickness = 0;
            ckbInicializar.UncheckedState.FillColor = Color.FromArgb(125, 137, 149);
            ckbInicializar.CheckedChanged += ckbInicializar_CheckedChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            label7.ForeColor = Color.FromArgb(0, 0, 0);
            label7.Location = new Point(3, 9);
            label7.Name = "label7";
            label7.Size = new Size(101, 18);
            label7.TabIndex = 1;
            label7.Text = "Inicialização";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Microsoft Sans Serif", 8F);
            label5.ForeColor = Color.FromArgb(142, 142, 147);
            label5.Location = new Point(85, 365);
            label5.Name = "label5";
            label5.Size = new Size(438, 30);
            label5.TabIndex = 10;
            label5.Text = "Este software é disponibilizado sob os termos da Licença Pública Geral GNU (GNU GPL). \r\n";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRede
            // 
            lblRede.AutoSize = true;
            lblRede.BackColor = Color.Transparent;
            lblRede.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            lblRede.ForeColor = Color.FromArgb(0, 0, 0);
            lblRede.Location = new Point(15, 14);
            lblRede.Name = "lblRede";
            lblRede.Size = new Size(47, 18);
            lblRede.TabIndex = 0;
            lblRede.Text = "Rede";
            // 
            // lblConected
            // 
            lblConected.BackColor = Color.Transparent;
            lblConected.Font = new Font("Microsoft Sans Serif", 8F);
            lblConected.ForeColor = Color.FromArgb(142, 142, 147);
            lblConected.Location = new Point(196, 14);
            lblConected.Name = "lblConected";
            lblConected.Size = new Size(65, 18);
            lblConected.TabIndex = 11;
            lblConected.Text = "192.168.1.1";
            lblConected.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.BorderRadius = 10;
            panel1.Controls.Add(btnFazerBackup);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnImportBackup);
            panel1.CustomizableEdges = customizableEdges11;
            panel1.FillColor = Color.White;
            panel1.Location = new Point(291, 195);
            panel1.Name = "panel1";
            panel1.ShadowDecoration.CustomizableEdges = customizableEdges12;
            panel1.Size = new Size(272, 95);
            panel1.TabIndex = 3;
            // 
            // btnFazerBackup
            // 
            btnFazerBackup.Animated = true;
            btnFazerBackup.BackColor = Color.Transparent;
            btnFazerBackup.BorderRadius = 6;
            btnFazerBackup.Cursor = Cursors.Hand;
            btnFazerBackup.CustomizableEdges = customizableEdges7;
            btnFazerBackup.FillColor = Color.FromArgb(10, 132, 255);
            btnFazerBackup.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Bold);
            btnFazerBackup.ForeColor = Color.White;
            btnFazerBackup.HoverState.FillColor = Color.FromArgb(0, 115, 230);
            btnFazerBackup.Location = new Point(127, 48);
            btnFazerBackup.Name = "btnFazerBackup";
            btnFazerBackup.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnFazerBackup.Size = new Size(105, 32);
            btnFazerBackup.TabIndex = 2;
            btnFazerBackup.Text = "Criar Backup";
            btnFazerBackup.TextFormatNoPrefix = true;
            btnFazerBackup.Click += btnFazerBackup_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(0, 0, 0);
            label1.Location = new Point(16, 16);
            label1.Name = "label1";
            label1.Size = new Size(128, 18);
            label1.TabIndex = 0;
            label1.Text = "Backup da Lista";
            // 
            // btnImportBackup
            // 
            btnImportBackup.Animated = true;
            btnImportBackup.BackColor = Color.Transparent;
            btnImportBackup.BorderRadius = 6;
            btnImportBackup.Cursor = Cursors.Hand;
            btnImportBackup.CustomizableEdges = customizableEdges9;
            btnImportBackup.FillColor = Color.FromArgb(239, 239, 244);
            btnImportBackup.Font = new Font("Microsoft Sans Serif", 9.5F);
            btnImportBackup.ForeColor = Color.FromArgb(0, 0, 0);
            btnImportBackup.HoverState.FillColor = Color.FromArgb(225, 225, 230);
            btnImportBackup.Location = new Point(16, 48);
            btnImportBackup.Name = "btnImportBackup";
            btnImportBackup.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnImportBackup.Size = new Size(105, 32);
            btnImportBackup.TabIndex = 1;
            btnImportBackup.Text = "Importar";
            btnImportBackup.Click += btnImportBackup_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.BorderRadius = 10;
            panel2.Controls.Add(lblEmail);
            panel2.Controls.Add(lblUsuario);
            panel2.Controls.Add(label2);
            panel2.CustomizableEdges = customizableEdges13;
            panel2.FillColor = Color.White;
            panel2.Location = new Point(24, 75);
            panel2.Name = "panel2";
            panel2.ShadowDecoration.CustomizableEdges = customizableEdges14;
            panel2.Size = new Size(250, 105);
            panel2.TabIndex = 4;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.BackColor = Color.Transparent;
            lblEmail.Font = new Font("Microsoft Sans Serif", 9.5F);
            lblEmail.ForeColor = Color.FromArgb(142, 142, 147);
            lblEmail.Location = new Point(16, 70);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(40, 16);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "email";
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.BackColor = Color.Transparent;
            lblUsuario.Font = new Font("Microsoft Sans Serif", 10F);
            lblUsuario.ForeColor = Color.FromArgb(0, 0, 0);
            lblUsuario.Location = new Point(16, 48);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(57, 17);
            lblUsuario.TabIndex = 5;
            lblUsuario.Text = "Usuário";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(0, 0, 0);
            label2.Location = new Point(16, 16);
            label2.Name = "label2";
            label2.Size = new Size(171, 18);
            label2.TabIndex = 0;
            label2.Text = "Informações de Login";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Microsoft Sans Serif", 8F);
            label3.ForeColor = Color.FromArgb(142, 142, 147);
            label3.Location = new Point(404, 9);
            label3.Name = "label3";
            label3.Size = new Size(172, 26);
            label3.TabIndex = 11;
            label3.Text = "Desenvolvido por Wenderson Dias\nLaboratório de Desenvolvimento";
            label3.TextAlign = ContentAlignment.TopRight;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Transparent;
            panel3.BorderRadius = 10;
            panel3.Controls.Add(label6);
            panel3.Controls.Add(txtConection);
            panel3.Controls.Add(btnConnect);
            panel3.Controls.Add(lblRede);
            panel3.Controls.Add(lblConected);
            panel3.CustomizableEdges = customizableEdges19;
            panel3.FillColor = Color.White;
            panel3.Location = new Point(291, 75);
            panel3.Name = "panel3";
            panel3.ShadowDecoration.CustomizableEdges = customizableEdges20;
            panel3.Size = new Size(272, 105);
            panel3.TabIndex = 12;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Microsoft Sans Serif", 8F);
            label6.ForeColor = Color.FromArgb(142, 142, 147);
            label6.Location = new Point(4, 44);
            label6.Name = "label6";
            label6.Size = new Size(90, 13);
            label6.TabIndex = 13;
            label6.Text = "Conexão privada ";
            label6.TextAlign = ContentAlignment.TopRight;
            // 
            // txtConection
            // 
            txtConection.AccessibleDescription = "";
            txtConection.BackColor = Color.Transparent;
            txtConection.BorderColor = Color.FromArgb(209, 209, 214);
            txtConection.BorderRadius = 8;
            txtConection.CustomizableEdges = customizableEdges15;
            txtConection.DefaultText = "";
            txtConection.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtConection.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtConection.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtConection.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtConection.FocusedState.BorderColor = Color.FromArgb(0, 122, 255);
            txtConection.Font = new Font("Segoe UI", 9F);
            txtConection.HoverState.BorderColor = Color.FromArgb(0, 122, 255);
            txtConection.Location = new Point(4, 63);
            txtConection.Name = "txtConection";
            txtConection.PlaceholderText = "Digite o ip e a porta";
            txtConection.SelectedText = "";
            txtConection.ShadowDecoration.CustomizableEdges = customizableEdges16;
            txtConection.Size = new Size(155, 29);
            txtConection.TabIndex = 25;
            txtConection.Tag = "";
            // 
            // btnConnect
            // 
            btnConnect.Animated = true;
            btnConnect.BackColor = Color.Transparent;
            btnConnect.BorderRadius = 6;
            btnConnect.Cursor = Cursors.Hand;
            btnConnect.CustomizableEdges = customizableEdges17;
            btnConnect.FillColor = Color.FromArgb(10, 132, 255);
            btnConnect.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Bold);
            btnConnect.ForeColor = Color.White;
            btnConnect.HoverState.FillColor = Color.FromArgb(0, 115, 230);
            btnConnect.Location = new Point(164, 62);
            btnConnect.Name = "btnConnect";
            btnConnect.ShadowDecoration.CustomizableEdges = customizableEdges18;
            btnConnect.Size = new Size(81, 32);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "Connect";
            btnConnect.TextFormatNoPrefix = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // Ajustes
            // 
            BackColor = Color.FromArgb(242, 242, 247);
            ClientSize = new Size(588, 390);
            Controls.Add(label5);
            Controls.Add(panel3);
            Controls.Add(label3);
            Controls.Add(pnlCardRede);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(lblTitulo);
            Controls.Add(pnlCardAparencia);
            ForeColor = Color.FromArgb(0, 0, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
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
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label4;
        private Guna2Panel panel3;
        private Guna2Button btnConnect;
        private Guna2TextBox txtConection;
        private Label label6;
        private Guna2CheckBox ckbInicializar;
        private Label label7;
    }
}