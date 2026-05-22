using System;
using System.Drawing;
using System.Windows.Forms;

namespace aether
{
    partial class Infomations
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Infomations));

            pnlMainCredits = new Guna.UI2.WinForms.Guna2Panel();
            lblHorario = new Label();
            lblTotal = new Label();
            lblRede = new Label();
            lblAl1 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            label1 = new Label();

            pnlMainCredits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            SuspendLayout();

            // 
            // pnlMainCredits
            // 
            pnlMainCredits.Controls.Add(label1);
            pnlMainCredits.Controls.Add(lblHorario);
            pnlMainCredits.Controls.Add(lblTotal);
            pnlMainCredits.Controls.Add(lblRede);
            pnlMainCredits.Controls.Add(lblAl1);
            pnlMainCredits.Controls.Add(label8);
            pnlMainCredits.Controls.Add(label7);
            pnlMainCredits.Controls.Add(label6);
            pnlMainCredits.Controls.Add(guna2PictureBox1);
            pnlMainCredits.CustomizableEdges = customizableEdges3;
            pnlMainCredits.Dock = DockStyle.Fill;
            pnlMainCredits.FillColor = Color.White; // Mudado para fundo Branco Limpo
            pnlMainCredits.Location = new Point(0, 0);
            pnlMainCredits.Name = "pnlMainCredits";
            pnlMainCredits.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlMainCredits.Size = new Size(850, 520);
            pnlMainCredits.TabIndex = 1;
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.BackColor = Color.Transparent;
            guna2PictureBox1.CustomizableEdges = customizableEdges1;
            guna2PictureBox1.Image = Properties.Resources.Inserir_um_título_20260520_081609_0000;
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(45, 25);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2PictureBox1.Size = new Size(760, 110); // Ajustado para agir como um banner elegante no topo
            guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            guna2PictureBox1.TabIndex = 7;
            guna2PictureBox1.TabStop = false;
            guna2PictureBox1.Click += guna2PictureBox1_Click;
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            label6.ForeColor = Color.FromArgb(30, 41, 59); // Cinza grafite premium (Slate 800)
            label6.Location = new Point(45, 155);
            label6.Name = "label6";
            label6.Size = new Size(760, 45);
            label6.TabIndex = 0;
            label6.Text = "Informações Técnicas";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Consolas", 10F, FontStyle.Bold);
            label7.ForeColor = Color.FromArgb(255, 51, 51); // Sua cor de destaque
            label7.Location = new Point(48, 220);
            label7.Name = "label7";
            label7.Size = new Size(350, 20);
            label7.TabIndex = 1;
            label7.Text = "DESENVOLVIMENTO CORE";
            // 
            // lblAl1
            // 
            lblAl1.BackColor = Color.Transparent;
            lblAl1.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            lblAl1.ForeColor = Color.FromArgb(15, 23, 42); // Quase preto premium
            lblAl1.Location = new Point(48, 245);
            lblAl1.Name = "lblAl1";
            lblAl1.Size = new Size(350, 28);
            lblAl1.TabIndex = 3;
            lblAl1.Text = "Wenderson Dias";
            // 
            // lblRede
            // 
            lblRede.BackColor = Color.Transparent;
            lblRede.Font = new Font("Segoe UI", 10F);
            lblRede.ForeColor = Color.FromArgb(100, 116, 139); // Muted gray para descrição
            lblRede.Location = new Point(48, 273);
            lblRede.Name = "lblRede";
            lblRede.Size = new Size(350, 22);
            lblRede.TabIndex = 4;
            lblRede.Text = "Low-Level Software Developer";
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Consolas", 10F, FontStyle.Bold);
            label8.ForeColor = Color.FromArgb(255, 51, 51); // Sua cor de destaque
            label8.Location = new Point(430, 220);
            label8.Name = "label8";
            label8.Size = new Size(375, 20);
            label8.TabIndex = 2;
            label8.Text = "TECNOLOGIAS E ENGINES";
            // 
            // lblTotal
            // 
            lblTotal.BackColor = Color.Transparent;
            lblTotal.Font = new Font("Segoe UI", 10F);
            lblTotal.ForeColor = Color.FromArgb(51, 65, 85);
            lblTotal.Location = new Point(430, 245);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(375, 180);
            lblTotal.TabIndex = 5;
            lblTotal.Text = "• .NET 10 C# - Windows 11/10\r\n\r\n• Guna UI Engine Components\r\n\r\n• welabs Development SDK\r\n\r\n• welabs Network Library\r\n\r\n• welabs Nanofox Engine";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(255, 51, 51);
            label1.Location = new Point(45, 465);
            label1.Name = "label1";
            label1.Size = new Size(200, 25);
            label1.TabIndex = 8;
            label1.Text = "www.welabs.me";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblHorario
            // 
            lblHorario.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblHorario.BackColor = Color.Transparent;
            lblHorario.Font = new Font("Segoe UI", 9.5F);
            lblHorario.ForeColor = Color.FromArgb(148, 163, 184);
            lblHorario.Location = new Point(555, 465);
            lblHorario.Name = "lblHorario";
            lblHorario.Size = new Size(250, 25);
            lblHorario.TabIndex = 6;
            lblHorario.Text = "welabs | Wenderson Dias";
            lblHorario.TextAlign = ContentAlignment.MiddleRight;
            // 
            // FrmInfomations
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(850, 520);
            Controls.Add(pnlMainCredits);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmInfomations";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Créditos & Informações";
            pnlMainCredits.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel pnlMainCredits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblAl1;
        private System.Windows.Forms.Label lblRede;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblHorario;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Label label1;
    }
}