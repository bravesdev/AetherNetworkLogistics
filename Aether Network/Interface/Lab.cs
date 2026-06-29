using aether;
using aether.Controle;
using aether.Interface;
using aether.Properties;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms;
using static aether.Controle.ThemeManager;
using static Guna.UI2.Native.WinApi;

namespace aether
{
    public partial class Lab : Form
    {
        // --- CONFIGURAÇÕES E CHAMADAS INICIAIS---
        [DllImport("kernel32.dll")]
        // APIs para manipular a janela do Console
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        // Comandos de exibição
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        private Chat _instanciaChat;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        // Comandos de exibição
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;
        private const string CompilatedVersion = "26.6.23";
        private const string UrlUpdate = "https://api.npoint.io/1deba0c7983047e931a3";
        private const string UrlBlockedSystem = "https://api.npoint.io/3205904552578c16bf73";
        public const string NomeArquivoLogs = "metadados\\screening-list.json";
        private const string ArquivoIPs = "metadados\\ping-ip.json";
        private const string ArquivoVersaoLocal = "metadados\\version.json";
        private System.Windows.Forms.Timer timerMonitorRede;
        private Stopwatch watch = new Stopwatch();
        private Server _athNet;
        private Chat _athChat;
        private List<string> _tecnicosOnline = new List<string>();


