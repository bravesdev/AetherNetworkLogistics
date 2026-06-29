using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace aether.Controle
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
        [DllImport("user32.DLL")] private extern static void ReleaseCapture();
        [DllImport("user32.DLL")] private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        // Cores Apple System (Light Mode como base)
        private readonly Color AppleBg = Color.FromArgb(248, 248, 248);
        private readonly Color AppleText = Color.FromArgb(28, 28, 30);
        private readonly Color AppleBlue = Color.FromArgb(10, 132, 255);

        public FrmAetherMsg(string mensagem, bool isQuestion)
        {
            ConfigurarInterface(mensagem, isQuestion);
        }

        private void ConfigurarInterface(string mensagem, bool isQuestion)
        {
            Font fonteTexto = new Font("Segoe UI", 11, FontStyle.Regular);
            Size tamanhoMaximo = new Size(400, 800);
            Size tamanhoTexto = TextRenderer.MeasureText(mensagem, fonteTexto, tamanhoMaximo, TextFormatFlags.WordBreak);

            this.Size = new Size(420, tamanhoTexto.Height + 170);
            this.BackColor = AppleBg;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));

            // Label de Mensagem
            Label lbl = new Label
            {
                Text = mensagem,
                Font = fonteTexto,
                ForeColor = AppleText,
                TextAlign = ContentAlignment.TopCenter,
                Location = new Point(30, 40),
                Size = new Size(this.Width - 60, tamanhoTexto.Height + 20),
                AutoSize = false
            };
            this.Controls.Add(lbl);

            // Botões Estilo Apple
            Panel pnlBotoes = new Panel { Dock = DockStyle.Bottom, Height = 80, BackColor = Color.Transparent };
            this.Controls.Add(pnlBotoes);

            if (isQuestion)
            {
                Button btnSim = CriarBotao("Confirmar", AppleBlue, Color.White, 120);
                Button btnNao = CriarBotao("Cancelar", Color.FromArgb(230, 230, 230), AppleText, 120);

                btnSim.Location = new Point((this.Width / 2) - 130, 20);
                btnNao.Location = new Point((this.Width / 2) + 10, 20);

                btnSim.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };
                btnNao.Click += (s, e) => { this.DialogResult = DialogResult.No; this.Close(); };
                pnlBotoes.Controls.AddRange(new Control[] { btnSim, btnNao });
            }
            else
            {
                Button btnOk = CriarBotao("OK", AppleBlue, Color.White, 160);
                btnOk.Location = new Point((this.Width / 2) - 80, 20);
                btnOk.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                pnlBotoes.Controls.Add(btnOk);
            }

            this.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); };
        }

        private Button CriarBotao(string texto, Color back, Color fore, int largura)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = new Size(largura, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                // CORREÇÃO: Troque "Semibold" por "Bold" ou "Regular"
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            // Efeito visual de botão arredondado simples
            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 8, 8));
            return btn;
        }

        [DllImport("Gdi32.dll")] private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
    }
}