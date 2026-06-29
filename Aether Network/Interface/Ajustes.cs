using aether.Controle;
using aether.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using static aether.Controle.ThemeManager;

namespace aether
{
    // A classe do Form DEVE ser a primeira do arquivo para manter o WinForms Designer funcional!
    public partial class Ajustes : Form
    {
        private Lab Home;
        private readonly string prefsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metadados\\preferences.json");
        private readonly string runRegistryKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private readonly string appName = "AetherNetwork";

        public Ajustes(Lab FormHome)
        {
            InitializeComponent();
            this.Home = FormHome;
            CarregarLogin();
            CarregarConfiguracaoRede();
            CarregarPreferenciasInicializacao();
            ThemeManager.InitializeTheme(this);
        }

        private void CarregarConfiguracaoRede()
        {
            try
            {
                if (File.Exists(prefsPath))
                {
                    string json = File.ReadAllText(prefsPath);
                    var prefs = JsonSerializer.Deserialize<LocalAppPreferences>(json);

                    if (prefs?.NetworkConfig != null && !string.IsNullOrEmpty(prefs.NetworkConfig.ServerIp))
                    {
                        lblConected.Text = $"Servidor Atual: {prefs.NetworkConfig.ServerIp}:{prefs.NetworkConfig.ServerPort}";
                    }
                    else
                    {
                        lblConected.Text = "Servidor Atual: Não configurado";
                    }
                }
                else
                {
                    lblConected.Text = "Servidor Atual: Não configurado";
                }
            }
            catch
            {
                lblConected.Text = "Servidor Atual: Erro ao ler arquivo";
            }
        }

        private void CarregarPreferenciasInicializacao()
        {
            try
            {
                if (File.Exists(prefsPath))
                {
                    string json = File.ReadAllText(prefsPath);
                    var prefs = JsonSerializer.Deserialize<LocalAppPreferences>(json);

                    if (prefs?.UserSettings != null)
                    {
                        ckbInicializar.CheckedChanged -= ckbInicializar_CheckedChanged;
                        ckbInicializar.Checked = prefs.UserSettings.Inicialization == "true";
                        ckbInicializar.CheckedChanged += ckbInicializar_CheckedChanged;
                    }
                }
            }
            catch
            {
                // Falha silenciosa
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string input = txtConection.Text.Trim();

            if (string.IsNullOrEmpty(input) || !input.Contains(":"))
            {
                PopupForm.Show(this, "Formato inválido! Use IP:Porta\n(Ex: 192.168.1.1:7777)");
                return;
            }

            string[] partes = input.Split(':');
            if (partes.Length != 2 || !int.TryParse(partes[1], out int novaPorta))
            {
                PopupForm.Show(this, "Porta inválida! Verifique o valor digitado.");
                return;
            }

            string novoIp = partes[0];

            try
            {
                LocalAppPreferences prefs = new LocalAppPreferences();

                if (File.Exists(prefsPath))
                {
                    string jsonExistente = File.ReadAllText(prefsPath);
                    prefs = JsonSerializer.Deserialize<LocalAppPreferences>(jsonExistente) ?? prefs;
                }

                if (prefs.NetworkConfig == null) prefs.NetworkConfig = new LocalNetworkConfig();

                prefs.NetworkConfig.ServerIp = novoIp;
                prefs.NetworkConfig.ServerPort = novaPorta;

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonSalvar = JsonSerializer.Serialize(prefs, options);
                File.WriteAllText(prefsPath, jsonSalvar);

                lblConected.Text = $"Servidor Atual: {novoIp}:{novaPorta}";
                txtConection.Text = "";

                PopupForm.Show(this, "Configurações de rede updated com sucesso!");
            }
            catch (Exception ex)
            {
                PopupForm.Show(this, $"Erro ao guardar definições de rede: {ex.Message}");
            }
        }

        private void CarregarLogin()
        {
            ThemeManager.InitializeTheme(this);
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metadados\\ident.lic");

            if (File.Exists(filePath))
            {
                try
                {
                    string[] linhas = File.ReadAllLines(filePath);
                    foreach (string linha in linhas)
                    {
                        if (linha.StartsWith("AUTHORIZED_USER:"))
                        {
                            string email = linha.Replace("AUTHORIZED_USER:", "").Trim();
                            lblEmail.Text = email;

                            string userPart = email.Split('@')[0];
                            string[] partes = userPart.Split('.');
                            string nomeFormatado = "";

                            foreach (string parte in partes)
                            {
                                if (parte.Length > 0)
                                {
                                    nomeFormatado += char.ToUpper(parte[0]) + parte.Substring(1) + " ";
                                }
                            }

                            lblUsuario.Text = nomeFormatado.Trim();
                            break;
                        }
                    }
                }
                catch
                {
                    lblUsuario.Text = "Erro ao carregar";
                    lblEmail.Text = "";
                }
            }
        }

        private void btnTheme_Click(object sender, EventArgs e)
        {
            Home.AlternarTemaGlobal();

            try
            {
                LocalAppPreferences prefs = new LocalAppPreferences();

                if (File.Exists(prefsPath))
                {
                    string jsonExistente = File.ReadAllText(prefsPath);
                    prefs = JsonSerializer.Deserialize<LocalAppPreferences>(jsonExistente) ?? prefs;
                }

                if (prefs.UserSettings == null) prefs.UserSettings = new LocalUserSettings();

                string temaAtual = (ThemeManager.CurrentTheme == ThemeMode.Dark) ? "Dark" : "Light";
                prefs.UserSettings.Theme = temaAtual;

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonSalvar = JsonSerializer.Serialize(prefs, options);
                File.WriteAllText(prefsPath, jsonSalvar);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao persistir tema no JSON: {ex.Message}");
            }
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

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    if (Msg.Question("Isso irá substituir todos os registros atuais pelos dados do backup selecionado.\nDeseja continuar?"))
                    {
                        try
                        {
                            if (File.Exists(Lab.NomeArquivoLogs))
                            {
                                string nomeAutoSave = $"pre_import_autosave_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                                File.Copy(Lab.NomeArquivoLogs, Path.Combine(pastaBackup, nomeAutoSave), true);
                            }

                            File.Copy(openFile.FileName, Lab.NomeArquivoLogs, true);

                            Home.CarregarLogsIniciais();
                            Home.AtualizarInterface();

                            PopupForm.Show(this, "Os dados foram importados corretamente.");
                        }
                        catch (Exception ex) { PopupForm.Show(this, $"ERRO AO IMPORTAR:\n{ex.Message}"); }
                    }
                }
            }
        }

