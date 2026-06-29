using aether.Controle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json; // Adicionado para carregar as preferências dinâmicas
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using static Guna.UI2.Native.WinApi;

namespace aether.Interface
{
    public partial class Chat : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")] private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")] private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private Guna2Elipse gunaElipseForm;

        private Panel pnlTopBar;
        private Label lblBarraTitulo;
        private Label lblStatusRede;
        private Button btnFechar;
        private Button btnMinimizar;

        private FlowLayoutPanel pnlMensagens;
        private Panel pnlInput;
        private Panel pnlInputWrapper;
        private Guna2TextBox txtInput;
        private Guna2Button btnEnviar;
        private Guna2Button btnAnexar;

        private ListBox lstUsuarios;
        private List<string> _usuariosConectados = new List<string>();

        private TcpClient _client;
        private NetworkStream _stream;
        private string _userName = "Usuário Aether";
        private string messageClone;
        private bool _ouvinteAtivo = false;
        private bool _historicoCarregado = false;

        private bool _deveTentarReconectar = true;
        private bool _tentandoConectar = false;

        // Cores padrão Apple Light Mode
        private Color appleBg = Color.WhiteSmoke;
        private Color appleSurface = Color.White;
        private Color appleTextPrimary = Color.Black;
        private Color appleTextSecondary = Color.FromArgb(142, 142, 147);
        private Color appleAccent = Color.FromArgb(10, 132, 255);

        public Chat()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = appleBg;
            this.Size = new Size(900, 680);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Arredondamento da janela
            gunaElipseForm = new Guna2Elipse { BorderRadius = 16, TargetControl = this };

            InitializeComponentManual();
            CarregarUsuario();
            ThemeManager.InitializeTheme(this);
        }

        public void ExibirChat()
        {
            this.Show();
            this.BringToFront();

            if (_client == null || !_client.Connected)
            {
                ConectarAoServidor();
            }
        }

        private void InitializeComponentManual()
        {
            // =================================================================
            // 1. BARRA SUPERIOR
            // =================================================================
            pnlTopBar = new Panel { Size = new Size(900, 45), Location = new Point(0, 0), BackColor = appleSurface };

            lblBarraTitulo = new Label { Text = "AETHER NETWORK", Font = new Font("SF Pro Text", 9F, FontStyle.Bold), ForeColor = appleTextPrimary, Location = new Point(18, 15), AutoSize = true };
            lblStatusRede = new Label { Text = "● Conectando ao ecossistema...", Font = new Font("SF Pro Text", 9F, FontStyle.Regular), ForeColor = Color.FromArgb(255, 149, 0), Location = new Point(155, 15), AutoSize = true };

            btnFechar = new Button { Text = "✕", Size = new Size(45, 45), Location = new Point(855, 0), FlatStyle = FlatStyle.Flat, ForeColor = appleTextSecondary, Font = new Font("SF Pro Text", 10F) };
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 59, 48); // Red Apple
            btnFechar.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 59, 48);
            btnFechar.Click += (s, e) => { this.Hide(); };

            btnMinimizar = new Button { Text = "—", Size = new Size(45, 45), Location = new Point(810, 0), FlatStyle = FlatStyle.Flat, ForeColor = appleTextSecondary, Font = new Font("SF Pro Text", 9F) };
            btnMinimizar.FlatAppearance.BorderSize = 0;
            btnMinimizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(229, 229, 234);
            btnMinimizar.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            pnlTopBar.Controls.AddRange(new Control[] { lblBarraTitulo, lblStatusRede, btnMinimizar, btnFechar });

            // =================================================================
            // 2. VIEWPORT DE MENSAGENS
            // =================================================================
            pnlMensagens = new FlowLayoutPanel
            {
                Location = new Point(0, 45),
                Size = new Size(900, 535),
                AutoScroll = true,
                BackColor = appleBg,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(20, 15, 20, 15)
            };

            // =================================================================
            // 3. DROPDOWN FLUTUANTE DE USUÁRIOS (SISTEMA @)
            // =================================================================
            lstUsuarios = new ListBox
            {
                BackColor = appleSurface,
                ForeColor = appleTextPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("SF Pro Text", 9.5F, FontStyle.Regular),
                Visible = false,
                Width = 220,
                Height = 110
            };
            lstUsuarios.Click += (s, e) => SelecionarUsuarioDropdown();
            lstUsuarios.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; SelecionarUsuarioDropdown(); } };

            // =================================================================
            // 4. CONTROL BAR INFERIOR (GUNA UI2)
            // =================================================================
            pnlInput = new Panel { Location = new Point(0, 580), Size = new Size(900, 100), BackColor = appleBg };

            pnlInputWrapper = new Panel { Location = new Point(20, 10), Size = new Size(860, 60), BackColor = appleBg };

            btnAnexar = new Guna2Button
            {
                Text = "➕",
                FillColor = Color.Transparent,
                ForeColor = appleTextSecondary,
                Location = new Point(0, 10),
                Size = new Size(40, 40),
                Font = new Font("SF Pro Text", 14F),
                BorderRadius = 20,
                Cursor = Cursors.Hand
            };
            btnAnexar.HoverState.FillColor = Color.FromArgb(229, 229, 234);
            btnAnexar.Click += BtnAnexar_Click;

            txtInput = new Guna2TextBox
            {
                Multiline = true,
                BackColor = Color.Transparent,
                FillColor = appleSurface,
                ForeColor = appleTextPrimary,
                BorderColor = Color.FromArgb(212, 212, 217),
                BorderRadius = 18,
                Font = new Font("SF Pro Text", 10.5F),
                Location = new Point(50, 10),
                Size = new Size(675, 40),
                PlaceholderText = "Escreva uma mensagem...",
                PlaceholderForeColor = appleTextSecondary,
                TextOffset = new Point(5, 5)
            };
            txtInput.TextChanged += TxtInput_TextChanged;
            txtInput.KeyDown += (s, e) =>
            {
                if (lstUsuarios.Visible)
                {
                    if (e.KeyCode == Keys.Down) { e.SuppressKeyPress = true; lstUsuarios.Focus(); if (lstUsuarios.Items.Count > 0) lstUsuarios.SelectedIndex = 0; return; }
                    if (e.KeyCode == Keys.Escape) { e.SuppressKeyPress = true; lstUsuarios.Visible = false; return; }
                }
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; EnviarMensagemTexto(); }
            };

            btnEnviar = new Guna2Button
            {
                Text = "Enviar",
                FillColor = appleAccent,
                ForeColor = Color.White,
                Location = new Point(735, 10),
                Size = new Size(115, 40),
                Font = new Font("SF Pro Text", 9.5F, FontStyle.Bold),
                BorderRadius = 18,
                Cursor = Cursors.Hand,
                Animated = true
            };
            btnEnviar.HoverState.FillColor = Color.FromArgb(0, 115, 230);
            btnEnviar.Click += (s, e) => EnviarMensagemTexto();

            pnlInputWrapper.Controls.AddRange(new Control[] { btnAnexar, txtInput, btnEnviar });
            pnlInput.Controls.Add(pnlInputWrapper);

            this.Controls.AddRange(new Control[] { pnlTopBar, pnlMensagens, pnlInput, lstUsuarios });
            pnlTopBar.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); };
        }

        private void TxtInput_TextChanged(object sender, EventArgs e)
        {
            string texto = txtInput.Text;
            int caretPos = txtInput.SelectionStart;

            if (string.IsNullOrEmpty(texto) || caretPos == 0)
            {
                lstUsuarios.Visible = false;
                return;
            }

            int indexAt = texto.LastIndexOf('@', caretPos - 1);

            if (indexAt >= 0 && (indexAt == 0 || texto[indexAt - 1] == ' '))
            {
                int comprimentoBusca = caretPos - (indexAt + 1);

                if (comprimentoBusca >= 0)
                {
                    string busca = texto.Substring(indexAt + 1, comprimentoBusca);
                    if (!busca.Contains(" "))
                    {
                        var filtrados = _usuariosConectados.Where(u => u.StartsWith(busca, StringComparison.OrdinalIgnoreCase)).ToList();
                        if (filtrados.Any())
                        {
                            lstUsuarios.Items.Clear();
                            foreach (var user in filtrados) lstUsuarios.Items.Add(user);

                            lstUsuarios.Location = new Point(pnlInputWrapper.Location.X + 55, pnlInput.Location.Y - lstUsuarios.Height - 5);
                            lstUsuarios.Visible = true;
                            lstUsuarios.BringToFront();
                            return;
                        }
                    }
                }
            }
            lstUsuarios.Visible = false;
        }

        private void SelecionarUsuarioDropdown()
        {
            if (lstUsuarios.SelectedItem == null) return;
            string nomeSelecionado = lstUsuarios.SelectedItem.ToString();
            string texto = txtInput.Text;
            int caretPos = txtInput.SelectionStart;
            int indexAt = texto.LastIndexOf('@', Math.Max(0, caretPos - 1));

            if (indexAt >= 0)
            {
                string antes = texto.Substring(0, indexAt);
                string depois = texto.Substring(caretPos);
                txtInput.Text = antes + "@" + nomeSelecionado + " " + depois;
                txtInput.SelectionStart = antes.Length + nomeSelecionado.Length + 2;
            }
            lstUsuarios.Visible = false;
            txtInput.Focus();
        }

        private void CarregarUsuario()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metadados", "ident.lic");
                if (File.Exists(path))
                {
                    string linha = File.ReadLines(path).FirstOrDefault(l => l.Contains("AUTHORIZED_USER"));
                    if (!string.IsNullOrEmpty(linha))
                    {
                        string emailCompleto = linha.Split(new char[] { ':' }, 2)[1].Trim();
                        string nomeSobrenome = emailCompleto.Split('@')[0];
                        string[] partes = nomeSobrenome.Split('.');
                        _userName = string.Join(" ", partes.Select(p => char.ToUpper(p[0]) + p.Substring(1).ToLower()));
                        return;
                    }
                }
            }
            catch { }
            _userName = "Usuário Aether";
        }

        private async Task ConectarAoServidor()
        {
            if (_tentandoConectar) return;
            _tentandoConectar = true;

            string prefsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metadados", "preferences.json");

            while (_deveTentarReconectar)
            {
                try
                {
                    if (_client != null && _client.Connected) break;

                    InvocarInterface(() => {
                        lblStatusRede.Text = "● Conectando ao ecossistema...";
                        lblStatusRede.ForeColor = Color.FromArgb(255, 149, 0); // Apple Orange
                    });

                    // Define fallbacks padrão caso o arquivo não exista ou esteja corrompido
                    string ipDestino = "127.0.0.1";
                    int portaDestino = 3356;

                    if (File.Exists(prefsPath))
                    {
                        try
                        {
                            string json = File.ReadAllText(prefsPath);
                            var prefs = JsonSerializer.Deserialize<AppPreferences>(json);
                            if (prefs?.NetworkConfig != null)
                            {
                                if (!string.IsNullOrEmpty(prefs.NetworkConfig.ServerIp))
                                    ipDestino = prefs.NetworkConfig.ServerIp;
                                if (prefs.NetworkConfig.ServerPort > 0)
                                    portaDestino = prefs.NetworkConfig.ServerPort;
                            }
                        }
                        catch { /* Ignora erros e mantém fallback */ }
                    }

                    _client = new TcpClient();
                    await _client.ConnectAsync(ipDestino, portaDestino);
                    _stream = _client.GetStream();

                    InvocarInterface(() => {
                        lblStatusRede.Text = $"● Conectado como {_userName}";
                        lblStatusRede.ForeColor = Color.FromArgb(52, 199, 89); // Apple Green
                    });

                    EnviarPacote(2, Encoding.UTF8.GetBytes(_userName));

                    if (!_ouvinteAtivo)
                    {
                        _ouvinteAtivo = true;
                        _ = Task.Run(() => OuvirServidorAsync());
                        _ = Task.Delay(1500).ContinueWith(_ => _historicoCarregado = true);
                    }
                    break;
                }
                catch
                {
                    InvocarInterface(() => {
                        lblStatusRede.Text = "✕ Servidor Offline. Tentando reconectar em 5s...";
                        lblStatusRede.ForeColor = Color.FromArgb(255, 59, 48); // Apple Red
                    });
                    await Task.Delay(5000);
                }
            }
            _tentandoConectar = false;
        }

        private async Task OuvirServidorAsync()
        {
            byte[] lengthBuffer = new byte[4];
            try
            {
                while (_client != null && _client.Connected)
                {
                    if (!await LerBytesExatosAsync(_stream, lengthBuffer, 4)) break;
                    int packetLength = BitConverter.ToInt32(lengthBuffer, 0);

                    if (packetLength <= 0 || packetLength > 150 * 1024 * 1024) continue;

                    byte[] packetBuffer = new byte[packetLength];
                    if (!await LerBytesExatosAsync(_stream, packetBuffer, packetLength)) break;

                    byte type = packetBuffer[0];
                    byte[] payload = new byte[packetLength - 1];
                    Buffer.BlockCopy(packetBuffer, 1, payload, 0, payload.Length);

                    if (type == 0)
                    {
                        string msg = Encoding.UTF8.GetString(payload);

                        this.Invoke(new Action(() =>
                        {
                            string msgCheck = msg.Trim();
                            if (msgCheck.Equals("/cls", StringComparison.OrdinalIgnoreCase) ||
                                msgCheck.Equals("system:/cls", StringComparison.OrdinalIgnoreCase) ||
                                msgCheck.Equals("system:cls", StringComparison.OrdinalIgnoreCase))
                            {
                                pnlMensagens.Controls.Clear();
                                PopupForm.Show(null, "O chat foi limpo pelo servidor.");
                                return;
                            }

                            AdicionarLinhaMensagem(msg);

                            if (!msg.StartsWith($"{_userName}:") && !msg.StartsWith("Você:"))
                            {
                                string textoPopup = msg.StartsWith("system:") ? msg.Replace("system:", "").Trim() : msg;
                                PopupForm.Show(null, textoPopup);
                            }
                        }));
                    }
                    else if (type == 1)
                    {
                        if (payload.Length < 4) continue;

                        int nameLength = BitConverter.ToInt32(payload, 0);
                        if (nameLength <= 0 || nameLength > payload.Length - 4) continue;

                        string fileName = Encoding.UTF8.GetString(payload, 4, nameLength);

                        int fileBytesPos = 4 + nameLength;
                        int fileBytesLen = payload.Length - fileBytesPos;

                        byte[] fileBytes = new byte[fileBytesLen];
                        Buffer.BlockCopy(payload, fileBytesPos, fileBytes, 0, fileBytesLen);

                        string remetente = "Usuário Remoto";

                        this.Invoke(new Action(() => {
                            AdicionarLinhaArquivo(remetente, fileName, fileBytes);
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro no loop de leitura do cliente: {ex.Message}");
            }
            finally
            {
                ForçarDesconexao();
                if (_deveTentarReconectar) { _ = Task.Run(() => ConectarAoServidor()); }
            }
        }

        private static async Task<bool> LerBytesExatosAsync(NetworkStream stream, byte[] buffer, int totalRequisitado)
        {
            int lidos = 0;
            while (lidos < totalRequisitado)
            {
                int resultado = await stream.ReadAsync(buffer, lidos, totalRequisitado - lidos);
                if (resultado == 0) return false;
                lidos += resultado;
            }
            return true;
        }

        private async void EnviarPacote(byte tipo, byte[] dados)
        {
            if (_stream == null || !_client.Connected) return;
            try
            {
                int totalLength = 1 + dados.Length;
                byte[] lengthHeader = BitConverter.GetBytes(totalLength);
                await _stream.WriteAsync(lengthHeader, 0, 4);
                await _stream.WriteAsync(new byte[] { tipo }, 0, 1);
                if (dados.Length > 0) await _stream.WriteAsync(dados, 0, dados.Length);
            }
            catch { TratarDesconexao(); }
        }

        private void EnviarMensagemTexto()
        {
            if (string.IsNullOrWhiteSpace(txtInput.Text) || _stream == null) return;
            string textoBruto = txtInput.Text.Trim();

            if (textoBruto.StartsWith("/"))
            {
                byte[] data = Encoding.UTF8.GetBytes(textoBruto);
                EnviarPacote(0, data);
            }
            else if (textoBruto.StartsWith("@"))
            {
                int primeiroEspaco = textoBruto.IndexOf(' ');
                if (primeiroEspaco > 0)
                {
                    string nomeDestino = textoBruto.Substring(1, primeiroEspaco - 1);
                    string mensagemPrivada = textoBruto.Substring(primeiroEspaco + 1).Trim();

                    string comandoTraduzido = $"/pm {nomeDestino} {mensagemPrivada}";
                    byte[] data = Encoding.UTF8.GetBytes(comandoTraduzido);

                    EnviarPacote(0, data);
                    AdicionarLinhaMensagem($"[PM para {nomeDestino}]: {mensagemPrivada}");
                }
                else
                {
                    string payloadStr = $"{_userName}: {textoBruto}";
                    EnviarPacote(0, Encoding.UTF8.GetBytes(payloadStr));
                    AdicionarLinhaMensagem($"Você: {textoBruto}");
                }
            }
            else
            {
                string payloadStr = $"{_userName}: {textoBruto}";
                EnviarPacote(0, Encoding.UTF8.GetBytes(payloadStr));
                AdicionarLinhaMensagem($"Você: {textoBruto}");
            }

            txtInput.Clear();
            txtInput.Focus();
        }

        private void AdicionarLinhaMensagem(string rawText)
        {
            int splitIdx = rawText.IndexOf(':');
            string autor = splitIdx != -1 ? rawText.Substring(0, splitIdx).Trim() : "system";
            string mensagem = splitIdx != -1 ? rawText.Substring(splitIdx + 1).Trim() : rawText;
            string horaAtual = DateTime.Now.ToString("HH:mm");

            Panel containerRow = new Panel { Width = pnlMensagens.Width - 55, Margin = new Padding(0, 4, 0, 6), BackColor = Color.Transparent };

            if (autor.Equals("system", StringComparison.OrdinalIgnoreCase) || rawText.StartsWith("system:"))
            {
                // CORREÇÃO: Alterado de 'message.Replace' para 'mensagem.Replace'
                if (mensagem.StartsWith("system:")) mensagem = mensagem.Replace("system:", "").Trim();

                if (mensagem.Contains("entrou no chat."))
                {
                    string userMap = mensagem.Replace("entrou no chat.", "").Trim();
                    if (!_usuariosConectados.Contains(userMap) && userMap != _userName) _usuariosConectados.Add(userMap);
                }
                else if (mensagem.Contains("saiu do chat."))
                {
                    string userMap = mensagem.Replace("saiu do chat.", "").Trim();
                    messageClone = userMap;
                    _usuariosConectados.Remove(userMap);
                }
                // CORREÇÃO: Alterado de 'message.StartsWith' para 'mensagem.StartsWith'
                else if (mensagem.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || mensagem.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    LinkLabel link = new LinkLabel { Text = mensagem, ForeColor = appleAccent, AutoSize = true, Location = new Point(54, 24) };
                    link.LinkClicked += (s, e) => { try { Process.Start(new ProcessStartInfo(mensagem) { UseShellExecute = true }); } catch { } };
                    containerRow.Controls.Add(link);
                }
                else
                {
                    containerRow.Controls.Add(new Label { Text = rawText, ForeColor = appleTextSecondary, AutoSize = true, Location = new Point(54, 24) });
                }

                containerRow.Height = 35;
                Panel pnlAlert = new Panel { Size = new Size(containerRow.Width - 10, 32), Location = new Point(5, 2), BackColor = Color.FromArgb(242, 242, 247) };
                Label lblAlert = new Label { Text = $"⚙️  {mensagem} ({horaAtual})", Font = new Font("SF Pro Text", 9F, FontStyle.Italic), ForeColor = appleTextSecondary, Location = new Point(15, 7), AutoSize = true };
                pnlAlert.Controls.Add(lblAlert);
                containerRow.Controls.Add(pnlAlert);

                pnlMensagens.Controls.Add(containerRow);
                pnlMensagens.ScrollControlIntoView(containerRow);
                return;
            }

            Color corNome = appleAccent;
            Color corMsg = appleTextPrimary;
            Color corFundoCard = Color.Transparent;
            string nomeLimpo = autor;

            if (autor.StartsWith("Você")) { corNome = Color.FromArgb(52, 199, 89); } // Apple Green
            else if (autor.StartsWith("[PM"))
            {
                corNome = Color.FromArgb(175, 82, 222); // Apple Purple
                corMsg = Color.FromArgb(175, 82, 222);
                corFundoCard = Color.FromArgb(242, 242, 247);
                nomeLimpo = autor.Replace("[PM de ", "").Replace("[PM para ", "").Replace("]", "");
            }

            containerRow.BackColor = corFundoCard;

            Panel pnlAvatar = CriarAvatarPainel(nomeLimpo);

            Label lblHeader = new Label { Text = $"{autor}   ", Font = new Font("SF Pro Text", 10F, FontStyle.Bold), ForeColor = corNome, Location = new Point(54, 4), AutoSize = true };
            Label lblTimestamp = new Label { Text = horaAtual, Font = new Font("SF Pro Text", 8F), ForeColor = appleTextSecondary, Location = new Point(lblHeader.Location.X + lblHeader.PreferredWidth, 7), AutoSize = true };
            Label lblMensagem = new Label { Text = mensagem, Font = new Font("SF Pro Text", 10F), ForeColor = corMsg, Location = new Point(54, 24), Width = containerRow.Width - 70, AutoSize = true, MaximumSize = new Size(containerRow.Width - 70, 0) };

            containerRow.Controls.AddRange(new Control[] { pnlAvatar, lblHeader, lblTimestamp, lblMensagem });
            containerRow.Height = Math.Max(48, lblMensagem.Location.Y + lblMensagem.PreferredHeight + 8);

            pnlMensagens.Controls.Add(containerRow);
            pnlMensagens.ScrollControlIntoView(containerRow);
        }

        private void AdicionarLinhaArquivo(string autor, string fileName, byte[] fileBytes)
        {
            Panel containerRow = new Panel { Width = pnlMensagens.Width - 55, Margin = new Padding(0, 4, 0, 6) };
            string ext = Path.GetExtension(fileName).ToLower();
            string[] extensoesImagem = { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

            if (extensoesImagem.Contains(ext))
            {
                try
                {
                    PictureBox picPreview = new PictureBox
                    {
                        Size = new Size(320, 180),
                        Location = new Point(54, 22),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Cursor = Cursors.Hand,
                        BackColor = appleBg
                    };

                    MemoryStream ms = new MemoryStream(fileBytes);
                    picPreview.Image = Image.FromStream(ms);

                    picPreview.Disposed += (s, e) => {
                        picPreview.Image?.Dispose();
                        ms.Dispose();
                    };

                    picPreview.Click += (s, e) => BaixarArquivoDoChat(fileName, fileBytes);

                    Label lblRemetenteImg = new Label { Text = $"{autor} compartilhou uma imagem:", Font = new Font("SF Pro Text", 8.5F, FontStyle.Italic), ForeColor = appleTextSecondary, Location = new Point(54, 1), AutoSize = true };
                    Panel pnlAvatarImg = CriarAvatarPainel(autor);

                    containerRow.Controls.AddRange(new Control[] { pnlAvatarImg, lblRemetenteImg, picPreview });
                    containerRow.Height = 210;

                    pnlMensagens.Controls.Add(containerRow);
                    pnlMensagens.ScrollControlIntoView(containerRow);
                    return;
                }
                catch { }
            }

            containerRow.Height = 80;
            Panel pnlFileCard = new Panel { Size = new Size(520, 68), Location = new Point(54, 6), BackColor = appleSurface };
            Label lblIcon = new Label { Text = "📁", Font = new Font("SF Pro Text", 18F), Location = new Point(12, 14), AutoSize = true };
            Label lblFileName = new Label { Text = fileName, Font = new Font("SF Pro Text", 10F, FontStyle.Bold), ForeColor = appleTextPrimary, Location = new Point(55, 14), Width = 340, AutoEllipsis = true };
            Label lblFileInfo = new Label { Text = $"{fileBytes.Length / 1024 + 1} KB • Arquivo Compartilhado", Font = new Font("SF Pro Text", 8F), ForeColor = appleTextSecondary, Location = new Point(55, 36), AutoSize = true };

            Guna2Button btnDownload = new Guna2Button
            {
                Text = "Baixar",
                Size = new Size(80, 32),
                Location = new Point(425, 18),
                FillColor = appleBg,
                ForeColor = appleTextPrimary,
                Font = new Font("SF Pro Text", 9F, FontStyle.Bold),
                BorderRadius = 8,
                Cursor = Cursors.Hand
            };
            btnDownload.HoverState.FillColor = Color.FromArgb(229, 229, 234);
            btnDownload.Click += (s, e) => BaixarArquivoDoChat(fileName, fileBytes);

            pnlFileCard.Controls.AddRange(new Control[] { lblIcon, lblFileName, lblFileInfo, btnDownload });
            Label lblRemetente = new Label { Text = $"{autor} enviou um anexo:", Font = new Font("SF Pro Text", 8.5F, FontStyle.Italic), ForeColor = appleTextSecondary, Location = new Point(54, 1), AutoSize = true };

            containerRow.Controls.AddRange(new Control[] { CriarAvatarPainel(autor), lblRemetente, pnlFileCard });
            pnlMensagens.Controls.Add(containerRow);
            pnlMensagens.ScrollControlIntoView(containerRow);
        }

        private void BaixarArquivoDoChat(string fileName, byte[] fileBytes)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { FileName = fileName, Filter = "Todos os arquivos (*.*)|*.*" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllBytes(sfd.FileName, fileBytes);
                        PopupForm.Show(null, "Arquivo baixado com sucesso!");
                    }
                    catch (Exception ex) { MessageBox.Show($"Erro na escrita de disco: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
        }

        private Panel CriarAvatarPainel(string nome)
        {
            Panel pnlAvatar = new Panel { Size = new Size(38, 38), Location = new Point(6, 6) };
            Color corAvatar = ObterCorAvatarPorNome(nome);
            pnlAvatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Brush b = new SolidBrush(corAvatar)) { e.Graphics.FillEllipse(b, 0, 0, 36, 36); }
                string inicial = nome.Length > 0 ? nome.Substring(0, 1).ToUpper() : "?";
                using (Font f = new Font("SF Pro Text", 11F, FontStyle.Bold))
                {
                    SizeF size = e.Graphics.MeasureString(inicial, f);
                    e.Graphics.DrawString(inicial, f, Brushes.White, (36 - size.Width) / 2, (36 - size.Height) / 2);
                }
            };
            return pnlAvatar;
        }

        private void BtnAnexar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] fileBytes = File.ReadAllBytes(ofd.FileName);
                    string fileName = Path.GetFileName(ofd.FileName);
                    byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
                    byte[] nameLenBytes = BitConverter.GetBytes(nameBytes.Length);

                    byte[] payload = new byte[4 + nameBytes.Length + fileBytes.Length];
                    Buffer.BlockCopy(nameLenBytes, 0, payload, 0, 4);
                    Buffer.BlockCopy(nameBytes, 0, payload, 4, nameBytes.Length);
                    Buffer.BlockCopy(fileBytes, 0, payload, 4 + nameBytes.Length, fileBytes.Length);

                    EnviarPacote(1, payload);
                    AdicionarLinhaArquivo("Você", fileName, fileBytes);
                }
            }
        }

        private void TratarDesconexao()
        {
            if (this.IsDisposed || this.Disposing) return;
            try
            {
                this.Invoke(new Action(() => {
                    lblStatusRede.Text = "✕ Conexão com o servidor interrompida.";
                    lblStatusRede.ForeColor = Color.FromArgb(255, 59, 48); // Apple Red
                }));
            }
            catch { }
        }

        private void ForçarDesconexao()
        {
            _ouvinteAtivo = false;
            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }

            InvocarInterface(() => {
                _usuariosConectados.Clear();
                lblStatusRede.Text = "✕ Conexão Perdida. Reconectando...";
                lblStatusRede.ForeColor = Color.FromArgb(255, 59, 48); // Apple Red
            });
        }

        private void InvocarInterface(Action acao)
        {
            if (this.InvokeRequired) this.BeginInvoke(acao); else acao();
        }

        public void Desconectar()
        {
            try
            {
                _ouvinteAtivo = false;

                if (_client != null && _client.Connected)
                {
                    _client.Client.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(50);
                }

                if (_stream != null)
                {
                    _stream.Close();
                    _stream.Dispose();
                    _stream = null;
                }

                if (_client != null)
                {
                    _client.Close();
                    _client.Dispose();
                    _client = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao limpar socket: {ex.Message}");
            }
        }

        private Color ObterCorAvatarPorNome(string nome)
        {
            if (string.IsNullOrEmpty(nome)) return appleTextSecondary;
            int hash = nome.GetHashCode();
            int r = Math.Abs((hash & 0xFF0000) >> 16) % 140 + 80;
            int g = Math.Abs((hash & 0x00FF00) >> 8) % 140 + 80;
            int b = Math.Abs(hash & 0x0000FF) % 140 + 80;
            return Color.FromArgb(r, g, b);
        }
    }
}