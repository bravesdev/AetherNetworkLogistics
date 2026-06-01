using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using aether.Controle;
using aether.Properties;
using System.Windows.Forms;

namespace aether
{
    public partial class Ajustes : Form
    {
        private Home Home; 
        public Ajustes(Home FormHome)
        {
            InitializeComponent();
            CarregarLogin();
        }
        private void CarregarLogin()
        {
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
                            // 1. Extrai o email da linha
                            string email = linha.Replace("AUTHORIZED_USER:", "").Trim();

                            // Exibe o email completo na lblEmail
                            lblEmail.Text = email;

                            // 2. Extrai o nome do usuário ('wenderson.dias')
                            string userPart = email.Split('@')[0];

                            // 3. Transforma 'wenderson.dias' em 'Wenderson Dias'
                            string[] partes = userPart.Split('.');
                            string nomeFormatado = "";

                            foreach (string parte in partes)
                            {
                                if (parte.Length > 0)
                                {
                                    nomeFormatado += char.ToUpper(parte[0]) + parte.Substring(1) + " ";
                                }
                            }

                            // 4. Exibe o nome formatado na lblUsuario
                            lblUsuario.Text = nomeFormatado.Trim();
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblUsuario.Text = "Erro ao carregar";
                    lblEmail.Text = "";
                }
            }
        }
        private void btnTheme_Click(object sender, EventArgs e)
        {

            // Apenas avisa o Home para alternar o tema
            Home.AlternarTemaGlobal();
        
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
                            if (File.Exists(Home.NomeArquivoLogs))
                            {
                                string nomeAutoSave = $"pre_import_autosave_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                                File.Copy(Home.NomeArquivoLogs, Path.Combine(pastaBackup, nomeAutoSave), true);
                            }

                            // 2. Copia o arquivo selecionado para o local do log oficial (sobrescrevendo)
                            File.Copy(openFile.FileName, Home.NomeArquivoLogs, true);

                            // 3. Recarrega a lista na memória e atualiza a interface
                            Home.CarregarLogsIniciais();
                            Home.AtualizarInterface();

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

        private void btnFazerBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string pastaBackup = Path.Combine(Application.StartupPath, "Backup_Seguranca");
                if (!Directory.Exists(pastaBackup)) Directory.CreateDirectory(pastaBackup);

                if (File.Exists(Home.NomeArquivoLogs))
                {
                    // Gera um nome com data e hora para não sobrescrever backups manuais anteriores
                    string nomeArquivo = $"manual_backup_{DateTime.Now:dd-MM-yyyy_HH-mm}.json";
                    string destino = Path.Combine(pastaBackup, nomeArquivo);

                    File.Copy(Home.NomeArquivoLogs, destino, true);

                    PopupForm.Show(this, $"Backup realizado com sucesso!\nSalvo como: {nomeArquivo}");
                
                }
                else
                {
                    PopupForm.Show(this, "Não há logs para fazer backup no momento.");
                }
            }
            catch (Exception ex)
            {
                PopupForm.Show(this, $"Erro ao criar backup: {ex.Message}");
            }
        }
    }
}
