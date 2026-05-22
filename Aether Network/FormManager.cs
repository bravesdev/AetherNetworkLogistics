using System;
using System.Windows.Forms;

namespace aether
{
    public static class FormManager
    {
        private static readonly object Padlock = new object();

        /// <summary>
        /// Abre um formulário flutuante (janela independente) garantindo que apenas uma instância exista.
        /// </summary>
        /// <typeparam name="T">O tipo do formulário (ex: FrmAetherAI)</typeparam>
        public static void OpenSingleForm<T>() where T : Form, new()
        {
            lock (Padlock)
            {
                string targetName = typeof(T).Name;

                // Varre todos os formulários abertos no Windows pelo aplicativo
                foreach (Form openForm in Application.OpenForms)
                {
                    // Verifica por tipo ou pelo nome real da classe na memória
                    if ((openForm is T || openForm.GetType().Name == targetName) && !openForm.IsDisposed)
                    {
                        if (openForm.WindowState == FormWindowState.Minimized)
                        {
                            openForm.WindowState = FormWindowState.Normal;
                        }

                        openForm.Activate();
                        openForm.BringToFront();
                        return; // Encontrou a janela aberta, foca nela e cancela a criação de uma nova
                    }
                }

                // Se não encontrou nenhuma instância rodando, cria e exibe
                T newForm = new T();
                newForm.Show();
            }
        }

        /// <summary>
        /// Use este método se o seu botão abre o formulário "embutido" dentro de um Panel (como se fosse uma aba).
        /// </summary>
        /// <typeparam name="T">O tipo do formulário (ex: FrmAetherAI)</typeparam>
        /// <param name="containerPanel">O Painel da tela principal que vai receber a janela</param>
        public static void OpenFormInPanel<T>(Panel containerPanel) where T : Form, new()
        {
            lock (Padlock)
            {
                string targetName = typeof(T).Name;

                // 1. Procura se o formulário já está injetado dentro dos controles do painel
                foreach (Control control in containerPanel.Controls)
                {
                    if ((control is T || control.GetType().Name == targetName) && !control.IsDisposed)
                    {
                        control.BringToFront();
                        control.Focus();
                        return; // Já está aberto no painel, joga pra frente e para aqui
                    }
                }

                // 2. Se tinha outra tela aberta no painel, limpa ela da memória antes de abrir a nova
                if (containerPanel.Controls.Count > 0)
                {
                    foreach (Control oldControl in containerPanel.Controls)
                    {
                        oldControl.Dispose();
                    }
                    containerPanel.Controls.Clear();
                }

                // 3. Configura o formulário para se comportar como um controle interno (aba)
                T newForm = new T
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };

                containerPanel.Controls.Add(newForm);
                newForm.Show();
                newForm.BringToFront();
            }
        }
    }
}