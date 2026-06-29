using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace aether.Controle
{
    public class AetherAI
    {
        private const string API_KEY = "";// COLOQUE AQUI SUA CHAVE DE API DA GROQ
        private const string URL_ENDPOINT = "https://api.groq.com/openai/v1/chat/completions";

        private string ObterCaminhoBloqueio()
        {
            string dir = "metadados";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(Environment.MachineName + "AETHER_SECURE_KEY"));
                string hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                return $"{dir}\\{hash}.tmp";
            }
        }

        private string ObterEmailUsuario()
        {
            try
            {
                if (File.Exists("metadados\\ident.lic"))
                {
                    string conteudo = File.ReadAllText("metadados\\ident.lic");
                    var linha = conteudo.Split('\n').FirstOrDefault(l => l.Contains("AUTHORIZED_USER:"));
                    return linha?.Replace("AUTHORIZED_USER:", "").Trim();
                }
            }
            catch { }
            return null;
        }

        public async Task<string> ObterResposta(string pergunta)
        {
            string path = ObterCaminhoBloqueio();

            if (File.Exists(path))
            {
                return "Protocolo WD211 ativado: Identificamos linguagem incompatível com nossas diretrizes de uso. O acesso a esta conta foi suspenso permanentemente.";
            }

            string[] palavrasProibidas = { "PORRA", "CARALHO", "FODA", "PUTA", "PUTA QUE PARIU", "BUCETA", "CARALHA", "CACETE", "MERDA", "BOSTA", "VIADO", "CU", "CÚ", "BABACA", "IDIOTA", "IMBECIL", "RETARDADO", "FILHO DA PUTA", "DESGRAÇADO", "VAGABUNDO", "VAGABUNDA", "FODASE", "FODA-SE" };

            if (palavrasProibidas.Any(p => pergunta.ToUpper().Contains(p)))
            {
                EnviarAlertas(Environment.UserName, pergunta);
                File.Create(path).Close();
                return "Identificamos linguagem incompatível. O acesso foi suspenso e aguarde até que um administrador entre em contato.";
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");
                    var requestBody = new { model = "llama-3.1-8b-instant", messages = new[] { new { role = "system", content = "Você é a Sora, assistente de IA da welabs, você está aqui para responder duvidas técnicas sobre Hardware, ONUs, Roteadores e qualquer coisa relacionada a tecnologia e rede de computadores. Não interaja de forma pessoal com os usuarios. RESPONDA APENAS DUVIDAS TECNINCAS" }, new { role = "user", content = pergunta } }, temperature = 0.7 };
                    var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(URL_ENDPOINT, content);
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                        return result.choices[0].message.content;
                    }
                    return $"[SISTEMA]: Erro ({response.StatusCode})";
                }
            }
            catch (Exception ex) { return $"[SISTEMA]: Falha: {ex.Message}"; }
        }

        private void EnviarAlertas(string usuario, string msg)
        {
            try
            {
                string emailUsuario = ObterEmailUsuario();
                var smtp = new SmtpClient("smtp.gmail.com", 587) { Credentials = new NetworkCredential("welabsme@gmail.com", "jzjj tcnx qejq yvaz"), EnableSsl = true };

                var mailAdm = new MailMessage("wenderson.dias@valenet.com.br", "wenderson.dias@valenet.com.br")
                {
                    Subject = "welabs - Bloqueio de Usuario",
                    IsBodyHtml = true,
                    Body = $@"
    <div style='font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; background-color: #f8f9fa; padding: 20px; border-radius: 15px;'>
        <div style='background: linear-gradient(135deg, #b71c1c 0%, #8e0000 100%); color: #ffffff; padding: 30px; text-align: center; border-radius: 12px 12px 0 0;'>
            <h1 style='margin: 0; font-size: 26px; letter-spacing: 1px;'>RELATÓRIO DE VIOLAÇÃO</h1>
            <p style='margin: 5px 0 0; opacity: 0.8;'>Sistema de Segurança Aether AI</p>
        </div>
        
        <div style='background-color: #ffffff; padding: 30px; border-radius: 0 0 12px 12px; box-shadow: 0 10px 20px rgba(0,0,0,0.05);'>
            <p style='color: #444; font-size: 16px;'>Uma violação de conduta foi registrada pelo protocolo <strong>WD211</strong>.</p>
            
            <table style='width: 100%; margin-top: 25px; border-collapse: separate; border-spacing: 0 10px;'>
                <tr>
                    <td style='padding: 12px; background-color: #f1f1f1; border-radius: 6px 0 0 6px; font-weight: bold; width: 40%;'>Usuário Máquina</td>
                    <td style='padding: 12px; background-color: #ffffff; border: 1px solid #eee; border-radius: 0 6px 6px 0;'>{usuario}</td>
                </tr>
                <tr>
                    <td style='padding: 12px; background-color: #f1f1f1; border-radius: 6px 0 0 6px; font-weight: bold;'>Termo Detectado</td>
                    <td style='padding: 12px; background-color: #ffffff; border: 1px solid #eee; border-radius: 0 6px 6px 0; color: #d32f2f;'><em>{msg}</em></td>
                </tr>
                <tr>
                    <td style='padding: 12px; background-color: #f1f1f1; border-radius: 6px 0 0 6px; font-weight: bold;'>Status Atual</td>
                    <td style='padding: 12px; background-color: #ffffff; border: 1px solid #eee; border-radius: 0 6px 6px 0; font-weight: bold; color: #2e7d32;'>BLOQUEADO PERMANENTEMENTE</td>
                </tr>
            </table>

            <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; text-align: center;'>
                <a href='#' style='background-color: #333; color: #ffffff; padding: 12px 25px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Acessar Painel de Logs</a>
            </div>
        </div>
    </div>"
                };
                smtp.Send(mailAdm);

                // --- DESIGN EMAIL USUÁRIO (NOTIFICAÇÃO FORMAL) ---
                if (!string.IsNullOrEmpty(emailUsuario))
                {
                    var mailUser = new MailMessage("welabsme@gmail.com", emailUsuario)
                    {
                        Subject = "Aviso: Suspensão de Acesso - Aether Cloud",
                        IsBodyHtml = true,
                        Body = $@"
                <div style='font-family: Arial; max-width: 600px; margin: auto; padding: 20px; border-top: 4px solid #333;'>
                    <h2 style='color: #333;'>Notificação de Suspensão</h2>
                    <p>Olá,</p>
                    <p>Informamos que o seu acesso à IA <b>Sora (welabs Cloud)</b> foi suspenso devido à detecção de linguagem inapropriada, violando nossas políticas de conduta.</p>
                    <div style='background: #f4f4f4; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        Esta decisão é baseada no protocolo automático de segurança WD211.
                    </div>
                    <p>Caso tenha dúvidas, entre em contato com o administrador do sistema.</p>
                    <small>welabs Cloud</small>
                </div>"
                    };
                    smtp.Send(mailUser);
                }

                Msg.Show($"Você teve seu acesso bloqueado. Um administrador foi notificado.\n\nUsuário: {usuario}");
            }
            catch { }
        }
    }
}