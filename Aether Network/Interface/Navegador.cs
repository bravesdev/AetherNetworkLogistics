using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.Threading.Tasks;

namespace aether
{
    public partial class Navegador : Form
    {
        // Componentes Estruturais Premium
        private Panel panelTop;
        private Panel panelTabs;
        private Panel panelUrlContainer;
        private TextBox txtUrl;
        private Button btnIr;
        private Button btnRefresh;
        private Button btnVoltar;
        private Button btnAvancar;
        private Button btnNovaAba;

        // Elementos da Animação de Entrada (Splash Screen Interna)
        private Panel panelSplash;
        private Label lblSplashTitle;
        private Panel panelSplashProgress;
        private int splashProgressWidth = 0;
        private System.Windows.Forms.Timer timerSplash;
        private System.Windows.Forms.Timer timerSpinner;

        // Controle de Abas e Animação de Carregamento (Spinner)
        private List<AbaNavegador> listaAbas = new List<AbaNavegador>();
        private AbaNavegador abaAtiva;
        private const string URL_PADRAO = "https://www.google.com/";

        private float spinnerAngulo = 0f;

        // Classe interna otimizada para as abas individuais
        private class AbaNavegador
        {
            public Button BotaoAba { get; set; }
            public Button BotaoFechar { get; set; }
            public Panel ContainerBotoes { get; set; }
            public WebView2 WebView { get; set; }
            public string UrlAtual { get; set; } = URL_PADRAO;
            public bool Carregando { get; set; } = false;
        }

