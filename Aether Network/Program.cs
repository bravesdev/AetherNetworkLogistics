using aether;
using System;
using System.IO;
using System.Threading; // Necessário para usar o Mutex
using System.Windows.Forms;
using aether.Controle;

namespace aether
{
    static class Program
    {
        // Cria um identificador único para a sua aplicação no Windows.
        private static readonly string MutexName = "Global\\AetherNetwork_Unique_Instance_Mutex_Key";

        [STAThread]
        static void Main()
        {
            // O Mutex verifica se já existe uma instância com o mesmo nome rodando no Windows
            using (Mutex mutex = new Mutex(true, MutexName, out bool novaInstancia))
            {
                // Se 'novaInstancia' for false, significa que o programa já está aberto
               /* if (!novaInstancia)
                {
                    Msg.Show("O Aether Network já está em execução.");
                    return; // Encerra a nova instância imediatamente antes de carregar qualquer interface
                }*/

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Garante que a pasta Apps existe para não dar erro de caminho
                string folderPath = Path.Combine(Application.StartupPath, "metadados");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string pathLicenca = Path.Combine(folderPath, "ident.lic");

                // 1. Verifica se a licença já existe
                if (File.Exists(pathLicenca))
                {
                    Application.Run(new HomePage());
                }
                else
                {
                    // 2. Se não existir, abre o Login
                    using (Login login = new Login())
                    {
                        // Mostra o login como uma caixa de diálogo
                        DialogResult result = login.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            // Se o login retornou OK, abre o Hub
                            Application.Run(new HomePage());
                        }
                        else
                        {
                            // Se fechou o login sem validar, encerra o app
                            Application.Exit();
                        }
                    }
                }
            } // O Mutex é liberado aqui quando a aplicação principal é fechada pelo usuário
        }
    }
}