        private void btnFazerBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string pastaBackup = Path.Combine(Application.StartupPath, "Backup_Seguranca");
                if (!Directory.Exists(pastaBackup)) Directory.CreateDirectory(pastaBackup);

                if (File.Exists(Lab.NomeArquivoLogs))
                {
                    string nomeArquivo = $"manual_backup_{DateTime.Now:dd-MM-yyyy_HH-mm}.json";
                    string destino = Path.Combine(pastaBackup, nomeArquivo);

                    File.Copy(Lab.NomeArquivoLogs, destino, true);

                    PopupForm.Show(this, $"Backup realizado com sucesso!\nSalvo como: {nomeArquivo}");
                }
                else
                {
                    PopupForm.Show(this, "Não há logs para fazer backup no momento.");
                }
            }
            catch (Exception ex) { PopupForm.Show(this, $"Erro ao criar backup: {ex.Message}"); }
        }

        private void chkAutoConectar_CheckedChanged(object sender, EventArgs e)
        {
            PopupForm.Show(null, $"{(chkAutoConectar.Checked ? "Conectado a rede segura da welabs!" : "Você foi desconectado da rede segura da welabs.")}.");
        }

        private void ckbInicializar_CheckedChanged(object sender, EventArgs e)
        {
            bool iniciarComWindows = ckbInicializar.Checked;

            try
            {
                LocalAppPreferences prefs = new LocalAppPreferences();

                if (File.Exists(prefsPath))
                {
                    string jsonExistente = File.ReadAllText(prefsPath);
                    prefs = JsonSerializer.Deserialize<LocalAppPreferences>(jsonExistente) ?? prefs;
                }

                if (prefs.UserSettings == null) prefs.UserSettings = new LocalUserSettings();

                prefs.UserSettings.Inicialization = iniciarComWindows ? "true" : "false";

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonSalvar = JsonSerializer.Serialize(prefs, options);
                File.WriteAllText(prefsPath, jsonSalvar);
            }
            catch (Exception ex)
            {
                PopupForm.Show(this, $"Erro ao salvar preferência no JSON: {ex.Message}");
            }

            try
            {
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(runRegistryKey, true))
                {
                    if (rk != null)
                    {
                        if (iniciarComWindows)
                        {
                            rk.SetValue(appName, Application.ExecutablePath);
                            PopupForm.Show(this, "Aplicação configurada para iniciar junto ao Windows.");
                        }
                        else
                        {
                            if (rk.GetValue(appName) != null)
                            {
                                rk.DeleteValue(appName, false);
                            }
                            PopupForm.Show(this, "Inicialização automática com o Windows desativada.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PopupForm.Show(this, $"Erro ao atualizar o registro do Windows: {ex.Message}");
            }
        }
    }

    // CLASSES MAPPEADAS NO FINAL: Evita conflito global e mantém integridade do Designer 100% intacta
    public class LocalUserSettings
    {
        [JsonPropertyName("Theme")]
        public string Theme { get; set; } = "Light";

        [JsonPropertyName("Inicialization")]
        public string Inicialization { get; set; } = "false";
    }

    public class LocalNetworkConfig
    {
        [JsonPropertyName("ServerIp")]
        public string ServerIp { get; set; }

        [JsonPropertyName("ServerPort")]
        public int ServerPort { get; set; }
    }

    public class LocalAppPreferences
    {
        [JsonPropertyName("User-Settings")]
        public LocalUserSettings UserSettings { get; set; } = new LocalUserSettings();

        [JsonPropertyName("NetworkConfig")]
        public LocalNetworkConfig NetworkConfig { get; set; } = new LocalNetworkConfig();
    }
}