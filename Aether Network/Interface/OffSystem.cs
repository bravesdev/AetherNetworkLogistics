using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace aether.Interface
{
    // A classe principal do Form permanece no topo para assegurar o funcionamento do Designer do Visual Studio!
    public partial class OffSystem : Form
    {
        // --- CONFIGURAÇÃO MANUAL DO DESENVOLVEDOR ---
        private const string URL_API_NPOINT = "https://api.npoint.io/3205904552578c16bf73";

        // Componentes da Interface Exclusiva Apple Light Mode
        private Panel pnlCenterCard;
        private Label lblBrand;
        private Label lblStatus;
        private TextBox txtDevNote;
        private Button btnAction;
        private Button btnExit;
        private Label lblFooter;

        // Paleta de Cores Oficial Apple Ecosystem
        private readonly Color colorBg = Color.FromArgb(245, 245, 247);            // Fundo cinza-claro/prata (macOS System Background)
        private readonly Color colorCard = Color.FromArgb(255, 255, 255);          // Card puramente branco plano
        private readonly Color colorTextPrimary = Color.FromArgb(29, 29, 31);      // Texto principal (Apple Standard Onyx)
        private readonly Color colorTextMuted = Color.FromArgb(134, 134, 139);     // Texto secundário/Muted
        private readonly Color colorAccentBlue = Color.FromArgb(0, 122, 255);      // Azul clássico de ação (iOS/macOS Accent)
        private readonly Color colorAccentBlueHover = Color.FromArgb(0, 105, 225); // Azul dinâmico para foco
        private readonly Color colorBorder = Color.FromArgb(210, 210, 215);        // Linha divisória fina e elegante

        public OffSystem()
        {
            InitializeComponent();
            SetupPureAppleUI();
            CarregarNotaDoDesenvolvedor();
        }

        private void SetupPureAppleUI()
        {
            // 1. Configurações Globais da Janela (Modo Tela Cheia)
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = colorBg;
            this.DoubleBuffered = true;
            this.SizeChanged += (s, e) => ReposicionarElementos();

            // 2. Card de Diálogo Centralizado (Padrão Clean UI)
            pnlCenterCard = new Panel
            {
                Size = new Size(460, 520),
                BackColor = colorCard
            };
            pnlCenterCard.Paint += RenderCardBordersAndCorners;

            // 3. Identidade Visual e Status
            lblBrand = new Label
            {
                Text = "Aether",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                ForeColor = colorTextPrimary,
                Location = new Point(0, 45),
                Size = new Size(460, 45),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblStatus = new Label
            {
                Text = "Aviso do Sistema",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = colorTextMuted,
                Location = new Point(0, 95),
                Size = new Size(460, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 4. Visualizador de Texto Integrado (Sem bordas, caixa limpa com scroll invisível/suave)
            txtDevNote = new TextBox
            {
                Text = "Sincronizando os comunicados da infraestrutura...",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 51, 54),
                BackColor = colorCard,
                BorderStyle = BorderStyle.None,
                Multiline = true,
                ReadOnly = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(40, 140),
                Size = new Size(380, 220)
            };

            // 5. Botão de Ação Primária (Estilo Arredondado Sólido da Apple)
            btnAction = new Button
            {
                Text = "Atualizar Informações",
                Location = new Point(40, 390),
                Size = new Size(380, 44),
                BackColor = colorAccentBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAction.FlatAppearance.BorderSize = 0;
            btnAction.MouseEnter += (s, e) => btnAction.BackColor = colorAccentBlueHover;
            btnAction.MouseLeave += (s, e) => btnAction.BackColor = colorAccentBlue;
            btnAction.Click += (s, e) => CarregarNotaDoDesenvolvedor();

            // 6. Opção de Saída (Link Minimalista que muda para vermelho ao passar o mouse)
            btnExit = new Button
            {
                Text = "Encerrar Aplicação",
                Location = new Point(40, 445),
                Size = new Size(380, 40),
                BackColor = colorCard,
                ForeColor = colorTextMuted,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnExit.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnExit.MouseEnter += (s, e) => btnExit.ForeColor = Color.FromArgb(255, 69, 58);
            btnExit.MouseLeave += (s, e) => btnExit.ForeColor = colorTextMuted;
            btnExit.Click += (s, e) => Application.Exit();

            // 7. Rodapé Discreto de Segurança
            lblFooter = new Label
            {
                Text = "Ambiente de Infraestrutura Automatizado • Lab/LogReversa",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = colorTextMuted,
                Size = new Size(600, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Montagem dos elementos na tela
            pnlCenterCard.Controls.AddRange(new Control[] { lblBrand, lblStatus, txtDevNote, btnAction, btnExit });
            this.Controls.Add(pnlCenterCard);
            this.Controls.Add(lblFooter);

            ReposicionarElementos();
        }

        private void ReposicionarElementos()
        {
            if (pnlCenterCard != null)
            {
                pnlCenterCard.Location = new Point(
                    (this.ClientSize.Width - pnlCenterCard.Width) / 2,
                    (this.ClientSize.Height - pnlCenterCard.Height) / 2
                );
            }

            if (lblFooter != null)
            {
                lblFooter.Location = new Point(
                    (this.ClientSize.Width - lblFooter.Width) / 2,
                    this.ClientSize.Height - 40
                );
            }
        }

        // Desenha as bordas finas e suaviza cantos do container (Estilo caixas de diálogo do macOS)
        private void RenderCardBordersAndCorners(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int radius = 16;
            Rectangle rect = new Rectangle(0, 0, pnlCenterCard.Width - 1, pnlCenterCard.Height - 1);

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                pnlCenterCard.Region = new Region(path);

                using (Pen pen = new Pen(colorBorder, 1.2F))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private async void CarregarNotaDoDesenvolvedor()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(6);
                    string jsonResponse = await client.GetStringAsync(URL_API_NPOINT);

                    string tokenBusca = "\"mensagem\"";
                    int indexToken = jsonResponse.IndexOf(tokenBusca);

                    if (indexToken != -1)
                    {
                        int indexDoisPontos = jsonResponse.IndexOf(":", indexToken + tokenBusca.Length);
                        if (indexDoisPontos != -1)
                        {
                            int primeiraAspas = jsonResponse.IndexOf("\"", indexDoisPontos);
                            if (primeiraAspas != -1)
                            {
                                int segundaAspas = jsonResponse.IndexOf("\"", primeiraAspas + 1);
                                if (segundaAspas != -1)
                                {
                                    string mensagemExtraida = jsonResponse.Substring(primeiraAspas + 1, segundaAspas - primeiraAspas - 1);
                                    txtDevNote.Text = Regex.Unescape(mensagemExtraida).Replace("\\n", "\r\n");
                                    return;
                                }
                            }
                        }
                    }
                    txtDevNote.Text = "Nenhum comunicado ativo no momento.";
                }
            }
            catch (Exception)
            {
                txtDevNote.Text = "Não foi possível sincronizar os dados com o servidor.\r\n\r\nPor favor, recorra aos procedimentos manuais padrão da unidade caso precise dar andamento às rotinas operacionais.";
            }
        }
    }
}