using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace aether
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            guna2ElipseForm = new Guna.UI2.WinForms.Guna2Elipse(components);
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            txtCode = new Guna.UI2.WinForms.Guna2TextBox();
            btnAcao = new Guna.UI2.WinForms.Guna2Button();
            lblInfo = new Label();
            btnFechar = new Label();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // guna2ElipseForm
            // 
            guna2ElipseForm.BorderRadius = 16;
            guna2ElipseForm.TargetControl = this;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.Transparent;
            txtEmail.BorderColor = Color.FromArgb(212, 212, 217);
            txtEmail.BorderRadius = 8;
            txtEmail.CustomizableEdges = customizableEdges5;
            txtEmail.DefaultText = "";
            txtEmail.Font = new Font("Microsoft Sans Serif", 11F);
            txtEmail.ForeColor = Color.Black;
            txtEmail.HoverState.BorderColor = Color.FromArgb(10, 132, 255);
            txtEmail.Location = new Point(65, 170);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderForeColor = Color.FromArgb(142, 142, 147);
            txtEmail.PlaceholderText = "usuario@valenet.com.br";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtEmail.Size = new Size(320, 36);
            txtEmail.TabIndex = 1;
            txtEmail.TextAlign = HorizontalAlignment.Center;
            // 
            // txtCode
            // 
            txtCode.BackColor = Color.Transparent;
            txtCode.BorderColor = Color.FromArgb(212, 212, 217);
            txtCode.BorderRadius = 8;
            txtCode.CustomizableEdges = customizableEdges3;
            txtCode.DefaultText = "";
            txtCode.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            txtCode.ForeColor = Color.Black;
            txtCode.HoverState.BorderColor = Color.FromArgb(10, 132, 255);
            txtCode.Location = new Point(65, 215);
            txtCode.Name = "txtCode";
            txtCode.PlaceholderForeColor = Color.FromArgb(142, 142, 147);
            txtCode.PlaceholderText = "••••••";
            txtCode.SelectedText = "";
            txtCode.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtCode.Size = new Size(320, 36);
            txtCode.TabIndex = 2;
            txtCode.TextAlign = HorizontalAlignment.Center;
            txtCode.Visible = false;
            // 
            // btnAcao
            // 
            btnAcao.Animated = true;
            btnAcao.BackColor = Color.Transparent;
            btnAcao.BorderRadius = 8;
            btnAcao.Cursor = Cursors.Hand;
            btnAcao.CustomizableEdges = customizableEdges1;
            btnAcao.FillColor = Color.FromArgb(10, 132, 255);
            btnAcao.Font = new Font("Microsoft Sans Serif", 10.5F, FontStyle.Bold);
            btnAcao.ForeColor = Color.White;
            btnAcao.HoverState.FillColor = Color.FromArgb(0, 115, 230);
            btnAcao.Location = new Point(65, 270);
            btnAcao.Name = "btnAcao";
            btnAcao.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnAcao.Size = new Size(320, 40);
            btnAcao.TabIndex = 3;
            btnAcao.Text = "Solicitar Acesso";
            // 
            // lblInfo
            // 
            lblInfo.Font = new Font("Microsoft Sans Serif", 9.5F);
            lblInfo.ForeColor = Color.FromArgb(142, 142, 147);
            lblInfo.Location = new Point(0, 110);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(450, 40);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Bem-vindo, Colaborador.\r\nPara continuar, insira seu e-mail corporativo abaixo.";
            lblInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnFechar
            // 
            btnFechar.Cursor = Cursors.Hand;
            btnFechar.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            btnFechar.ForeColor = Color.FromArgb(142, 142, 147);
            btnFechar.Location = new Point(415, 12);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(25, 25);
            btnFechar.TabIndex = 4;
            btnFechar.Text = "✕";
            btnFechar.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("Microsoft Sans Serif", 8F);
            label1.ForeColor = Color.FromArgb(142, 142, 147);
            label1.Location = new Point(0, 355);
            label1.Name = "label1";
            label1.Size = new Size(450, 30);
            label1.TabIndex = 6;
            label1.Text = "Software publicado sob a licença GNU.";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft Sans Serif", 32F, FontStyle.Bold);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(104, 45);
            label2.Name = "label2";
            label2.Size = new Size(242, 51);
            label2.TabIndex = 7;
            label2.Text = "Aether Log";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(450, 400);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnFechar);
            Controls.Add(btnAcao);
            Controls.Add(txtCode);
            Controls.Add(txtEmail);
            Controls.Add(lblInfo);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "welabs - Cloud Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // Controles atualizados para tipos da Guna UI2
        private Guna.UI2.WinForms.Guna2Elipse guna2ElipseForm;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtCode;
        private Guna.UI2.WinForms.Guna2Button btnAcao;

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label btnFechar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}