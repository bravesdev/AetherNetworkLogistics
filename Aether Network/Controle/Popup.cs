using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using Guna.UI2.WinForms;

namespace aether.Controle
{
    public class PopupForm : Form
    {
        private static List<PopupForm> _ativos = new List<PopupForm>();
        private System.Windows.Forms.Timer _timerAnimacao = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer _timerVida = new System.Windows.Forms.Timer();
        private bool _fechando = false;
        private int _yDestino;
        private Form _pai;

        public PopupForm(Form pai, string mensagem)
        {
            _pai = pai;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.FromArgb(15, 15, 15);
            this.ShowInTaskbar = false;
            this.TopMost = true;

            // Design
            int largura = 300;
            Size tamanhoTexto = TextRenderer.MeasureText(mensagem, new Font("Segoe UI", 10), new Size(largura, 0), TextFormatFlags.WordBreak);
            this.Size = new Size(largura + 70, tamanhoTexto.Height + 50);

            // Container Guna2
            Guna2Panel container = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.FromArgb(15, 15, 15),
                BorderColor = Color.FromArgb(40, 40, 40),
                BorderThickness = 1
            };
            this.Controls.Add(container);

            new Guna2Elipse { TargetControl = this, BorderRadius = 12 };

            // Ícone Identico ao seu original
            Panel pnlIcone = new Panel { Size = new Size(40, 40), Location = new Point(10, (this.Height / 2) - 20), BackColor = Color.Transparent };
            pnlIcone.Paint += DesenharIcone;
            container.Controls.Add(pnlIcone);

            Label lbl = new Label
            {
                Text = mensagem,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semilight", 10),
                Location = new Point(55, 15),
                Size = new Size(largura, tamanhoTexto.Height),
                BackColor = Color.Transparent
            };
            container.Controls.Add(lbl);

            // Eventos de clique (em tudo)
            this.Click += (s, e) => Fechar();
            lbl.Click += (s, e) => Fechar();
            container.Click += (s, e) => Fechar();
            pnlIcone.Click += (s, e) => Fechar();

            _timerAnimacao.Interval = 10;
            _timerAnimacao.Tick += Animar;

            _timerVida.Interval = 5000;
            _timerVida.Tick += (s, e) => Fechar();
        }

        private void DesenharIcone(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen p = new Pen(Color.FromArgb(255, 180, 0), 2)) e.Graphics.DrawEllipse(p, 4, 4, 25, 25);
            using (SolidBrush b = new SolidBrush(Color.FromArgb(255, 180, 0)))
            {
                e.Graphics.FillRectangle(b, 15, 10, 3, 8);
                e.Graphics.FillRectangle(b, 15, 20, 3, 3);
            }
        }

        private void AjustarPosicao()
        {
            Point pontoPai = _pai.PointToScreen(Point.Empty);
            int x = pontoPai.X + _pai.Width - this.Width - 20;
            int yBase = pontoPai.Y + _pai.Height - this.Height - 20;

            _yDestino = yBase - (_ativos.Count * (this.Height + 10));
            this.Location = new Point(x, pontoPai.Y + _pai.Height);
        }

        private void Animar(object sender, EventArgs e)
        {
            if (!_fechando)
            {
                if (this.Location.Y > _yDestino) this.Location = new Point(this.Location.X, this.Location.Y - 10);
                else _timerAnimacao.Stop();
            }
            else
            {
                this.Location = new Point(this.Location.X + 10, this.Location.Y);
                this.Opacity -= 0.1;
                if (this.Opacity <= 0) { _timerAnimacao.Stop(); base.Close(); }
            }
        }

        public void Fechar()
        {
            if (_fechando) return;
            _fechando = true;
            _timerVida.Stop();
            _ativos.Remove(this);
            _timerAnimacao.Start();
        }

        public static void Show(Form pai, string mensagem)
        {
            PopupForm p = new PopupForm(pai, mensagem);
            _ativos.Add(p);
            p.AjustarPosicao();
            p.Show(pai);
            p.Invalidate();
            p._timerAnimacao.Start();
            p._timerVida.Start();
        }
    }
}