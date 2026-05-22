using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using Guna.UI2.WinForms; // Certifique-se de manter a referência do Guna ativa

namespace aether
{
    public partial class Sora : Form
    {
        // PInvoke mantido estritamente para o arraste fluido da janela customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        // Controles atualizados para a engine do Guna
        private Guna2Panel pnlMainBackground;
        private Guna2Panel pnlHeader;
        private RichTextBox rtbChat;
        private Guna2TextBox txtInput;
        private Guna2Button btnEnviar;
        private Guna2Button btnFechar;
        private Label lblTitulo;
        private Guna2Separator sepHeader;

        public Sora()
        {
            InitializeComponentCustom();
            this.Load += (s, e) => AdicionarMensagemIA("Conectado ao servidor LittleBoy-SORA34.welabs");
        }

        private void InitializeComponentCustom()
        {
            // CONFIGURAÇÃO DA JANELA PRINCIPAL (Cantos arredondados nativos do Guna sem serrilhado)
            this.Size = new Size(850, 720);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(10, 10, 12); // Fundo ultra-dark (quase preto, moderno)

            // CONTÊINER PRINCIPAL (Aplica o arredondamento perfeito na janela inteira)
            pnlMainBackground = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = Color.FromArgb(10, 10, 12),
                BorderColor = Color.FromArgb(255, 51, 51), // Borda sutil na cor de destaque do seu ecossistema
                BorderThickness = 1,
                BorderRadius = 16
            };
            this.Controls.Add(pnlMainBackground);

