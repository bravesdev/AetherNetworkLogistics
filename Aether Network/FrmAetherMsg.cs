using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace aether
{
    public static class Msg
    {
        public static void Show(string mensagem) => Executar(mensagem, false);
        public static bool Question(string mensagem) => Executar(mensagem, true);

        private static bool Executar(string mensagem, bool isQuestion)
        {
            using (FrmAetherMsg msg = new FrmAetherMsg(mensagem, isQuestion))
            {
                return msg.ShowDialog() == DialogResult.Yes;
            }
        }
    }

    public partial class FrmAetherMsg : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public FrmAetherMsg(string mensagem, bool isQuestion)
        {
            ConfigurarInterface(mensagem, isQuestion);
        }

        private void ConfigurarInterface(string mensagem, bool isQuestion)
        {
            // --- 1. CONFIGURAÇÕES DE FONTE E MEDIÇÃO ---
            Font fonteTexto = new Font("Segoe UI Semilight", 10);
            Size tamanhoMaximo = new Size(500, 800); // Largura máxima de 500px
            TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter;

            // Mede o tamanho real que o texto ocupará
            Size tamanhoTexto = TextRenderer.MeasureText(mensagem, fonteTexto, tamanhoMaximo, flags);

            // --- 2. AJUSTE DINÂMICO DO FORMULÁRIO ---
            int larguraFinal = Math.Max(420, tamanhoTexto.Width + 100);
            int alturaFinal = tamanhoTexto.Height + 160; // Texto + Espaço do Ícone + Botões

            this.Size = new Size(larguraFinal, alturaFinal);
            this.BackColor = Color.FromArgb(10, 10, 10);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;

            // Arredondar cantos (GDI+)
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 15, 15));

            // --- 3. ÍCONE DE AVISO ---
            PictureBox pbIcone = new PictureBox
            {
                Size = new Size(32, 32),
                Location = new Point((this.Width / 2) - 16, 15),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            pbIcone.Paint += DesenharIcone;
            this.Controls.Add(pbIcone);

            // --- 4. LABEL DO TEXTO (DINÂMICO) ---
            Label lbl = new Label
            {
                Text = mensagem,
                Font = fonteTexto,
                ForeColor = Color.FromArgb(230, 230, 230),
                TextAlign = ContentAlignment.TopCenter,
                Location = new Point(30, 60),
                Size = new Size(this.Width - 60, tamanhoTexto.Height + 20),
                AutoSize = false
            };
            this.Controls.Add(lbl);

            // --- 5. CONTAINER DE BOTÕES ---
            Panel pnlBotoes = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pnlBotoes);

            if (isQuestion)
            {
                Button btnSim = CriarBotao("SIM", Color.White, Color.Black, 120);
                Button btnNao = CriarBotao("NÃO", Color.FromArgb(30, 30, 30), Color.White, 120);

                btnSim.Location = new Point((this.Width / 2) - 125, 15);
                btnNao.Location = new Point((this.Width / 2) + 5, 15);

                btnSim.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };
                btnNao.Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };

                pnlBotoes.Controls.Add(btnSim);
                pnlBotoes.Controls.Add(btnNao);
                this.AcceptButton = btnSim;
            }
            else
            {
                Button btnOk = CriarBotao("ENTENDIDO", Color.White, Color.Black, this.Width - 80);
                btnOk.Location = new Point(40, 15);
                btnOk.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                pnlBotoes.Controls.Add(btnOk);
                this.AcceptButton = btnOk;
            }

            // Borda externa sutil
            this.Paint += (s, e) => {
                using (Pen p = new Pen(Color.FromArgb(50, 50, 50), 1))
                    e.Graphics.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
            };

            // Arrastar
            this.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); };
        }

        private Button CriarBotao(string texto, Color back, Color fore, int largura)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = new Size(largura, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                Font = new Font("Segoe UI Bold", 9),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void DesenharIcone(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen p = new Pen(Color.FromArgb(255, 180, 0), 2))
                e.Graphics.DrawEllipse(p, 2, 2, 28, 28);
            using (SolidBrush b = new SolidBrush(Color.FromArgb(255, 180, 0)))
            {
                e.Graphics.FillRectangle(b, 14, 8, 4, 10);
                e.Graphics.FillRectangle(b, 14, 20, 4, 4);
            }
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
    }
}