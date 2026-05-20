using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aether
{
    public partial class FrmLogin : Form
    {
        private string codigoGerado = "";
        private const string DOMINIO_PERMITIDO = "@valenet.com.br";

        // CONFIGURAÇÃO DO EMISSOR
        private const string EMAIL_SUPORTE = "welabsme@gmail.com";
        private const string SENHA_APP = "jzjj tcnx qejq yvaz";

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public FrmLogin()
        {
            InitializeComponent();
            VincularEventosNativos();
        }

        private void VincularEventosNativos()
        {
            // Vincula as ações lógicas aos controles instanciados no designer
            btnAcao.Click += async (s, e) => {
                if (btnAcao.Text == "SOLICITAR ACESSO")
                {
                    await ProcessarEnvioEmail();
                }
                else
                {
                    VerificarCodigo();
                }
            };

            btnFechar.Click += (s, e) =>
            {
                Application.Exit();
            };

            this.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); };
        }

        private async Task ProcessarEnvioEmail()
        {
            string email = txtEmail.Text.Trim().ToLower();

            if (!email.EndsWith(DOMINIO_PERMITIDO))
            {
                Msg.Show("EMAIL INVALIDO\nUtilize seu email Valenet para acessar a ferramenta");
                return;
            }

            btnAcao.Enabled = false;
            btnAcao.Text = "ENVIANDO...";

            Random rnd = new Random();
            codigoGerado = rnd.Next(100000, 999999).ToString();

            bool sucesso = await EnviarEmailSmtp(email, codigoGerado);

            if (sucesso)
            {
                txtEmail.ReadOnly = true;
                txtCode.Visible = true;
                btnAcao.Text = "VALIDAR CÓDIGO";
                btnAcao.Enabled = true;
                Msg.Show("Código enviado! Verifique sua caixa de entrada.");
            }
            else
            {
                btnAcao.Text = "SOLICITAR ACESSO";
                btnAcao.Enabled = true;
                Msg.Show("Falha ao enviar e-mail. Verifique sua conexão.");
            }
        }

        private async Task<bool> EnviarEmailSmtp(string destino, string code)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(EMAIL_SUPORTE, "welabs.me | Aether Network");
                    mailMessage.Subject = code + " é seu código de autenticação";
                    mailMessage.To.Add(destino);

                    string caminhoLogo = Path.Combine(Application.StartupPath, "logo.png");
                    AlternateView htmlView;
                    string corDestaque = "#FF3333";

                    if (File.Exists(caminhoLogo))
                    {
                        string headerHtml = $"<img src='cid:logo_img' style='max-width: 200px; height: auto; display: block; margin: 0 auto;' alt='Logo' />";

                        string htmlBody = $@"
                        <div style='background-color: #f9f9f9; padding: 40px 10px; font-family: ""Segoe UI"", Helvetica, Arial, sans-serif;'>
                            <div style='max-width: 460px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05); border-top: 4px solid {corDestaque};'>
                                
                                <div style='padding: 32px 32px 20px 32px; text-align: center; background-color: #ffffff;'>
                                    {headerHtml}
                                </div>

                                <div style='padding: 0 32px 32px 32px; text-align: center;'>
                                    <p style='color: #4A5568; font-size: 15px; line-height: 24px; margin: 0 0 24px 0;'>
                                        Olá, colaborador.<br>Foi solicitada uma tentativa de acesso à sua conta corporativa. Use o código de verificação abaixo para confirmar sua identidade:
                                    </p>

                                    <div style='margin: 24px 0; padding: 20px; background-color: #F7FAFC; border: 1px solid #E2E8F0; border-radius: 6px;'>
                                        <input type='text' value='{code}' readonly onclick='this.select();' style='width: 100%; max-width: 220px; background: transparent; border: none; font-family: ""Consolas"", Monaco, monospace; font-size: 32px; font-weight: bold; color: #1A202C; text-align: center; letter-spacing: 6px; outline: none; cursor: pointer;' title='Clique para selecionar o código' />
                                        <p style='color: #718096; font-size: 11px; margin: 8px 0 0 0;'>
                                            💡 Clique sobre os números para selecioná-los e copiar rápido
                                        </p>
                                    </div>

                                    <p style='color: #E53E3E; font-size: 12px; font-weight: 500; margin: 24px 0 0 0;'>
                                        Se você não solicitou este código, ignore este e-mail com segurança.
                                    </p>
                                </div>
                            </div>
                        </div>";

                        htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

                        LinkedResource inlineLogo = new LinkedResource(caminhoLogo, MediaTypeNames.Image.Png);
                        inlineLogo.ContentId = "logo_img";
                        htmlView.LinkedResources.Add(inlineLogo);
                    }
                    else
                    {
                        string htmlBodyDefault = $@"
                        <div style='font-family: Arial; border: 1px solid #ddd; padding: 20px; max-width: 400px; border-top: 4px solid {corDestaque};'>
                            <h2 style='color: {corDestaque}; text-align: center;'>AETHER CLOUD</h2>
                            <p>Seu código de acesso é:</p>
                            <div style='background: #f4f4f4; padding: 15px; text-align: center; font-size: 24px; font-weight: bold; letter-spacing: 4px;'>
                                {code}
                            </div>
                            <p style='font-size: 11px; color: #888; margin-top: 20px;'>
                                Este código foi solicitado para o e-mail: {destino}
                            </p>
                        </div>";

                        htmlView = AlternateView.CreateAlternateViewFromString(htmlBodyDefault, null, MediaTypeNames.Text.Html);
                    }

                    mailMessage.AlternateViews.Add(htmlView);

                    using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Port = 587;
                        smtpClient.Credentials = new NetworkCredential(EMAIL_SUPORTE, SENHA_APP);
                        smtpClient.EnableSsl = true;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;

                        await smtpClient.SendMailAsync(mailMessage);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERRO SMTP: " + ex.Message);
                return false;
            }
        }

        private void VerificarCodigo()
        {
            if (txtCode.Text.Trim() == codigoGerado)
            {
                try
                {
                    string folder = Path.Combine(Application.StartupPath, "metadados");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string pathLicenca = Path.Combine(folder, "ident.lic");
                    File.WriteAllText(pathLicenca, "AUTHORIZED_USER: " + txtEmail.Text);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    Msg.Show("Erro ao salvar licença local: " + ex.Message);
                }
            }
            else
            {
                Msg.Show("Código incorreto. Tente novamente.");
            }
        }
    }
}