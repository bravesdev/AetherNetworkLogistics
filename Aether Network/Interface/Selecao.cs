using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using aether.Controle;

namespace aether
{
    public partial class Selecao : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")] private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")] private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")] private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        public class ItemDefeito
        {
            public string Codigo { get; set; }
            public string Descricao { get; set; }
            public override string ToString() => $" {Descricao}";
        }

        private List<ItemDefeito> listaMestra = new List<ItemDefeito> {
            new ItemDefeito { Codigo = "C1", Descricao = "EQUIPAMENTO COM CARCAÇA DANIFICADA" },
            new ItemDefeito { Codigo = "D1", Descricao = "EQUIPAMENTO ACENDE SOMENTE LED POWER" },
            new ItemDefeito { Codigo = "D2", Descricao = "EQUIPAMENTO APRESENTANDO LOS" },
            new ItemDefeito { Codigo = "D3", Descricao = "EQUIPAMENTO APRESENTANDO SINAL RX ALTO" },
            new ItemDefeito { Codigo = "D4", Descricao = "EQUIPAMENTO APRESENTANDO SINAL RX BAIXO" },
            new ItemDefeito { Codigo = "D5", Descricao = "EQUIPAMENTO APRESENTANDO SINAL TX ALTO" },
            new ItemDefeito { Codigo = "D6", Descricao = "EQUIPAMENTO APRESENTANDO SINAL TX BAIXO" },
            new ItemDefeito { Codigo = "D7", Descricao = "EQUIPAMENTO COM CONECTOR PON DANIFICADO" },
            new ItemDefeito { Codigo = "D8", Descricao = "EQUIPAMENTO COM FALHA DE CONECTIVIDADE" },
            new ItemDefeito { Codigo = "D9", Descricao = "EQUIPAMENTO COM FALHA DE SOFTWARE" },
            new ItemDefeito { Codigo = "D10", Descricao = "EQUIPAMENTO COM FALHA NO WIFI 2.4G" },
            new ItemDefeito { Codigo = "D11", Descricao = "EQUIPAMENTO COM FALHA NO WIFI 2.4G/5G" },
            new ItemDefeito { Codigo = "D12", Descricao = "EQUIPAMENTO COM FALHA NO WIFI 5G" },
            new ItemDefeito { Codigo = "D13", Descricao = "EQUIPAMENTO COM PORTA DE TELEFONIA QUEIMADA" },
            new ItemDefeito { Codigo = "D14", Descricao = "EQUIPAMENTO COM PORTA LAN QUEIMADA" },
            new ItemDefeito { Codigo = "D15", Descricao = "EQUIPAMENTO NÃO PROVISIONA" },
            new ItemDefeito { Codigo = "D16", Descricao = "EQUIPAMENTO QUEIMADO" },
            new ItemDefeito { Codigo = "D17", Descricao = "EQUIPAMENTO REINICIANDO" },
            new ItemDefeito { Codigo = "D18", Descricao = "EQUIPAMENTO TRAVADO" },
            new ItemDefeito { Codigo = "D19", Descricao = "LEDS DE IDENTIFICAÇÃO DO EQUIPAMENTO QUEIMADOS" },
            new ItemDefeito { Codigo = "N4", Descricao = "EQUIPAMENTO ATUALIZADO PARA REDE NEUTRA, EQUIPAMENTO TESTADO E FUNCIONANDO" },
            new ItemDefeito { Codigo = "N3", Descricao = "EQUIPAMENTO COM A FIRMWARE ATUALIZADA, EQUIPAMENTO TESTADO E FUNCIONANDO" },
            new ItemDefeito { Codigo = "N2", Descricao = "EQUIPAMENTO ATUALIZADO PARA REDE NEUTRA" },
            new ItemDefeito { Codigo = "N1", Descricao = "EQUIPAMENTO TESTADO E FUNCIONANDO" },
        };

        public List<ItemDefeito> Selecionados { get; private set; } = new List<ItemDefeito>();
        private CheckedListBox clbDefeitos;
        private TextBox txtBusca;

        public Selecao()
        {
            ConfigurarInterfaceApple();
            CarregarListaOriginal();
        }

        private void ConfigurarInterfaceApple()
        {
            // Tamanho reduzido para um formato de modal mais compacto
            this.Size = new Size(450, 600);
            this.BackColor = Color.FromArgb(248, 248, 248);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));

            // Título
            Label lblHeader = new Label
            {
                Text = "SELECIONAR DIAGNÓSTICO",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(25, 20),
                AutoSize = true
            };
            this.Controls.Add(lblHeader);

            // Busca (ajustada para a nova largura)
            txtBusca = new TextBox
            {
                Location = new Point(25, 45),
                Size = new Size(this.Width - 50, 30),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None
            };
            txtBusca.TextChanged += (s, e) => FiltrarLista();
            this.Controls.Add(txtBusca);

            // Lista (proporcional ao tamanho da janela)
            clbDefeitos = new CheckedListBox
            {
                Location = new Point(25, 85),
                Size = new Size(this.Width - 60, 450), // Altura ajustada
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9),
                CheckOnClick = true
            };
            this.Controls.Add(clbDefeitos);

            // Painel Botões (mais compacto)
            Panel pnlBotoes = new Panel { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.Transparent };
            this.Controls.Add(pnlBotoes);

            // Botões (largura ajustada para 180px cada)
            Button btnConfirmar = CriarBotaoApple("CONFIRMAR", Color.FromArgb(10, 132, 255), Color.White, 180);
            btnConfirmar.Location = new Point(25, 10);
            btnConfirmar.Click += (s, e) => ExecutarConfirmacao();
            pnlBotoes.Controls.Add(btnConfirmar);

            Button btnCancelar = CriarBotaoApple("CANCELAR", Color.FromArgb(220, 220, 220), Color.Black, 180);
            btnCancelar.Location = new Point(this.Width - 205, 10);
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            pnlBotoes.Controls.Add(btnCancelar);

            this.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); };
        }

        private Button CriarBotaoApple(string texto, Color back, Color fore, int largura)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = new Size(largura, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                // CORREÇÃO: Usar Bold ou Regular, nunca Semibold
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 10, 10));
            return btn;
        }

        private void ExecutarConfirmacao()
        {
            Selecionados = clbDefeitos.CheckedItems.Cast<ItemDefeito>().ToList();
            if (Selecionados.Count > 0) { this.DialogResult = DialogResult.OK; this.Close(); }
            else { MessageBox.Show("SELECIONE AO MENOS UM ITEM."); }
        }

        private void CarregarListaOriginal() { foreach (var item in listaMestra) clbDefeitos.Items.Add(item); }

        private void FiltrarLista()
        {
            string termo = txtBusca.Text.ToLower();
            var marcados = clbDefeitos.CheckedItems.Cast<ItemDefeito>().Select(x => x.Codigo).ToList();
            clbDefeitos.Items.Clear();
            foreach (var item in listaMestra)
            {
                if (item.Descricao.ToLower().Contains(termo) || item.Codigo.ToLower().Contains(termo))
                {
                    int idx = clbDefeitos.Items.Add(item);
                    if (marcados.Contains(item.Codigo)) clbDefeitos.SetItemChecked(idx, true);
                }
            }
        }
    }
}