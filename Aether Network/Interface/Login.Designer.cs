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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            lblInfo = new Label();
            txtEmail = new TextBox();
            txtCode = new TextBox();
            btnAcao = new Button();
            btnFechar = new Label();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // lblInfo
            // 
            lblInfo.Font = new Font("Segoe UI", 9F);
            lblInfo.ForeColor = Color.Gray;
            lblInfo.Location = new Point(0, 105);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(450, 40);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Bem vindo Colaborador!.\r\nPara continuar, insira seu e-mail corporativo abaixo.";
            lblInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Consolas", 11F);
            txtEmail.ForeColor = Color.Black;
            txtEmail.Location = new Point(65, 160);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "usuario@valenet.com.br";
            txtEmail.Size = new Size(320, 25);
            txtEmail.TabIndex = 1;
            txtEmail.TextAlign = HorizontalAlignment.Center;
            // 
            // txtCode
            // 
            txtCode.Font = new Font("Consolas", 14F, FontStyle.Bold);
            txtCode.Location = new Point(65, 210);
            txtCode.Name = "txtCode";
            txtCode.PlaceholderText = "------";
            txtCode.Size = new Size(320, 29);
            txtCode.TabIndex = 2;
            txtCode.TextAlign = HorizontalAlignment.Center;
            txtCode.Visible = false;
            // 
            // btnAcao
            // 
            btnAcao.BackColor = Color.SteelBlue;
            btnAcao.Cursor = Cursors.Hand;
            btnAcao.FlatAppearance.BorderSize = 0;
            btnAcao.FlatStyle = FlatStyle.Flat;
            btnAcao.Font = new Font("Consolas", 10F, FontStyle.Bold);
            btnAcao.ForeColor = Color.White;
            btnAcao.Location = new Point(65, 270);
            btnAcao.Name = "btnAcao";
            btnAcao.Size = new Size(320, 45);
            btnAcao.TabIndex = 3;
            btnAcao.Text = "SOLICITAR ACESSO";
            btnAcao.UseVisualStyleBackColor = false;
            // 
            // btnFechar
            // 
            btnFechar.Cursor = Cursors.Hand;
            btnFechar.Font = new Font("Consolas", 12F);
            btnFechar.Location = new Point(415, 10);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(25, 25);
            btnFechar.TabIndex = 4;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 9F);
            label1.ForeColor = Color.Gray;
            label1.Location = new Point(0, 368);
            label1.Name = "label1";
            label1.Size = new Size(450, 40);
            label1.TabIndex = 6;
            label1.Text = "Software publicado sob a licensa GNU.";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Ubuntu Mono", 47.9999924F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.SteelBlue;
            label2.Location = new Point(60, 26);
            label2.Name = "label2";
            label2.Size = new Size(358, 79);
            label2.TabIndex = 7;
            label2.Text = "welabs.me";
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(450, 400);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnFechar);
            Controls.Add(btnAcao);
            Controls.Add(txtCode);
            Controls.Add(txtEmail);
            Controls.Add(lblInfo);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "welabs - Cloud Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblInfo;
        private TextBox txtEmail;
        private TextBox txtCode;
        private Button btnAcao;
        private Label btnFechar;
        private Label label1;
        private Label label2;
    }
}