        public Navegador()
        {
            InitializeComponent();
            ConfigurarInterfaceBase();
            ConfigurarAnimacaoEntrada(); // 1º: Instancia o panelSplash para que ele não seja null
            CriarNovaAba(URL_PADRAO);     // 2º: Cria a aba inicial com segurança
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Navegador));
            SuspendLayout();
            // 
            // Navegador
            // 
            ClientSize = new Size(1250, 850);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Navegador";
            ResumeLayout(false);
        }

        // --- 1. ANIMAÇÃO DE ABERTURA PREMIUM ---
        private void ConfigurarAnimacaoEntrada()
        {
            // Oculta os painéis principais inicialmente para dar lugar à animação
            panelTop.Visible = false;
            panelTabs.Visible = false;

            panelSplash = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(10, 10, 10),
                Location = new Point(0, 0),
                Size = this.ClientSize
            };

            lblSplashTitle = new Label
            {
                Text = "AETHER WEB",
                Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point((this.ClientSize.Width / 2) - 110, (this.ClientSize.Height / 2) - 60)
            };

            // Trilho da barra de progresso minimalista
            Panel panelTrack = new Panel
            {
                Size = new Size(250, 4),
                BackColor = Color.FromArgb(30, 30, 30),
                Location = new Point((this.ClientSize.Width / 2) - 125, (this.ClientSize.Height / 2) + 10)
            };

            panelSplashProgress = new Panel
            {
                Size = new Size(0, 4),
                BackColor = Color.FromArgb(255, 51, 51), // Vermelho Assinatura
                Location = new Point(0, 0)
            };

            panelTrack.Controls.Add(panelSplashProgress);
            panelSplash.Controls.Add(lblSplashTitle);
            panelSplash.Controls.Add(panelTrack);
            this.Controls.Add(panelSplash);
            panelSplash.BringToFront();

            // Centralização responsiva se redimensionar durante o splash
            this.Resize += (s, e) => {
                if (panelSplash != null && panelSplash.Visible)
                {
                    lblSplashTitle.Location = new Point((this.ClientSize.Width / 2) - 110, (this.ClientSize.Height / 2) - 60);
                    panelTrack.Location = new Point((this.ClientSize.Width / 2) - 125, (this.ClientSize.Height / 2) + 10);
                }
            };

            // Inicializa o timer da animação de entrada
            timerSplash = new System.Windows.Forms.Timer { Interval = 15 };
            timerSplash.Tick += TimerSplash_Tick;
            timerSplash.Start();
        }

        private void TimerSplash_Tick(object sender, EventArgs e)
        {
            if (splashProgressWidth < 250)
            {
                splashProgressWidth += 8; // Velocidade da animação
                if (splashProgressWidth > 250) splashProgressWidth = 250;
                panelSplashProgress.Width = splashProgressWidth;
            }
            else
            {
                timerSplash.Stop();
                FinalizarSplashComEfeito();
            }
        }

        private async void FinalizarSplashComEfeito()
        {
            // Efeito suave de esmaecimento (Fade Out) simulado
            await Task.Delay(200);

            this.Controls.Remove(panelSplash);
            panelSplash.Dispose();

            // Exibe a interface com comportamento fluido
            panelTabs.Visible = true;
            panelTop.Visible = true;
            if (abaAtiva != null) abaAtiva.WebView.Visible = true;
        }

        // --- 2. CONFIGURAÇÃO VISUAL DA INTERFACE ---
        private void ConfigurarInterfaceBase()
        {
            this.Text = "Aether Premium Web";
            this.BackColor = Color.FromArgb(10, 10, 10);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Timer global para a animação de rotação das bolinhas (Spinners)
            timerSpinner = new System.Windows.Forms.Timer { Interval = 30 };
            timerSpinner.Tick += (s, e) => {
                spinnerAngulo += 15f;
                if (spinnerAngulo >= 360f) spinnerAngulo = 0f;

                // Força apenas as abas que estão carregando a se redesenharem
                foreach (var aba in listaAbas)
                {
                    if (aba.Carregando) aba.ContainerBotoes.Invalidate();
                }
            };
            timerSpinner.Start();

            // Barra de Abas Superior Premium
            panelTabs = new Panel
            {
                Dock = DockStyle.Top,
                Height = 38,
                BackColor = Color.FromArgb(14, 14, 14),
                Padding = new Padding(10, 5, 10, 0)
            };

            btnNovaAba = new Button
            {
                Size = new Size(28, 26),
                Text = "＋",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(22, 22, 22),
                ForeColor = Color.FromArgb(160, 160, 160),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnNovaAba.FlatAppearance.BorderSize = 0;
            btnNovaAba.MouseEnter += (s, e) => { btnNovaAba.BackColor = Color.FromArgb(35, 35, 35); btnNovaAba.ForeColor = Color.White; };
            btnNovaAba.MouseLeave += (s, e) => { btnNovaAba.BackColor = Color.FromArgb(22, 22, 22); btnNovaAba.ForeColor = Color.FromArgb(160, 160, 160); };
            btnNovaAba.Click += (s, e) => CriarNovaAba(URL_PADRAO);
            panelTabs.Controls.Add(btnNovaAba);

            // Toolbar Otimizada
            panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                BackColor = Color.FromArgb(20, 20, 20),
                Padding = new Padding(15, 11, 15, 11)
            };

            // Botões de Comando Minimalistas
            btnVoltar = CriarBotaoNavegacao("◀", new Point(15, 11), 8F);
            btnVoltar.Click += (s, e) => { if (abaAtiva?.WebView?.CoreWebView2 != null && abaAtiva.WebView.CoreWebView2.CanGoBack) abaAtiva.WebView.CoreWebView2.GoBack(); };

            btnAvancar = CriarBotaoNavegacao("▶", new Point(52, 11), 8F);
            btnAvancar.Click += (s, e) => { if (abaAtiva?.WebView?.CoreWebView2 != null && abaAtiva.WebView.CoreWebView2.CanGoForward) abaAtiva.WebView.CoreWebView2.GoForward(); };

            btnRefresh = CriarBotaoNavegacao("⟳", new Point(95, 11), 11F);
            btnRefresh.Click += (s, e) => abaAtiva?.WebView?.CoreWebView2?.Reload();

            // Container Arredondado para a URL Bar
            panelUrlContainer = new Panel
            {
                Location = new Point(140, 11),
                Size = new Size(930, 32),
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(15, 8, 15, 7)
            };
            panelUrlContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            // Tratamento de desenho GDI+ para arredondar a URL Bar
            panelUrlContainer.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = ObterCaminhoArredondado(new Rectangle(0, 0, panelUrlContainer.Width - 1, panelUrlContainer.Height - 1), 6))
                using (Pen p = new Pen(Color.FromArgb(45, 45, 45), 1))
                {
                    e.Graphics.DrawPath(p, path);
                }
            };

            txtUrl = new TextBox
            {
                Text = URL_PADRAO,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular)
            };
            txtUrl.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) ExecutarNavegacaoInteligente(txtUrl.Text); };
            panelUrlContainer.Controls.Add(txtUrl);

            // Botão de Ação Assinatura
            btnIr = new Button
            {
                Text = "Acessar",
                Size = new Size(95, 32),
                Location = new Point(1085, 11),
                BackColor = Color.FromArgb(255, 51, 51),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnIr.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnIr.FlatAppearance.BorderSize = 0;
            btnIr.Click += (s, e) => ExecutarNavegacaoInteligente(txtUrl.Text);

            // Montagem da Estrutura
            panelTop.Controls.Add(btnVoltar);
            panelTop.Controls.Add(btnAvancar);
            panelTop.Controls.Add(btnRefresh);
            panelTop.Controls.Add(panelUrlContainer);
            panelTop.Controls.Add(btnIr);

            this.Controls.Add(panelTop);
            this.Controls.Add(panelTabs);
        }

        private Button CriarBotaoNavegacao(string texto, Point local, float fontSize)
        {
            Button btn = new Button
            {
                Size = new Size(32, 32),
                Location = local,
                BackColor = Color.FromArgb(26, 26, 26),
                ForeColor = Color.DarkGray,
                FlatStyle = FlatStyle.Flat,
                Text = texto,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(38, 38, 38);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(26, 26, 26);
            return btn;
        }

        // --- 3. LÓGICA DE ABAS AVANÇADA (COM SPINNER DE CARREGAMENTO) ---
        private void CriarNovaAba(string urlInicial)
        {
            var novaAba = new AbaNavegador();

            // Container customizado via GDI+ para desenhar abas arredondadas e a bolinha de loading
            novaAba.ContainerBotoes = new Panel
            {
                Height = 33,
                Width = 165,
                BackColor = Color.FromArgb(14, 14, 14),
                Padding = new Padding(0)
            };

            // Tratamento customizado de pintura da aba para renderizar o Spinner dinamicamente
            novaAba.ContainerBotoes.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                bool ativa = (abaAtiva == novaAba);
                Color corFundo = ativa ? Color.FromArgb(20, 20, 20) : Color.FromArgb(14, 14, 14);

                // Desenha o fundo arredondado da aba estilizada
                using (SolidBrush pb = new SolidBrush(corFundo))
                using (GraphicsPath path = ObterCaminhoArredondadoSuperior(new Rectangle(0, 0, novaAba.ContainerBotoes.Width, novaAba.ContainerBotoes.Height), 6))
                {
                    e.Graphics.FillPath(pb, path);
                }

                // Se a página estiver carregando, renderiza a bolinha giratória (Spinner)
                if (novaAba.Carregando)
                {
                    e.Graphics.Save();
                    // Define a posição da bolinha no canto esquerdo da aba
                    e.Graphics.TranslateTransform(15, 16);
                    e.Graphics.RotateTransform(spinnerAngulo);

                    using (Pen spinnerPen = new Pen(Color.FromArgb(255, 51, 51), 2))
                    {
                        spinnerPen.DashStyle = DashStyle.Solid;
                        // Desenha um arco de 270 graus para criar o visual aberto do círculo de loading
                        e.Graphics.DrawArc(spinnerPen, -6, -6, 12, 12, 0, 270);
                    }
                }
            };

            novaAba.BotaoAba = new Button
            {
                Width = 130,
                Height = 33,
                Dock = DockStyle.Left,
                Text = "   Nova guia",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BackColor = Color.Transparent,
                ForeColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            novaAba.BotaoAba.FlatAppearance.BorderSize = 0;
            novaAba.BotaoAba.FlatAppearance.MouseDownBackColor = Color.Transparent;
            novaAba.BotaoAba.FlatAppearance.MouseOverBackColor = Color.Transparent;
            novaAba.BotaoAba.Click += (s, e) => SelecionarAba(novaAba);

            novaAba.BotaoFechar = new Button
            {
                Width = 30,
                Height = 33,
                Dock = DockStyle.Right,
                Text = "✕",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            novaAba.BotaoFechar.FlatAppearance.BorderSize = 0;
            novaAba.BotaoFechar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            novaAba.BotaoFechar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            novaAba.BotaoFechar.Click += (s, e) => FecharAba(novaAba);
            novaAba.BotaoFechar.MouseEnter += (s, e) => novaAba.BotaoFechar.ForeColor = Color.FromArgb(255, 80, 80);
            novaAba.BotaoFechar.MouseLeave += (s, e) => novaAba.BotaoFechar.ForeColor = Color.Gray;

            novaAba.ContainerBotoes.Controls.Add(novaAba.BotaoAba);
            novaAba.ContainerBotoes.Controls.Add(novaAba.BotaoFechar);

            // Instanciação da Engine Web
            novaAba.WebView = new WebView2
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(10, 10, 10),
                Visible = false
            };

            // Gerenciamento de Estados de Carregamento para o Spinner
            novaAba.WebView.NavigationStarting += (s, e) => {
                novaAba.Carregando = true;
                // Ajusta o padding do texto para dar espaço à bolinha de loading
                novaAba.BotaoAba.Text = "        Carregando...";
                novaAba.ContainerBotoes.Invalidate();
            };

            novaAba.WebView.SourceChanged += (s, e) => {
                if (novaAba.WebView.Source != null)
                {
                    novaAba.UrlAtual = novaAba.WebView.Source.ToString();
                    if (abaAtiva == novaAba) txtUrl.Text = novaAba.UrlAtual;
                }
            };

            novaAba.WebView.NavigationCompleted += (s, e) => {
                novaAba.Carregando = false;
                AtualizarTituloAba(novaAba);
                if (abaAtiva == novaAba) MonitorarBotoesDeNavegacao();
            };

            // Adiciona os controles em background de forma segura
            this.Controls.Add(novaAba.WebView);

            // Correção de segurança contra NullReference: Só chama BringToFront se o panelSplash já existir e não estiver visível
            if (panelSplash != null && !panelSplash.Visible)
                novaAba.WebView.BringToFront();

            listaAbas.Add(novaAba);
            panelTabs.Controls.Add(novaAba.ContainerBotoes);

            InicializarMotorAba(novaAba, urlInicial);
            OrganizarLayoutAbas();
            SelecionarAba(novaAba);
        }

        private async void InicializarMotorAba(AbaNavegador aba, string url)
        {
            try
            {
                await aba.WebView.EnsureCoreWebView2Async(null);
                aba.WebView.CoreWebView2.Profile.PreferredColorScheme = CoreWebView2PreferredColorScheme.Dark;

                // Vinculação correta e nativa do título da página
                aba.WebView.CoreWebView2.DocumentTitleChanged += (s, e) => {
                    this.Invoke((MethodInvoker)delegate {
                        AtualizarTituloAba(aba);
                    });
                };

                aba.WebView.CoreWebView2.Navigate(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha na Engine Aether: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarTituloAba(AbaNavegador aba)
        {
            if (aba.Carregando) return;

            string tituloReal = aba.WebView.CoreWebView2?.DocumentTitle;
            if (string.IsNullOrEmpty(tituloReal)) tituloReal = "Nova guia";

            if (tituloReal.Length > 15)
                tituloReal = tituloReal.Substring(0, 12) + "...";

            // Se não estiver carregando, o texto volta para a margem padrão (espaço reduzido)
            aba.BotaoAba.Text = "   " + tituloReal;
            aba.ContainerBotoes.Invalidate();
        }

        private void SelecionarAba(AbaNavegador abaAlvo)
        {
            if (abaAlvo == null) return;

            abaAtiva = abaAlvo;
            txtUrl.Text = abaAtiva.UrlAtual;

            foreach (var aba in listaAbas)
            {
                bool eAbaAtiva = (aba == abaAlvo);

                // Os fundos e estilos do container são tratados via evento Paint usando herança transparente
                aba.BotaoAba.Font = new Font("Segoe UI", 9F, eAbaAtiva ? FontStyle.Bold : FontStyle.Regular);
                aba.BotaoAba.ForeColor = eAbaAtiva ? Color.White : Color.FromArgb(150, 150, 150);

                // Correção contra NullReference: Verifica se o Splash Screen já foi destruído ou está oculto
                if (panelSplash == null || !panelSplash.Visible)
                {
                    aba.WebView.Visible = eAbaAtiva;
                }
                aba.ContainerBotoes.Invalidate(); // Força a renderização com as novas cores reativas
            }

            MonitorarBotoesDeNavegacao();
        }

        private void FecharAba(AbaNavegador abaAlvo)
        {
            if (listaAbas.Count <= 1)
            {
                abaAlvo.WebView.CoreWebView2?.Navigate(URL_PADRAO);
                return;
            }

            int indexAbaFechada = listaAbas.IndexOf(abaAlvo);

            listaAbas.Remove(abaAlvo);
            panelTabs.Controls.Remove(abaAlvo.ContainerBotoes);
            this.Controls.Remove(abaAlvo.WebView);
            abaAlvo.WebView.Dispose();

            if (abaAtiva == abaAlvo)
            {
                int novoIndex = Math.Max(0, indexAbaFechada - 1);
                SelecionarAba(listaAbas[novoIndex]);
            }

            OrganizarLayoutAbas();
        }

        private void OrganizarLayoutAbas()
        {
            int posicaoX = 15;
            int espacamento = 2;

            foreach (var aba in listaAbas)
            {
                aba.ContainerBotoes.Location = new Point(posicaoX, 5);
                posicaoX += aba.ContainerBotoes.Width + espacamento;
            }

            btnNovaAba.Location = new Point(posicaoX + 4, 8);
        }

        private void ExecutarNavegacaoInteligente(string entrada)
        {
            string urlDestino = entrada.Trim();
            if (string.IsNullOrEmpty(urlDestino) || abaAtiva == null) return;

            if (urlDestino.Contains(" "))
            {
                urlDestino = "https://www.google.com/search?q=" + Uri.EscapeDataString(urlDestino);
            }
            else
            {
                if (!urlDestino.StartsWith("http://") && !urlDestino.StartsWith("https://"))
                {
                    if (!urlDestino.Contains(".")) urlDestino += ".com";
                    urlDestino = "https://" + urlDestino;
                }
            }

            txtUrl.Text = urlDestino;
            abaAtiva.WebView.CoreWebView2?.Navigate(urlDestino);
        }

        private void MonitorarBotoesDeNavegacao()
        {
            if (abaAtiva?.WebView?.CoreWebView2 != null)
            {
                btnVoltar.Enabled = abaAtiva.WebView.CoreWebView2.CanGoBack;
                btnVoltar.ForeColor = abaAtiva.WebView.CoreWebView2.CanGoBack ? Color.White : Color.FromArgb(65, 65, 65);

                btnAvancar.Enabled = abaAtiva.WebView.CoreWebView2.CanGoForward;
                btnAvancar.ForeColor = abaAtiva.WebView.CoreWebView2.CanGoForward ? Color.White : Color.FromArgb(65, 65, 65);
            }
        }

        // --- 4. FUNÇÕES DE SUPORTE GRÁFICO (GDI+ VETORIAL) ---
        private GraphicsPath ObterCaminhoArredondado(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private GraphicsPath ObterCaminhoArredondadoSuperior(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);

            path.AddArc(arc, 180, 90); // Canto Superior Esquerdo
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90); // Canto Superior Direito
            path.AddLine(bounds.Right, bounds.Bottom, bounds.Left, bounds.Bottom); // Fecha a base reta
            path.CloseFigure();
            return path;
        }
    }
}