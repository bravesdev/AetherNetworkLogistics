using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using Guna.UI2.WinForms;
using System.Media;
using System.Linq;

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

        // Cores padrão Apple Light Mode
        private readonly Color appleCardBg = Color.White;
        private readonly Color appleText = Color.FromArgb(0, 0, 0);
        private readonly Color appleBorder = Color.FromArgb(229, 229, 234); // Cinza de sistema sutil
        private readonly Color appleAccentBlue = Color.FromArgb(10, 132, 255);

        // ====================================================================
        // PROTEÇÃO DE FOCO: Impede que o popup roube o foco do teclado
        // ====================================================================
        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // WS_EX_NOACTIVATE (0x08000000) - Não ativa a janela
                // WS_EX_TOPMOST (0x00000008) - Garante que fique sempre no topo
                cp.ExStyle |= 0x08000000 | 0x00000008;
                return cp;
            }
        }
        // ====================================================================

        public PopupForm(Form pai, string mensagem)
        {
            if (!string.IsNullOrEmpty(mensagem) && mensagem.Length > 100)
            {
                mensagem = mensagem.Substring(0, 100) + "..";
            }

            _pai = pai;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = appleCardBg;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Opacity = 0;

            int maxLargura = 320;
            Size tamanhoTexto = TextRenderer.MeasureText(mensagem, new Font("SF Pro Text", 10F, FontStyle.Regular), new Size(maxLargura, 0), TextFormatFlags.WordBreak);
            this.Size = new Size(tamanhoTexto.Width + 80, Math.Max(60, tamanhoTexto.Height + 30));

            // Card principal arredondado e limpo (Padrão macOS/iOS)
            Guna2Panel container = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = appleCardBg,
                BorderColor = appleBorder,
                BorderThickness = 1,
                BorderRadius = 14 // Cantos mais arredondados estilo Apple
            };
            this.Controls.Add(container);
            new Guna2Elipse { TargetControl = this, BorderRadius = 14 };

            Panel pnlIcone = new Panel { Size = new Size(30, 30), Location = new Point(15, (this.Height - 30) / 2), BackColor = Color.Transparent };
            pnlIcone.Paint += DesenharIcone;
            container.Controls.Add(pnlIcone);

            Label lbl = new Label
            {
                Text = mensagem,
                ForeColor = appleText,
                Font = new Font("SF Pro Text", 10F, FontStyle.Regular),
                Location = new Point(55, (this.Height - tamanhoTexto.Height) / 2),
                Size = new Size(tamanhoTexto.Width, tamanhoTexto.Height),
                BackColor = Color.Transparent
            };
            container.Controls.Add(lbl);

            this.Click += (s, e) => Fechar();
            lbl.Click += (s, e) => Fechar();
            container.Click += (s, e) => Fechar();
            pnlIcone.Click += (s, e) => Fechar();

            _timerAnimacao.Interval = 10;
            _timerAnimacao.Tick += Animar;
            _timerVida.Interval = 4000;
            _timerVida.Tick += (s, e) => Fechar();
        }

        private void DesenharIcone(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Fundo circular azul translúcido suave
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(30, 10, 132, 255)))
                e.Graphics.FillEllipse(bg, 0, 0, 30, 30);

            // Linhas de "Info" no azul sólido da Apple
            using (Pen p = new Pen(appleAccentBlue, 2.5f))
            {
                // Ponto de cima
                e.Graphics.DrawLine(p, 15, 8, 15, 11);
                // Linha de baixo
                e.Graphics.DrawLine(p, 15, 14, 15, 21);
            }
        }

        private void CalcularDestino()
        {
            if (_pai == null || _pai.IsDisposed) return;

            Rectangle rectPai = _pai.RectangleToScreen(_pai.ClientRectangle);
            int idx = _ativos.IndexOf(this);

            int margemTopo = 20;
            int margemDireita = 20;

            int xDestino = rectPai.Right - this.Width - margemDireita;
            _yDestino = rectPai.Top + margemTopo + (idx * (this.Height + 10));

            this.Left = xDestino;
        }

        private void Animar(object sender, EventArgs e)
        {
            if (!_fechando)
            {
                if (this.Opacity < 1.0) this.Opacity += 0.08;

                int dist = _yDestino - this.Location.Y;
                if (Math.Abs(dist) > 1)
                    this.Location = new Point(this.Location.X, this.Location.Y + (int)Math.Round(dist / 5.0));
                else
                    this.Location = new Point(this.Location.X, _yDestino);
            }
            else
            {
                this.Location = new Point(this.Location.X + 8, this.Location.Y);
                this.Opacity -= 0.08;
                if (this.Opacity <= 0)
                {
                    _timerAnimacao.Stop();
                    base.Close();
                }
            }
        }

        public void Fechar()
        {
            if (_fechando) return;
            _fechando = true;
            _timerVida.Stop();
            _ativos.Remove(this);
            foreach (var p in _ativos) p.CalcularDestino();
            _timerAnimacao.Start();
        }

        public static void Show(Form pai, string mensagem)
        {
            // Utiliza o ActiveForm, o pai especificado, ou o formulário principal
            Form alvo = Form.ActiveForm ?? pai ?? Application.OpenForms[0];
            if (alvo == null) return;

            while (_ativos.Count >= 3)
            {
                _ativos[0].Fechar();
            }

            PopupForm p = new PopupForm(alvo, mensagem);
            Rectangle rectPai = alvo.RectangleToScreen(alvo.ClientRectangle);

            p.Location = new Point(rectPai.Right - p.Width - 20, rectPai.Top - p.Height);

            _ativos.Add(p);
            p.CalcularDestino();

            // ====================================================================
            // SOLUÇÃO DE Z-ORDER
            // ====================================================================
            p.Show();

            try
            {
                string caminhoAudio = System.IO.Path.Combine(Application.StartupPath, "assets", "notify.wav");
                using (SoundPlayer player = new SoundPlayer(caminhoAudio))
                {
                    player.Play();
                }
            }
            catch
            {
                // Proteção contra ausência de saídas de áudio
            }

            p._timerAnimacao.Start();
            p._timerVida.Start();
        }
    }
}