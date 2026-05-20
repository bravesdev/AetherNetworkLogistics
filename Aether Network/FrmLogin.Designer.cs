using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace aether
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            lblInfo = new Label();
            txtEmail = new TextBox();
            txtCode = new TextBox();
            btnAcao = new Button();
            btnFechar = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
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
            btnAcao.BackColor = Color.FromArgb(255, 51, 51);
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
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(-6, -35);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(450, 189);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(450, 400);
            Controls.Add(btnFechar);
            Controls.Add(btnAcao);
            Controls.Add(txtCode);
            Controls.Add(txtEmail);
            Controls.Add(lblInfo);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "welabs - Cloud Login";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblInfo;
        private TextBox txtEmail;
        private TextBox txtCode;
        private Button btnAcao;
        private Label btnFechar;
        private PictureBox pictureBox1;
    }
}