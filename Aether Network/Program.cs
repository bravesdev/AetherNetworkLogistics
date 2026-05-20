using aether;
using System;
using System.IO;
using System.Windows.Forms;

namespace aether
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Garante que a pasta Apps existe para não dar erro de caminho
            string folderPath = Path.Combine(Application.StartupPath, "metadados");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string pathLicenca = Path.Combine(folderPath, "ident.lic");

            // 1. Verifica se a licença já existe
            if (File.Exists(pathLicenca))
            {
                Application.Run(new NetworkEngine());
            }
            else
            {
                // 2. Se não existir, abre o Login
                using (FrmLogin login = new FrmLogin())
                {
                    // Mostra o login como uma caixa de diálogo
                    DialogResult result = login.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // Se o login retornou OK, abre o Hub
                        Application.Run(new NetworkEngine());
                    }
                    else
                    {
                        // Se fechou o login sem validar, encerra o app
                        Application.Exit();
                    }
                }
            }
        }
    }
}