        private List<RegistroEquipamento> logsGerais = new List<RegistroEquipamento>();
        private System.Windows.Forms.Timer timerRelogio;
        private System.Windows.Forms.Timer timerPing;
        private System.Windows.Forms.Timer timerBloq;
        private HttpClient? httpClient;
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        public Lab()
        {

            InitializeComponent();

            this.FormClosing += AetherClose_FormClosing;
            _athNet = new Server(Environment.UserName);
            _athNet.OnMensagemRecebida += (msg) =>
            {
 
                this.Invoke(new Action(() =>
                {
                }));
            };
            _athNet.Conectar();
            _athNet.OnMensagemRecebida += (msg) =>
            {
                this.Invoke(new Action(() =>
                {
                    ProcessarTarefaRecebida(msg);
                }));
            };

            _athNet.OnListaTecnicosRecebida += (lista) =>
            {
                this.Invoke(new Action(() =>
                {
                    _tecnicosOnline = lista; // Atualiza a variável que vai pro ComboBox
                }));
            };
            _instanciaChat = new Chat();
            dgvLogs.SelectionMode = (DataGridViewSelectionMode)SelectionMode.MultiExtended;
            ThemeManager.InitializeTheme(this);
            ConfigurarEventosInterfaceIP();
            timerMonitorRede = new System.Windows.Forms.Timer { Interval = 1000 };
            timerMonitorRede.Tick += (s, e) => AtualizarVelocidadeDoLink();
            timerMonitorRede.Start();

            string nomeOperador = ObterNomeExibicao().ToUpper();
            ConfigurarTerminalDebug();
            ToolTip toolTipDinamico = new ToolTip();
            // 1. Esconde a barra 3D nativa (mas mantém a lógica de scroll ativa)

            // Configurações opcionais para ficar mais profissional
            toolTipDinamico.AutoPopDelay = 5000; // Tempo que o texto fica visível (5 seg)
            toolTipDinamico.InitialDelay = 500;  // Meio segundo para aparecer
            toolTipDinamico.ReshowDelay = 500;    // Tempo entre um botão e outro
            toolTipDinamico.ShowAlways = true;    // Força exibir mesmo se a janela não estiver em foco

            AetherLogger.MonitorResources(); // <--- Adicione isso


            // Define os textos para os seus botões
            toolTipDinamico.SetToolTip(btnAetherAI, "Assistente de IA Sora.");
            toolTipDinamico.SetToolTip(btnChat, "Chat rápido.");
            toolTipDinamico.SetToolTip(btnAjustes, "Configurações do Aether Network.");

            this.Text = $"Aether Network v{CompilatedVersion}";
            lblVersion.Text = $"{CompilatedVersion}";

            if (_athChat == null || _athChat.IsDisposed)
            {
                _athChat = new Chat();
                _athChat.ExibirChat();
            }

            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.MultiSelect = true; // Permite selecionar várias linhas
            dgvLogs.CellDoubleClick += DgvLogs_CellDoubleClick;
            // Registra os encodings extras (como o 850 do CMD)
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // ==== CHAMADAS ====
            ConfigurarRelogio();
            ConfigurarTimerPing();
            ConfigurarLayoutPrompt();
            GerarLogsInicializacao();
            CarregarLogsIniciais();
            CarregarIPsSalvos();
            AtualizarInterface();
            SincronizarVersaoLocal();
            Atualizacao();

            txtPesquisa.TextChanged += TxtPesquisa_TextChanged;
            txtPesquisa.PlaceholderText = "Pesquisar identificador";
            txtIdentificador.PlaceholderText = "Digite o identificador...";
            // Procure onde estão os outros eventos e adicione esta linha:
            txtIdentificador.KeyDown += txtIdentificador_KeyDown;

            // Eventos ao carregar o formulário
            this.Load += async (s, e) =>
            {
                await ExecutarVerificacoesRemotas();
            };
        }
        private void SincronizarVersaoLocal()
        {
            try
            {
                if (!File.Exists(ArquivoVersaoLocal) || JsonSerializer.Deserialize<VersaoModel>(File.ReadAllText(ArquivoVersaoLocal))?.versao != CompilatedVersion)
                    File.WriteAllText(ArquivoVersaoLocal, JsonSerializer.Serialize(new VersaoModel { versao = CompilatedVersion }));
            }
            catch { }
        }
        private async Task Atualizacao()
        {

            try
            {
                string json = await _httpClient.GetStringAsync(UrlUpdate);
                var dados = JsonSerializer.Deserialize<InfoUpdate>(json);

                if (new Version(dados.versao) > new Version(CompilatedVersion))
                {
                    string msg = $"NOVA VERSÃO DISPONÍVEL: {dados.versao}\n\nNOTAS:\n{dados.notas}\n\nDeseja atualizar?";

                    if (Msg.Question(msg))
                    {
                        string updater = Path.Combine(Application.StartupPath, "welabs-update.exe");

                        if (File.Exists(updater))
                        {
                            PopupForm.Show(this, "Iniciando atualização...");
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = updater,
                                Arguments = dados.url_zip,
                                UseShellExecute = true,
                                Verb = "runas"
                            });
                            Application.Exit();
                        }
                    }
                }
            }
            catch { PopupForm.Show(this, $"Erro ao iniciar atualização."); }

        }


        private async System.Threading.Tasks.Task ExecutarVerificacoesRemotas()
        {
            // 1. Verifica Bloqueio primeiro
            await ValidarStatusSistema();
        }
        private void ConfigurarTerminalDebug()
        {
            // 1. Cria o console para o processo
            AllocConsole();

            // 2. Pega o "ID" da janela do console
            IntPtr handle = GetConsoleWindow();

            // 3. Esconde o console (ele continua rodando, só não aparece)
            ShowWindow(handle, SW_HIDE);

            Console.WriteLine("[SYSTEM] Terminal de Depuração AetherRuntimes Ativado.");
        }

        // Método para você chamar no código todo vez que algo acontecer
        private void LogDebug(string mensagem, string tipo = "INFO")
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{time}] [{tipo}] {mensagem}");
        }
        private async System.Threading.Tasks.Task ValidarStatusSistema()
        {
            if (string.IsNullOrEmpty(UrlBlockedSystem)) return;

            try
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {

                    string jsonString = await client.DownloadStringTaskAsync(UrlBlockedSystem);
                    var infoStatus = JsonSerializer.Deserialize<StatusModel>(jsonString);

                    if (infoStatus != null && infoStatus.status.ToLower() == "blocked")
                    {

                        if (timerPing != null) timerPing.Stop();

                        OffSystem offForm = new OffSystem();
                        offForm.Show();
                        this.Hide();

                    }
                }
            }
            catch
            {
                PopupForm.Show(this, "Erro ao se concetar aos servidores. Verifique sua conexão com a internet.\nCode:11");
            }


        }

        private void TxtPesquisa_TextChanged(object sender, EventArgs e)
        {
            string termoBusca = txtPesquisa.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(termoBusca))
            {
                // Se a pesquisa estiver vazia, mostra a lista completa
                AtualizarInterface();
            }
            else
            {
                // Filtra os logs onde o Identificador contém o texto digitado
                var filtrados = logsGerais.Where(l => l.Identificador.ToUpper().Contains(termoBusca)).Select(l => new
                {
                    DATA = l.DataHora,
                    SERIAL = l.Identificador,
                    CATEGORIA = l.Categoria,
                    DIAGNOSTICO = string.Join(", ", l.DefeitosBrutos.Select(d => d.Split('|')[1]))
                }).ToList();

                // Atualiza o DataGridView apenas com os resultados filtrados
                dgvLogs.DataSource = null;
                dgvLogs.DataSource = filtrados;

                // Reaplica a formatação de colunas
                if (dgvLogs.Columns.Count > 0)
                {
                    dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvLogs.Columns["DIAGNOSTICO"].FillWeight = 200;
                }
                AetherLogger.UI("txtPesquisa", $"FILTRO APLICADO: {txtPesquisa.Text}");
                // Atualiza o contador para mostrar quantos foram encontrados no filtro
                lblTotal.Text = $"ENCONTRADOS: {filtrados.Count}";
            }
        }
        private void txtIdentificador_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!checkBoxMultiline.Checked)
                {
                    // MODO INDIVIDUAL: Bipou, já dispara o salvar
                    btnSalvar_Click(this, new EventArgs());
                    e.SuppressKeyPress = true; // Impede o "beep" e a quebra de linha no modo simples
                }
                // Se estiver em Multiline, não fazemos nada, o AcceptsReturn cuida de pular a linha
            }
        }

        private void AtualizarVelocidadeDoLink()
        {
            try
            {
                // 1. Localiza a placa de rede física que está com o cabo conectado (Up)
                var interfaceAtiva = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(n => n.OperationalStatus == OperationalStatus.Up &&
                                         (n.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                                          n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) &&
                                         !n.Description.ToLower().Contains("virtual") &&
                                         !n.Description.ToLower().Contains("pseudo"));

                if (interfaceAtiva != null)
                {
                    // 2. n.Speed retorna o valor em bits por segundo (bps). 
                    // Dividimos por 1.000.000 para obter o valor em Mbps.
                    long velocidadeMbps = interfaceAtiva.Speed / 1000000;

                    // 3. Exibe o valor no seu label
                    lblRede.Text = $"{velocidadeMbps} Mbps";

                    // 4. Lógica visual da barra (ex: se for 1000 Mbps fica cheia, se for 100 fica 10%)
                    // Isso ajuda a identificar visualmente se o cabo é ruim ou porta é 10/100
                    if (velocidadeMbps >= 1000)
                    {
                        pbRede.Value = 100;
                        pbRede.ProgressColor = Color.FromArgb(94, 148, 255); // Azul (Giga)
                    }
                    else if (velocidadeMbps >= 300)
                    {
                        pbRede.Value = 50;
                        pbRede.ProgressColor = Color.Orange; // Laranja (Fast Ethernet)
                    }
                    else
                    {
                        pbRede.Value = 10;
                        pbRede.ProgressColor = Color.Red; // Vermelho (10 Mbps ou cabo ruim)
                    }
                }
                else
                {
                    lblRede.Text = "Erro";
                    pbRede.Value = 0;
                }
            }
            catch
            {
                lblRede.Text = "Erro ao ler porta LAN";
            }
        }
        private void GerarLogsInicializacao()
        {
            // Limpa se houver algo (opcional)
            lstPromptPing.Items.Clear();
            string nomeOperador = ObterNomeExibicao().ToUpper();
            // Cabeçalho de Inicialização
            lstPromptPing.Items.Add($"[{DateTime.Now:HH:mm:ss}]========= AETHER ENGINE =========");
            lstPromptPing.Items.Add($"[{DateTime.Now:HH:mm:ss}] Versão {CompilatedVersion} - Engine WD09");
            lstPromptPing.Items.Add($"[{DateTime.Now:HH:mm:ss}] Operador: {nomeOperador}");
            lstPromptPing.Items.Add($"[{DateTime.Now:HH:mm:ss}] Verificando integridade dos módulos...");
            // Verificação de arquivos locais
            string statusIps = File.Exists(ArquivoIPs) ? "Carregado" : "Não encontrado (Novo)";
            lstPromptPing.Items.Add($"[{DateTime.Now:HH:mm:ss}]Banco de IPs {statusIps}.");
            // Informação de Rede
            lstPromptPing.Items.Add($"[{DateTime.Now:HH:mm:ss}]Monitor de interface ativado.");
            lstPromptPing.Items.Add($"[{DateTime.Now:HH:mm:ss}]Lista de triagem carregada.");
            lstPromptPing.Items.Add($"[{DateTime.Now:HH:mm:ss}]Conectado a rede interna da Aether Services.");

            lstPromptPing.Items.Add("------------------------------------------------------------");
            lstPromptPing.Items.Add("Pronto para operação. Selecione um IP para iniciar o monitoramento.");
            lstPromptPing.Items.Add("------------------------------------------------------------");
        }
        private string ObterNomeExibicao()
        {
            try
            {
                // Tenta buscar o nome completo via WMI (mais leve e sem referências extras)
                string query = $"SELECT RealName FROM Win32_UserAccount WHERE Name = '{Environment.UserName}' AND Domain = '{Environment.UserDomainName}'";
                using (var searcher = new System.Management.ManagementObjectSearcher(query))
                {
                    foreach (System.Management.ManagementObject item in searcher.Get())
                    {
                        string nomeReal = item["Fullname"]?.ToString() ?? item["Name"]?.ToString();
                        if (!string.IsNullOrEmpty(nomeReal)) return nomeReal;
                    }
                }
            }
            catch
            {
                // Se falhar, tenta o nome registrado no registro do Windows
                try
                {
                    return Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AccountPicture", "DisplayName", null)?.ToString()
                           ?? Environment.UserName;
                }
                catch { }
            }

            return Environment.UserName; // Último recurso: nome de login comum
        }


        private void ConfigurarLayoutPrompt()
        {
            lstPromptPing.Font = new Font("Consolas", 9);
        }

        private void ConfigurarTimerPing()
        {
            timerPing = new System.Windows.Forms.Timer { Interval = 1000 };
            timerPing.Tick += async (s, e) =>

            {
                string ip = cbIPs.Text.Trim();
                if (string.IsNullOrEmpty(ip)) return;
                using (Ping p = new Ping())
                {
                    try
                    {
                        byte[] buffer = new byte[32];
                        var rep = await p.SendPingAsync(ip, 1000, buffer);
                        string log;
                        if (rep.Status == IPStatus.Success)
                        {
                            log = $"[{DateTime.Now:HH:mm:ss}]> {ip}: bytes=32 tempo={rep.RoundtripTime}ms TTL={rep.Options?.Ttl}";
                            lblStatusLan.Text = "Equipamento Conectado";
                            lblStatusLan.ForeColor = Color.Green;

                            AtualizarVelocidadeDoLink();
                        }
                        else
                        {
                            log = $"[{DateTime.Now:HH:mm:ss}]> PING {ip} -> FALHA (Tempo Esgotado)";
                            lblStatusLan.Text = "Nenhum equipamento conectado";
                            lblStatusLan.ForeColor = Color.Red;

                            AtualizarVelocidadeDoLink();
                        }
                        lstPromptPing.Items.Insert(0, log);
                        if (lstPromptPing.Items.Count > 50) lstPromptPing.Items.RemoveAt(50);
                    }
                    catch { }
                }
            };
            timerPing.Start();
        }
        private void ConfigurarEventosInterfaceIP()
        {
            // Associa o evento de mudança de seleção do ComboBox
            cbIPs.SelectedIndexChanged += (s, e) =>
            {
                string ipSelecionado = cbIPs.Text.Trim();

                // Evita disparar se estiver vazio ou nulo
                if (string.IsNullOrEmpty(ipSelecionado)) return;

                // Utiliza a sua caixa de mensagem customizada (Aether UI)
                bool desejaAcessar = Msg.Question($"Deseja acessar a interface Web do equipamento ({ipSelecionado})?");

                if (desejaAcessar)
                {
                    try
                    {
                        // Garante o prefixo http:// para o navegador entender como URL
                        string url = ipSelecionado.StartsWith("http://") || ipSelecionado.StartsWith("https://")
                            ? ipSelecionado
                            : $"http://{ipSelecionado}";

                        // Abre o navegador padrão do sistema
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true // Necessário no .NET Core / .NET 5+ para abrir URLs
                        });
                    }
                    catch (Exception ex)
                    {
                        // Caso falhe, usa o seu método padrão de aviso (sem botões de interrogação)
                        PopupForm.Show(this, $"Erro ao abrir o navegador:\n{ex.Message}");
                    }
                }
            };
        }
        // --- GESTÃO DE IPs ---
        private void btnAdicionarIP_Click(object sender, EventArgs e)
        {
            string novoIp = Interaction.InputBox("Digite o IP do equipamento:", "Cadastrar IP", "192.168.1.1");

            if (!string.IsNullOrWhiteSpace(novoIp) && !cbIPs.Items.Contains(novoIp))
            {
                cbIPs.Items.Add(novoIp);
                cbIPs.SelectedItem = novoIp;
                SalvarListaIPs();
            }
        }
        private void SalvarListaIPs()
        {
            try
            {
                // 1. Define o caminho para a pasta "metadados" (no diretório onde o .exe está rodando)
                string pastaMetadados = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metadados");

                // 2. Garante que a pasta exista antes de tentar salvar
                if (!Directory.Exists(pastaMetadados))
                {
                    Directory.CreateDirectory(pastaMetadados);
                }

                // 3. Define o caminho final do arquivo dentro da pasta metadados
                // Se 'ArquivoIPs' já contiver apenas o nome do arquivo (ex: "ips.json"), usamos Path.GetFileName
                string caminhoCompleto = Path.Combine(pastaMetadados, Path.GetFileName(ArquivoIPs));

                // 4. Serializa e salva os dados de forma assíncrona ou direta
                string json = JsonSerializer.Serialize(cbIPs.Items.Cast<string>().ToList());
                File.WriteAllText(caminhoCompleto, json);
            }
            catch (Exception ex)
            {


            }
        }
        private void CarregarIPsSalvos()
        {
            if (File.Exists(ArquivoIPs))
            {

                try
                {

                    var ips = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(ArquivoIPs));

                    if (ips != null) cbIPs.Items.AddRange(ips.ToArray());
                }
                catch { }
            }
        }
        // --- BOTÕES E COMPORTAMENTOS ---
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Captura todos os IDs, removendo espaços e linhas vazias
            var seriais = txtIdentificador.Lines
                .Select(s => s.Trim().ToUpper())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .ToList();

            if (seriais.Count == 0)
            {
                PopupForm.Show(this, "Nenhum identificador encontrado!");
                return;
            }

            // Validação de Duplicidade
            List<string> seriaisValidos = new List<string>();
            foreach (var id in seriais)
            {
                if (SerialJaExiste(id))
                {
                    // Se for apenas um, mostra o erro e para. Se for lote, avisa e ignora o duplicado.
                    if (seriais.Count == 1)
                    {
                        PopupForm.Show(this, $"O equipamento {id} já foi triado.");
                        txtIdentificador.Clear();
                        return;
                    }
                    LogDebug($"ID {id} ignorado por duplicidade.");
                }
                else
                {
                    seriaisValidos.Add(id);
                }
            }

            if (seriaisValidos.Count == 0) return;

            using (var telaSelecao = new Selecao())
            {
                if (telaSelecao.ShowDialog() == DialogResult.OK && telaSelecao.Selecionados.Count > 0)
                {
                    string categoria = telaSelecao.Selecionados.Any(d => d.Codigo.StartsWith("N")) ? "NORMAL" :
                                       (telaSelecao.Selecionados.Any(d => d.Codigo.StartsWith("C")) ? "CARCAÇA" : "DEFEITO");

                    foreach (string id in seriaisValidos)
                    {
                        logsGerais.Add(new RegistroEquipamento
                        {
                            Identificador = id,
                            DataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                            Categoria = categoria,
                            DefeitosBrutos = telaSelecao.Selecionados.Select(x => $"{x.Codigo}|{x.Descricao}").ToList()
                        });
                    }

                    File.WriteAllText(NomeArquivoLogs, JsonSerializer.Serialize(logsGerais, new JsonSerializerOptions { WriteIndented = true }));
                    AetherLogger.Disk(NomeArquivoLogs, "SYNC_SAVE", new FileInfo(NomeArquivoLogs).Length);
                    AtualizarInterface();

                    txtIdentificador.Clear();
                    txtIdentificador.Focus();


                    if (seriaisValidos.Count > 1)
                        PopupForm.Show(this, $"Sucesso! {seriaisValidos.Count} itens processados em lote.");
                }
            }
        }
        private void btnGerar_Click_1(object sender, EventArgs e)

        {
            if (logsGerais.Count == 0)
            {
                PopupForm.Show(this, "Não existe itens na lista para gerar um relatorio.");
            }
            else
            {


                string caminhoTxt = Path.Combine(Application.StartupPath, "Relatorio_Triagem.txt");
                try
                {
                    PopupForm.Show(this, "Seu relatorio de triagem foi gerado!");
                    using (StreamWriter sw = new StreamWriter(caminhoTxt))
                    {
                        sw.WriteLine($"RELATÓRIO DE TESTE DE EQUIPAMENTOS AGRUPADO - {DateTime.Now:dd/MM/yyyy HH:mm}");
                        sw.WriteLine("==================================================");
                        // Agrupamos os registros pela COMBINAÇÃO de defeitos que eles possuem

                        var gruposPorDefeitos = logsGerais

                            .GroupBy(l => string.Join(" ", l.DefeitosBrutos
                                .OrderBy(d => d.Split('|')[0]) // Ordena os códigos para que C1 D6 seja igual a D6 C1
                                .Select(d =>
                                {
                                    var p = d.Split('|');

                                    return $"{p[0]} \"{p[1]}\""; // Formato: C1 "DEFEITO"

                                })))

                            .OrderBy(g => g.Key);
                        foreach (var grupo in gruposPorDefeitos)
                        {
                            // O grupo.Key é a string com todos os defeitos juntos
                            sw.WriteLine($"\n{grupo.Key}");
                            sw.WriteLine(new string('-', 40));
                            // Lista todos os seriais que possuem EXATAMENTE essa combinação
                            foreach (var item in grupo)
                            {
                                sw.WriteLine(item.Identificador);
                            }
                        }
                        sw.WriteLine("\n==================================================");
                        sw.WriteLine($"Total de Equipamentos: {logsGerais.Count}");
                    }
                    Process.Start(new ProcessStartInfo(caminhoTxt) { UseShellExecute = true });
                }
                catch (Exception ex)
                {

                    Msg.Show($"ERRO: {ex.Message}");
                }
            }
        }
        private void DgvLogs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                string serialSelecionado = dgvLogs.Rows[e.RowIndex].Cells["SERIAL"].Value.ToString();
                var registro = logsGerais.FirstOrDefault(l => l.Identificador == serialSelecionado);

                // Copia o identificador para a área de transferência automaticamente
                Clipboard.SetText(serialSelecionado);

                if (registro != null)
                {

                    string detalhesDefeitos = string.Join(Environment.NewLine, registro.DefeitosBrutos.Select(d =>
                    {
                        var partes = d.Split('|');
                        return $"  > [{partes[0]}] {partes[1]}";
                    }));

                    string mensagem = $"FICHA TÉCNICA DO EQUIPAMENTO\n" +
                                      $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                      $"ID: {registro.Identificador} [COPIADO]\n" +
                                      $"HORÁRIO: {registro.DataHora}\n" +
                                      $"CATEGORIA: {registro.Categoria}\n" +
                                      $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                      $"DIAGNÓSTICO:\n{detalhesDefeitos}\n\n" +
                                      $"DESEJA ELIMINAR ESTE REGISTRO?";

                    if (Msg.Question(mensagem))
                    {
                        RemoverRegistro(registro);

                    }
                    PopupForm.Show(this, $"Identificador'{registro.Identificador}' copiado para aréa de transferencia.");

                }

            }
        }
        private void RemoverRegistro(RegistroEquipamento registro)
        {
            try
            {
                // Remove da lista na memória
                logsGerais.Remove(registro);
                // Atualiza o arquivo JSON local imediatamente
                string json = JsonSerializer.Serialize(logsGerais, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(NomeArquivoLogs, json);
                // Atualiza a grade (DataGridView) e o contador
                AtualizarInterface();

                PopupForm.Show(this, "Equipamento removido com sucesso!");
            }
            catch (Exception ex)
            {
                PopupForm.Show(this, $"Erro ao remover: {ex.Message}");
            }
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            {
                if (File.Exists(NomeArquivoLogs))
                {
                    if (Msg.Question("Tem certeza que deseja limpar a lista?\nNão será possível fazer o relatório após a confirmação."))
                    {
                        logsGerais.Clear();

                        if (File.Exists(NomeArquivoLogs))
                            File.Delete(NomeArquivoLogs);

                        AtualizarInterface();
                        PopupForm.Show(this, "Lista limpa com sucesso!");
                    }
                }
                else
                {
                    PopupForm.Show(this, "Não existem registros na lista.");
                }
            }
        }

        private void ConfigurarRelogio()
        {
            timerRelogio = new System.Windows.Forms.Timer { Interval = 1000 };
            timerRelogio.Tick += (s, e) => { lblHorario.Text = DateTime.Now.ToString("HH:mm:ss"); };
            timerRelogio.Start();
        }
        public void AtualizarInterface()
        {
            lblTotal.Text = $"TOTAL DE EQUIPAMENTOS TRIADOS: {logsGerais.Count}";
            if (logsGerais.Count <= 0)
            {
                lblAl1.Text = "SEM EQUIPAMENTOS TRIADOS";
            }
            else
            {
                lblAl1.Text = "";
            }
            dgvLogs.DataSource = null;

            // Adicionamos o .Reverse() aqui para inverter a exibição
            dgvLogs.DataSource = logsGerais.Select(l => new
            {
                DATA = l.DataHora,
                SERIAL = l.Identificador,
                CATEGORIA = l.Categoria,
                DIAGNOSTICO = string.Join(", ", l.DefeitosBrutos.Select(d => d.Split('|')[1]))
            }).Reverse().ToList(); // <--- Inverte a ordem visual

            if (dgvLogs.Columns.Count > 0)
            {
                dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvLogs.Columns["DIAGNOSTICO"].FillWeight = 200;
            }
            if (dgvLogs.Columns.Count > 0) { dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; dgvLogs.Columns["DIAGNOSTICO"].FillWeight = 200; }
        }
        public void CarregarLogsIniciais() { if (File.Exists(NomeArquivoLogs)) try { logsGerais = JsonSerializer.Deserialize<List<RegistroEquipamento>>(File.ReadAllText(NomeArquivoLogs)) ?? new List<RegistroEquipamento>(); } catch { } }
        private void cbIPs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private bool SerialJaExiste(string serial)
        {
            // Verifica se existe algum registro na lista com o mesmo Identificador (Serial)
            // Usamos StringComparison.OrdinalIgnoreCase para ignorar diferenças entre maiúsculas e minúsculas
            return logsGerais.Any(l => l.Identificador.Equals(serial, StringComparison.OrdinalIgnoreCase));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string pastaBackup = Path.Combine(Application.StartupPath, "Backup_Seguranca");
                if (!Directory.Exists(pastaBackup)) Directory.CreateDirectory(pastaBackup);

                if (File.Exists(NomeArquivoLogs))
                {
                    // Gera um nome com data e hora para não sobrescrever backups manuais anteriores
                    string nomeArquivo = $"manual_backup_{DateTime.Now:dd-MM-yyyy_HH-mm}.json";
                    string destino = Path.Combine(pastaBackup, nomeArquivo);

                    File.Copy(NomeArquivoLogs, destino, true);

                    PopupForm.Show(this, $"Backup Manual\nBackup realizado com sucesso!\nSalvo como: {nomeArquivo}");
                    LogDebug($"Backup manual criado: {nomeArquivo}");
                }
                else
                {
                    PopupForm.Show(this, "Não há logs para fazer backup no momento.");
                }
            }
            catch (Exception ex)
            {
                PopupForm.Show(this, $"Aviso\nErro ao criar backup: {ex.Message}");
            }
        }
        private void dgvLogs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (ModifierKeys & Keys.Control) == Keys.Control)
            {
                // O SEGREDO: Força a tabela a focar para aceitar comandos de teclado (Backspace)
                dgvLogs.Focus();

                var hit = dgvLogs.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0)
                {
                    dgvLogs.Rows[hit.RowIndex].Selected = !dgvLogs.Rows[hit.RowIndex].Selected;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 1. Captura o Backspace
            if (keyData == Keys.Back)
            {
                // 2. Garante que a tabela dgvLogs está focada e possui itens selecionados
                if ((dgvLogs.ContainsFocus || dgvLogs.Focused) && dgvLogs.SelectedRows.Count > 0)
                {
                    string mensagemConfirmacao = $"Deseja eliminar os {dgvLogs.SelectedRows.Count} registros selecionados?";

                    // Pergunta ao usuário se ele realmente deseja apagar o lote (baseado no seu Msg.Question)
                    if (Msg.Question(mensagemConfirmacao))
                    {
                        try
                        {
                            // Lista temporária para guardar os registros que serão assassinados
                            List<RegistroEquipamento> itensParaRemover = new List<RegistroEquipamento>();

                            // Varte as linhas selecionadas na tabela
                            foreach (DataGridViewRow linha in dgvLogs.SelectedRows)
                            {
                                if (!linha.IsNewRow && linha.Cells["SERIAL"].Value != null)
                                {
                                    string serialSelecionado = linha.Cells["SERIAL"].Value.ToString();

                                    // Procura o objeto correspondente dentro da sua lista de memória logsGerais
                                    var registro = logsGerais.FirstOrDefault(l => l.Identificador == serialSelecionado);

                                    if (registro != null)
                                    {
                                        itensParaRemover.Add(registro);
                                    }
                                }
                            }

                            // Remove todos os registros encontrados da lista principal na memória
                            foreach (var registro in itensParaRemover)
                            {
                                logsGerais.Remove(registro);
                            }

                            // Sincroniza com o arquivo JSON local imediatamente (idêntico ao seu RemoverRegistro)
                            string json = JsonSerializer.Serialize(logsGerais, new JsonSerializerOptions { WriteIndented = true });
                            File.WriteAllText(NomeArquivoLogs, json);

                            // Atualiza a grade (DataGridView) e os contadores da tela
                            AtualizarInterface();

                            PopupForm.Show(this, $"{itensParaRemover.Count} equipamentos removidos com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            PopupForm.Show(this, $"Erro ao remover registros em lote: {ex.Message}");
                        }
                    }

                    // Avisa o ecossistema do Windows que a tecla Backspace foi processada com sucesso
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void btnImportBackup_Click(object sender, EventArgs e)
        {
            string pastaBackup = Path.Combine(Application.StartupPath, "Backup_Seguranca");
            if (!Directory.Exists(pastaBackup)) Directory.CreateDirectory(pastaBackup);

            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                openFile.InitialDirectory = pastaBackup;
                openFile.Filter = "Arquivos JSON (*.json)|*.json|Todos os arquivos (*.*)|*.*";
                openFile.Title = "Selecionar Backup para Importação";

                // Abre a janela de seleção de arquivo
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    // Pergunta personalizada usando o sistema Dark da Aether
                    if (Msg.Question("Isso irá substituir todos os registros atuais pelos dados do backup selecionado.\nDeseja continuar?"))
                    {
                        try
                        {
                            // 1. Backup de segurança do estado atual (preventivo) antes de sobrescrever
                            if (File.Exists(NomeArquivoLogs))
                            {
                                string nomeAutoSave = $"pre_import_autosave_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                                File.Copy(NomeArquivoLogs, Path.Combine(pastaBackup, nomeAutoSave), true);
                            }

                            // 2. Copia o arquivo selecionado para o local do log oficial (sobrescrevendo)
                            File.Copy(openFile.FileName, NomeArquivoLogs, true);

                            // 3. Recarrega a lista na memória e atualiza a interface
                            CarregarLogsIniciais();
                            AtualizarInterface();

                            // Mensagem de sucesso personalizada
                            PopupForm.Show(this, "Os dados foram importados corretamente.");
                        }
                        catch (Exception ex)
                        {
                            // Mensagem de erro personalizada
                            PopupForm.Show(this, $"ERRO AO IMPORTAR:\n{ex.Message}");
                        }
                    }
                }
            }
        }

        // No evento CheckedChanged do seu CheckBox
        private void checkBoxMultiline_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMultiline.Checked)
            {
                // Ativa o modo Multiline
                txtIdentificador.Multiline = true;
                txtIdentificador.ScrollBars = ScrollBars.Vertical;
                txtIdentificador.AcceptsReturn = true; // Permite que o Enter pule linha
                txtIdentificador.Height = 80;
            }
            else
            {
                // Volta para o modo linha única
                txtIdentificador.Multiline = false;
                txtIdentificador.AcceptsReturn = false; // Enter volta a ser usado para disparar o botão padrão
                txtIdentificador.Height = 30; // Ajuste para a altura padrão de uma linha
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // SÓ executa a saída limpa se o chat chegou a ser criado em algum momento
            if (_athChat != null)
            {
                _athChat.Desconectar();
            }

            base.OnFormClosing(e);
        }
        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Substitua 'meuChat' pelo nome da variável que você usou para instanciar o formulário Chat
            if (_athChat != null && !_athChat.IsDisposed)
            {
                _athChat.Desconectar();
            }
        }
        private void btnAetherAI_Click(object sender, EventArgs e)
        {

            // Agora o construtor não pede nada (), tornando-o independente
            Sora frmIA = new Sora();

            // Abre como uma janela flutuante independente
            frmIA.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Ajustes ajustes = new Ajustes(this);
            ajustes.Show();
        }

        public void AlternarTemaGlobal()
        {
            var novo = (ThemeManager.CurrentTheme == ThemeManager.ThemeMode.Dark)
                        ? ThemeManager.ThemeMode.Light
                        : ThemeManager.ThemeMode.Dark;

            // Aplica no próprio Home
            ThemeManager.ApplyAndSaveThemeToAllForms(novo); // Ou Light

            // Se o Ajustes estiver aberto, ele já está com o tema atualizado 
            // ou você pode forçar a atualização se ele estiver referenciado
        }
        // --- CLASSES DE MODELO (MODELS) ---
        public class StatusModel { public string status { get; set; } = "free"; public string mensagem { get; set; } = ""; }
        public class RegistroEquipamento { public string Identificador { get; set; } = ""; public string DataHora { get; set; } = ""; public string Categoria { get; set; } = ""; public List<string> DefeitosBrutos { get; set; } = new List<string>(); }
        public class KeySystem { public List<KeyConfig> keys { get; set; } }
        public class VersaoModel { public string versao { get; set; } = ""; }
        public class InfoUpdate { public string versao { get; set; } = ""; public string url_zip { get; set; } = ""; public string notas { get; set; } = ""; }

        public class KeyConfig { public string codigo { get; set; } public string dono { get; set; } public string validade { get; set; } }

        private void lblwelabs_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/Siliconarch/";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            if (_athChat == null || _athChat.IsDisposed)
            {
                _athChat = new Chat();
            }

            _athChat.ExibirChat();
        }
        private void btnAtribuir_Click(object sender, EventArgs e)
        {
            // Solicita os técnicos online (não precisamos mais checar se há algo selecionado no dgvLogs)
            _athNet.SolicitarListaTecnicos();

            // Passamos apenas a lista de técnicos para o formulário limpo
            using (var form = new FormAtribuicao(_tecnicosOnline))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    ExecutarAtribuicaoEmLote(form);
                }
            }
        }

        private void ExecutarAtribuicaoEmLote(FormAtribuicao form)
        {
            if (form.Seriais == null || form.Seriais.Count == 0) return;

            // Junta todos os seriais digitados em uma única linha separada por vírgulas
            string seriaisAgrupados = string.Join(",", form.Seriais);

            // Envia o lote inteiro em um único pacote de rede
            _athNet.EnviarMensagem(
                form.TecnicoSelecionado,
                seriaisAgrupados,
                form.Categoria,
                form.Diagnostico,
                form.Nota
            );

            PopupForm.Show(this, $"{form.Seriais.Count} tarefa(s) enviada(s) em lote com sucesso!");
        }

        private void ProcessarTarefaRecebida(string mensagem)
        {
            this.Invoke(new Action(() =>
            {
                // O servidor manda: TAREFA|SeriaisSeparadosPorVirgula|Categoria|Diagnostico|Nota
                string[] partes = mensagem.Split('|');

                if (partes.Length >= 5)
                {
                    // Separa de volta a string em uma lista de seriais reais
                    List<string> seriaisRecebidos = partes[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                             .Select(s => s.Trim())
                                                             .ToList();

                    string categoria = partes[2].Trim();
                    string diagnostico = partes[3].Trim();
                    string nota = partes[4].Trim();

                    DialogResult respostaDialogo = DialogResult.No;

                    // Monta o texto em formato de lista estruturada para colocar na caixa Guna
                    System.Text.StringBuilder sbItens = new System.Text.StringBuilder();
                    sbItens.AppendLine("┌──────────────────────────────────────────┐");
                    sbItens.AppendLine($"  CATEGORIA   : {categoria}");
                    sbItens.AppendLine($"  DIAGNÓSTICO : {diagnostico}");
                    sbItens.AppendLine($"  OBSERVAÇÃO  : {nota}");
                    sbItens.AppendLine("├──────────────────────────────────────────┤");
                    sbItens.AppendLine("  EQUIPAMENTOS NO LOTE:");

                    foreach (var s in seriaisRecebidos)
                    {
                        sbItens.AppendLine($"  • {s}");
                    }
                    sbItens.AppendLine("└──────────────────────────────────────────┘");
                    sbItens.AppendLine("\r\n Deseja assumir TODOS os equipamentos acima juntos?");

                    // Caixa de Mensagem Customizada Única usando Guna
                    using (Form formNotificacao = new Form())
                    {
                        formNotificacao.Text = "Aether Network - Envio de Equipamentos";
                        formNotificacao.Size = new Size(460, 420); // Aumentado um pouco para comportar a lista
                        formNotificacao.StartPosition = FormStartPosition.CenterParent;
                        formNotificacao.FormBorderStyle = FormBorderStyle.FixedDialog;
                        formNotificacao.MaximizeBox = false;
                        formNotificacao.MinimizeBox = false;
                        formNotificacao.BackColor = Color.FromArgb(245, 245, 245);
                        formNotificacao.Font = new Font("Arial", 10F);

                        Guna.UI2.WinForms.Guna2Panel pnlHeader = new Guna.UI2.WinForms.Guna2Panel
                        {
                            Size = new Size(460, 50),
                            Location = new Point(0, 0),
                            FillColor = Color.FromArgb(0, 102, 204),
                        };

                        Label lblTitulo = new Label
                        {
                            Text = $"RECEBIMENTO DE EQUIPAMENTO: {seriaisRecebidos.Count} ITENS)",
                            Font = new Font("Arial", 11F, FontStyle.Bold),
                            ForeColor = Color.White,
                            Location = new Point(15, 16),
                            AutoSize = true,
                            BackColor = Color.Transparent
                        };
                        pnlHeader.Controls.Add(lblTitulo);

                        Guna.UI2.WinForms.Guna2TextBox txtFicha = new Guna.UI2.WinForms.Guna2TextBox
                        {
                            Location = new Point(20, 70),
                            Size = new Size(405, 220),
                            Multiline = true,
                            ReadOnly = true,
                            ScrollBars = ScrollBars.Vertical,
                            Font = new Font("Courier New", 9.5F, FontStyle.Bold),
                            ForeColor = Color.Black,
                            FillColor = Color.White,
                            BorderColor = Color.FromArgb(180, 180, 180),
                            BorderRadius = 0,
                            Text = sbItens.ToString()
                        };

                        Guna.UI2.WinForms.Guna2Button btnAceitar = new Guna.UI2.WinForms.Guna2Button
                        {
                            Text = "ACEITAR EQUIPAMENTO(OS)",
                            Size = new Size(190, 42),
                            Location = new Point(20, 310),
                            FillColor = Color.FromArgb(0, 128, 64),
                            ForeColor = Color.White,
                            Font = new Font("Arial", 9.5F, FontStyle.Bold),
                            BorderRadius = 0,
                            Animated = true
                        };
                        btnAceitar.Click += (s, e) => { respostaDialogo = DialogResult.Yes; formNotificacao.Close(); };

                        Guna.UI2.WinForms.Guna2Button btnRecusar = new Guna.UI2.WinForms.Guna2Button
                        {
                            Text = "RECUSAR EQUIPAMENTO(0S)",
                            Size = new Size(190, 42),
                            Location = new Point(235, 310),
                            FillColor = Color.FromArgb(166, 0, 0),
                            ForeColor = Color.White,
                            Font = new Font("Arial", 9.5F, FontStyle.Bold),
                            BorderRadius = 0,
                            Animated = true
                        };
                        btnRecusar.Click += (s, e) => { respostaDialogo = DialogResult.No; formNotificacao.Close(); };

                        formNotificacao.Controls.AddRange(new Control[] { pnlHeader, txtFicha, btnAceitar, btnRecusar });
                        formNotificacao.ShowDialog();
                    }

                    // Se aceitou, adiciona e salva todos de uma vez só
                    if (respostaDialogo == DialogResult.Yes)
                    {
                        int adicionados = 0;
                        List<string> duplicados = new List<string>();

                        foreach (string serial in seriaisRecebidos)
                        {
                            if (SerialJaExiste(serial))
                            {
                                duplicados.Add(serial);
                                continue;
                            }

                            // Adiciona na lista principal de memória
                            logsGerais.Add(new RegistroEquipamento
                            {
                                Identificador = serial,
                                DataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                                Categoria = categoria,
                                DefeitosBrutos = new List<string> { $"ATRIB|{diagnostico}" }
                            });
                            adicionados++;
                        }

                        // Se pelo menos um item foi adicionado, salva o JSON físico de uma vez só
                        if (adicionados > 0)
                        {
                            File.WriteAllText(NomeArquivoLogs, System.Text.Json.JsonSerializer.Serialize(logsGerais, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

                            // Atualiza a tabela na tela do técnico
                            AtualizarInterface();
                        }

                        // Feedback final resumido
                        if (duplicados.Count > 0)
                        {
                            MessageBox.Show($"{adicionados} itens adicionados. {duplicados.Count} seriais foram ignorados pois já existiam na sua lista local.", "Resultado da Atribuição");
                        }
                        else
                        {
                            PopupForm.Show(null, $"{adicionados} novos equipamentos foram integrados à sua lista!");
                        }
                    }
                }
            }));
        }
            private void AetherClose_FormClosing(object sender, FormClosingEventArgs e)
        {
            _athNet.Desconectar();
            _athChat?.Desconectar();
            Application.Exit();
        }
    }
    }
  


