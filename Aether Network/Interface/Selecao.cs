using System;
using System.Collections.Generic;
using System.Data;
using aether.Controle;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace aether
{
    public partial class Selecao : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        public class ItemDefeito
        {
            public string Codigo { get; set; }
            public string Descricao { get; set; }
            public override string ToString() => $"{Codigo} - {Descricao}";
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
            new ItemDefeito { Codigo = "N1", Descricao = "EQUIPAMENTO TESTADO E FUNCIONANDO" }
        };

        public List<ItemDefeito> Selecionados { get; private set; } = new List<ItemDefeito>();
        private CheckedListBox clbDefeitos;
        private TextBox txtBusca;

        public Selecao()
        {
            ConfigurarInterfaceCopiada();
            CarregarListaOriginal();
        }

        private void ConfigurarInterfaceCopiada()
        {
            // --- 1. CONFIGURAÇÕES IDENTICAS À SUA MSG (TAMANHO MAIOR) ---
            this.Size = new Size(600, 700);
            this.BackColor = Color.FromArgb(10, 10, 10);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 15, 15));

            // --- 2. TÍTULO ---
            Label lblHeader = new Label
            {
                Text = "SELECIONAR DIAGNÓSTICO",
                Font = new Font("Segoe UI Bold", 9),
                ForeColor = Color.FromArgb(150, 150, 150),
                Location = new Point(30, 20),
                AutoSize = true
            };
            this.Controls.Add(lblHeader);

            // --- 3. TEXTBOX DE BUSCA ---
            txtBusca = new TextBox
            {
                Location = new Point(30, 50),
                Size = new Size(this.Width - 60, 30),
                BackColor = Color.FromArgb(25, 25, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semilight", 11),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtBusca.TextChanged += (s, e) => FiltrarLista();
            this.Controls.Add(txtBusca);

            // --- 4. LISTA ---
            clbDefeitos = new CheckedListBox
            {
                Location = new Point(30, 100),
                Size = new Size(this.Width - 60, 480), // Aumentado para acompanhar o form
                BackColor = Color.FromArgb(10, 10, 10),
                ForeColor = Color.FromArgb(200, 200, 200),
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI Semilight", 10),
                CheckOnClick = true,
                Cursor = Cursors.Hand
            };
            this.Controls.Add(clbDefeitos);

            // --- 5. CONTAINER DE BOTÕES ---
            Panel pnlBotoes = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pnlBotoes);

            // Botão Confirmar (Estilo 'SIM' / 'ENTENDIDO')
            Button btnConfirmar = CriarBotaoAether("CONFIRMAR", Color.White, Color.Black, 260);
            btnConfirmar.Location = new Point(30, 15);
            btnConfirmar.Click += (s, e) => ExecutarConfirmacao();
            pnlBotoes.Controls.Add(btnConfirmar);

            // Botão Cancelar (Estilo 'NÃO')
            Button btnCancelar = CriarBotaoAether("CANCELAR", Color.FromArgb(30, 30, 30), Color.White, 260);
            btnCancelar.Location = new Point(this.Width - 290, 15);
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            pnlBotoes.Controls.Add(btnCancelar);

            // --- 6. PINTURA DE BORDA ---
            this.Paint += (s, e) => {
                using (Pen p = new Pen(Color.FromArgb(40, 40, 40), 1))
                    e.Graphics.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
            };

            // --- 7. MOVIMENTAÇÃO ---
            this.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0x112, 0xf012, 0); };

            this.AcceptButton = btnConfirmar;
            this.CancelButton = btnCancelar;
        }

        private Button CriarBotaoAether(string texto, Color back, Color fore, int largura)
        {
            Button btn = new Button
            {
                Text = texto,
                Size = new Size(largura, 45),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                Font = new Font("Segoe UI Bold", 9),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void ExecutarConfirmacao()
        {
            Selecionados = clbDefeitos.CheckedItems.Cast<ItemDefeito>().ToList();
            if (Selecionados.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                PopupForm.Show(this,"SELECIONE AO MENOS UM ITEM.");
            }
        }

        private void CarregarListaOriginal()
        {
            clbDefeitos.Items.Clear();
            foreach (var item in listaMestra) clbDefeitos.Items.Add(item);
        }

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