            // --- HEADER PREMIUM ---
            pnlHeader = new Guna2Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                FillColor = Color.FromArgb(15, 15, 18)
            };
            pnlHeader.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); };

            lblTitulo = new Label
            {
                Text = "SORA // AI CORE v0.1",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 26),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            btnFechar = new Guna2Button
            {
                Text = "EXIT",
                Size = new Size(90, 36),
                Location = new Point(730, 22),
                FillColor = Color.Transparent,
                ForeColor = Color.FromArgb(148, 163, 184), // Slate Gray
                Font = new Font("Consolas", 9.5F, FontStyle.Bold),
                BorderColor = Color.FromArgb(51, 65, 85),
                BorderThickness = 1,
                BorderRadius = 6,
                Cursor = Cursors.Hand
            };
            btnFechar.HoverState.BorderColor = Color.FromArgb(255, 51, 51);
            btnFechar.HoverState.FillColor = Color.FromArgb(30, 15, 15);
            btnFechar.HoverState.ForeColor = Color.FromArgb(255, 51, 51);
            btnFechar.Click += (s, e) => this.Close();

            sepHeader = new Guna2Separator
            {
                Location = new Point(0, 78),
                Size = new Size(850, 2),
                FillColor = Color.FromArgb(30, 30, 35)
            };

            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(btnFechar);
            pnlHeader.Controls.Add(sepHeader);
            pnlMainBackground.Controls.Add(pnlHeader);

            // --- DISPLAY DO CHAT (RichTextBox embutido em um painel com cantos suaves)
            Guna2Panel pnlChatWrapper = new Guna2Panel
            {
                Location = new Point(30, 110),
                Size = new Size(790, 470),
                FillColor = Color.FromArgb(15, 15, 18),
                BorderRadius = 12,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(25, 25, 30)
            };

            rtbChat = new RichTextBox
            {
                Location = new Point(15, 15),
                Size = new Size(760, 440),
                BackColor = Color.FromArgb(15, 15, 18),
                ForeColor = Color.FromArgb(226, 232, 240), // Texto claro de alta legibilidade
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 11),
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };
            pnlChatWrapper.Controls.Add(rtbChat);
            pnlMainBackground.Controls.Add(pnlChatWrapper);

            // --- CAIXA DE ENTRADA INDUSTRIAL (Guna TextBox) ---
            txtInput = new Guna2TextBox
            {
                Location = new Point(30, 605),
                Size = new Size(645, 65),
                BackColor = Color.Transparent,
                FillColor = Color.FromArgb(15, 15, 18),
                BorderColor = Color.FromArgb(38, 38, 45),
                BorderThickness = 1,
                BorderRadius = 10,
                Font = new Font("Consolas", 12),
                ForeColor = Color.White,
                PlaceholderText = "> SOLICITAR INSTRUÇÃO TÉCNICA...",
                PlaceholderForeColor = Color.FromArgb(100, 116, 139),
                TextOffset = new Point(10, 0)
            };
            // Efeito luminoso tecnológico ao focar na caixa de texto
            txtInput.FocusedState.BorderColor = Color.FromArgb(255, 51, 51);
            txtInput.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; await EnviarMensagem(); } };
            pnlMainBackground.Controls.Add(txtInput);

            // --- BOTÃO DE EXECUÇÃO ---
            btnEnviar = new Guna2Button
            {
                Text = "EXECUTE",
                Size = new Size(130, 65),
                Location = new Point(690, 605),
                FillColor = Color.FromArgb(255, 51, 51), // Sua cor padrão vermelha
                ForeColor = Color.White,
                Font = new Font("Consolas", 11, FontStyle.Bold),
                BorderRadius = 10,
                Cursor = Cursors.Hand,
                Animated = true
            };
            btnEnviar.HoverState.FillColor = Color.FromArgb(220, 38, 38); // Escurece levemente no hover
            btnEnviar.HoverState.CustomBorderColor = Color.FromArgb(255, 100, 100);
            btnEnviar.Click += async (s, e) => await EnviarMensagem();
            pnlMainBackground.Controls.Add(btnEnviar);
        }

        private void AdicionarMensagemIA(string texto)
        {
            if (rtbChat.InvokeRequired)
            {
                rtbChat.Invoke(new Action(() => AdicionarMensagemIA(texto)));
                return;
            }
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionColor = Color.FromArgb(255, 51, 51); // Tags do sistema em Vermelho Destaque
            rtbChat.SelectionFont = new Font("Consolas", 11, FontStyle.Bold);
            rtbChat.AppendText("[SORA]: ");
            rtbChat.SelectionColor = Color.FromArgb(226, 232, 240);
            rtbChat.SelectionFont = new Font("Consolas", 11);
            rtbChat.AppendText(texto + "\n\n");
            rtbChat.ScrollToCaret();
        }

        private void AdicionarMensagemUsuario(string texto)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionColor = Color.FromArgb(148, 163, 184); // Usuário em tom cinza-metálico limpo
            rtbChat.SelectionFont = new Font("Consolas", 11, FontStyle.Bold);
            rtbChat.AppendText("[USER]: ");
            rtbChat.SelectionColor = Color.White;
            rtbChat.SelectionFont = new Font("Consolas", 11);
            rtbChat.AppendText(texto + "\n\n");
            rtbChat.ScrollToCaret();
        }

        private async Task EnviarMensagem()
        {
            string pergunta = txtInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(pergunta)) return;

            AdicionarMensagemUsuario(pergunta);
            txtInput.Clear();
            txtInput.Enabled = false;

            string statusText = ">> AGUARDANDO RESPOSTA DO CORE...";
            rtbChat.AppendText(statusText);
            int statusStartPos = rtbChat.TextLength - statusText.Length;

            try
            {
                AetherAI engine = new AetherAI();
                string resposta = await engine.ObterResposta(pergunta);

                RemoverTextoStatus(statusStartPos, statusText.Length);
                AdicionarMensagemIA(resposta);
            }
            catch (Exception ex)
            {
                RemoverTextoStatus(statusStartPos, statusText.Length);
                AdicionarMensagemIA("ERRO DE CONEXÃO: " + ex.Message.ToUpper());
            }
            finally
            {
                txtInput.Enabled = true;
                txtInput.Focus();
            }
        }

        private void RemoverTextoStatus(int start, int length)
        {
            rtbChat.ReadOnly = false;
            rtbChat.Select(start, length);
            rtbChat.SelectedText = string.Empty;
            rtbChat.ReadOnly = true;
        }
    }
}