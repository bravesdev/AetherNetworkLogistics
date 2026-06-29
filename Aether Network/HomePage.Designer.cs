namespace aether
{
    partial class HomePage
    {
        private System.ComponentModel.IContainer components = null;
        private Guna.UI2.WinForms.Guna2Button btnLaboratorio;
        private Guna.UI2.WinForms.Guna2Button btnLogistica;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSubtitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomePage));
            lblSubtitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnLaboratorio = new Guna.UI2.WinForms.Guna2Button();
            btnLogistica = new Guna.UI2.WinForms.Guna2Button();
            label1 = new Label();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            SuspendLayout();
            // 
            // lblSubtitle
            // 
            lblSubtitle.BackColor = Color.Transparent;
            lblSubtitle.Font = new Font("Segoe UI", 11F);
            lblSubtitle.ForeColor = Color.FromArgb(134, 134, 139);
            lblSubtitle.Location = new Point(165, 110);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(227, 22);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Selecione o módulo de operação:";
            lblSubtitle.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // btnLaboratorio
            // 
            btnLaboratorio.BorderRadius = 10;
            btnLaboratorio.CustomizableEdges = customizableEdges1;
            btnLaboratorio.FillColor = Color.FromArgb(0, 122, 255);
            btnLaboratorio.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnLaboratorio.ForeColor = Color.White;
            btnLaboratorio.Location = new Point(100, 160);
            btnLaboratorio.Name = "btnLaboratorio";
            btnLaboratorio.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnLaboratorio.Size = new Size(170, 40);
            btnLaboratorio.TabIndex = 2;
            btnLaboratorio.Text = "Laboratório";
            btnLaboratorio.Click += btnLaboratorio_Click;
            // 
            // btnLogistica
            // 
            btnLogistica.BorderRadius = 10;
            btnLogistica.CustomizableEdges = customizableEdges3;
            btnLogistica.FillColor = Color.FromArgb(0, 122, 255);
            btnLogistica.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnLogistica.ForeColor = Color.White;
            btnLogistica.Location = new Point(280, 160);
            btnLogistica.Name = "btnLogistica";
            btnLogistica.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnLogistica.Size = new Size(170, 40);
            btnLogistica.TabIndex = 3;
            btnLogistica.Text = "Logística Reversa";
            btnLogistica.Click += btnLogistica_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(29, 29, 31);
            label1.Location = new Point(190, 40);
            label1.Name = "label1";
            label1.Size = new Size(181, 45);
            label1.TabIndex = 1;
            label1.Text = "Aether Log";
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI", 8F);
            guna2HtmlLabel1.ForeColor = Color.FromArgb(134, 134, 139);
            guna2HtmlLabel1.Location = new Point(139, 240);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(266, 15);
            guna2HtmlLabel1.TabIndex = 4;
            guna2HtmlLabel1.Text = "Software sob licença GNU GPL. Siliconarch Systems.";
            guna2HtmlLabel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // HomePage
            // 
            BackColor = Color.FromArgb(250, 250, 250);
            ClientSize = new Size(550, 274);
            Controls.Add(guna2HtmlLabel1);
            Controls.Add(btnLaboratorio);
            Controls.Add(lblSubtitle);
            Controls.Add(label1);
            Controls.Add(btnLogistica);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "HomePage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Aether";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
    }
}