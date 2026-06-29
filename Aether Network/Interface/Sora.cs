using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using Guna.UI2.WinForms;
using aether.Controle;

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
            ThemeManager.InitializeTheme(this);
            InitializeComponentCustom();
            this.Load += (s, e) => AdicionarMensagemIA("Bem-vindo à Sora! Para garantir a melhor experiência, pedimos que mantenha um tom respeitoso e cordial em nossas interações. Recomendamos a leitura da nossa Política de Uso para evitar suspensões preventivas.");
        }

        private void InitializeComponentCustom()
        {
            ThemeManager.InitializeTheme(this);
            // CONFIGURAÇÃO DA JANELA PRINCIPAL (Cantos arredondados nativos do Guna)
            this.Size = new Size(850, 720);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Cores base do Ecossistema Apple Light Mode
            Color appleBg = Color.FromArgb(242, 242, 247);          // System Gray 6 (Fundo de respiro)
            Color appleSurface = Color.White;                       // Superfícies e Cards puros
            Color appleTextPrimary = Color.FromArgb(0, 0, 0);       // Texto principal
            Color appleTextSecondary = Color.FromArgb(142, 142, 147); // Texto secundário / Slate Gray alternativo
            Color appleAccentBlue = Color.FromArgb(10, 132, 255);    // Azul Vivid Apple
            Color appleAccentHover = Color.FromArgb(0, 115, 230);   // Azul escurecido para feedback
            Color appleBorder = Color.FromArgb(218, 218, 222);      // Borda sutil interna

            this.BackColor = appleBg;

            // CONTÊINER PRINCIPAL (Aplica o arredondamento perfeito na janela inteira)
            pnlMainBackground = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                FillColor = appleBg,
                BorderColor = appleBorder,
                BorderThickness = 1,
                BorderRadius = 16
            };
            this.Controls.Add(pnlMainBackground);

            // --- HEADER PREMIUM ---
            pnlHeader = new Guna2Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                FillColor = appleSurface
            };
            pnlHeader.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); };

            lblTitulo = new Label
            {
                Text = "SORA // AI CORE v0.1",
                Font = new Font("SF Pro Display", 13, FontStyle.Bold), // Tipografia Apple nativa
                ForeColor = appleTextPrimary,
                Location = new Point(30, 28),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            btnFechar = new Guna2Button
            {
                Text = "EXIT",
                Size = new Size(90, 36),
                Location = new Point(730, 22),
                FillColor = Color.FromArgb(239, 239, 244), // Botão secundário sutil
                ForeColor = appleTextSecondary,
                Font = new Font("SF Pro Text", 9F, FontStyle.Bold),
                BorderRadius = 18, // Estilo pílula do iOS/macOS
                Cursor = Cursors.Hand,
                Animated = true
            };
            btnFechar.HoverState.FillColor = Color.FromArgb(255, 59, 48); // Vermelho destrutivo Apple no hover
            btnFechar.HoverState.ForeColor = Color.White;
            btnFechar.Click += (s, e) => this.Close();

            sepHeader = new Guna2Separator
            {
                Location = new Point(0, 78),
                Size = new Size(850, 2),
                FillColor = Color.FromArgb(229, 229, 234) // Divisor claro sutil
            };

            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(btnFechar);
            pnlHeader.Controls.Add(sepHeader);
            pnlMainBackground.Controls.Add(pnlHeader);

            // --- DISPLAY DO CHAT (RichTextBox embutido em um painel branco limpo)
            Guna2Panel pnlChatWrapper = new Guna2Panel
            {
                Location = new Point(30, 110),
                Size = new Size(790, 470),
                FillColor = appleSurface,
                BorderRadius = 12,
                BorderThickness = 1,
                BorderColor = Color.FromArgb(229, 229, 234)
            };

            rtbChat = new RichTextBox
            {
                Location = new Point(15, 15),
                Size = new Size(760, 440),
                BackColor = appleSurface,
                ForeColor = Color.FromArgb(30, 30, 30), // Escuro de alta legibilidade no fundo branco
                BorderStyle = BorderStyle.None,
                Font = new Font("SF Pro Text", 10.5F),
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };
            pnlChatWrapper.Controls.Add(rtbChat);
            pnlMainBackground.Controls.Add(pnlChatWrapper);

            // --- CAIXA DE ENTRADA MODERNIZADA (Guna TextBox Estilo Pílula) ---
            txtInput = new Guna2TextBox
            {
                Location = new Point(30, 605),
                Size = new Size(645, 50), // Ajustado ligeiramente para proporção visual harmoniosa
                BackColor = Color.Transparent,
                FillColor = appleSurface,
                BorderColor = Color.FromArgb(212, 212, 217),
                BorderThickness = 1,
                BorderRadius = 18, // Formato arredondado iMessage
                Font = new Font("SF Pro Text", 10.5F),
                ForeColor = appleTextPrimary,
                PlaceholderText = "Solicitar instrução técnica...",
                PlaceholderForeColor = appleTextSecondary,
                TextOffset = new Point(10, 0)
            };
            txtInput.FocusedState.BorderColor = appleAccentBlue;
            txtInput.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; await EnviarMensagem(); } };
            pnlMainBackground.Controls.Add(txtInput);

            // --- BOTÃO DE EXECUÇÃO PRIMÁRIO ---
            btnEnviar = new Guna2Button
            {
                Text = "EXECUTE",
                Size = new Size(130, 50),
                Location = new Point(690, 605),
                FillColor = appleAccentBlue, // Substituído o vermelho agressivo pelo Azul de Sistema
                ForeColor = Color.White,
                Font = new Font("SF Pro Text", 10F, FontStyle.Bold),
                BorderRadius = 18,
                Cursor = Cursors.Hand,
                Animated = true
            };
            btnEnviar.HoverState.FillColor = appleAccentHover;
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
            rtbChat.SelectionColor = Color.FromArgb(10, 132, 255); // Marcação da IA em Azul Apple Accent
            rtbChat.SelectionFont = new Font("SF Pro Text", 10.5F, FontStyle.Bold);
            rtbChat.AppendText("[SORA]: ");
            rtbChat.SelectionColor = Color.FromArgb(30, 30, 30);
            rtbChat.SelectionFont = new Font("SF Pro Text", 10.5F);
            rtbChat.AppendText(texto + "\n\n");
            rtbChat.ScrollToCaret();
        }

        private void AdicionarMensagemUsuario(string texto)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionColor = Color.FromArgb(142, 142, 147); // Marcação neutra em Cinza de Sistema
            rtbChat.SelectionFont = new Font("SF Pro Text", 10.5F, FontStyle.Bold);
            rtbChat.AppendText("[USER]: ");
            rtbChat.SelectionColor = Color.Black;
            rtbChat.SelectionFont = new Font("SF Pro Text", 10.5F);
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