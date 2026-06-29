using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using aether.Controle;

namespace aether
{
    public partial class FormAtribuicao : Form
    {
        private Guna2ComboBox cmbTecnicos, cmbCategoria;
        private Guna2TextBox txtNota, txtSeriais, txtDiagnostico;
        private Guna2Button btnConfirmar;

        // Propriedades acessíveis pelo Lab.cs
        public string TecnicoSelecionado => cmbTecnicos.SelectedItem?.ToString();
        public string Nota => txtNota.Text;
        public string Diagnostico => txtDiagnostico.Text;
        public string Categoria => cmbCategoria.SelectedItem?.ToString() ?? "Normal";

        public List<string> Seriais => txtSeriais.Text
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Distinct()
            .ToList();

        public FormAtribuicao(List<string> tecnicosOnline)
        {
            this.Text = "Atribuir Tarefas";
            this.Size = new Size(400, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            ThemeManager.InitializeTheme(this);
            // Design Moderno Apple-like
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            // Método auxiliar de estilo para inputs
            Action<Guna2TextBox> styleInput = (tb) => {
                tb.Width = 340;
                tb.BorderRadius = 8;
                tb.BorderThickness = 1;
            };

            txtSeriais = new Guna2TextBox { PlaceholderText = "Cole os identificadores...", Location = new Point(30, 30), Height = 100, Multiline = true };
            styleInput(txtSeriais);

            txtDiagnostico = new Guna2TextBox { PlaceholderText = "Diagnóstico padrão", Location = new Point(30, 145) };
            styleInput(txtDiagnostico);

            cmbCategoria = new Guna2ComboBox { Location = new Point(30, 195), Width = 340, BorderRadius = 8 };
            cmbCategoria.Items.AddRange(new string[] { "Normal", "Defeito", "Carcaça" });
            cmbCategoria.SelectedIndex = 0;

            cmbTecnicos = new Guna2ComboBox { Location = new Point(30, 245), Width = 340, DataSource = tecnicosOnline, BorderRadius = 8 };

            txtNota = new Guna2TextBox { PlaceholderText = "Nota adicional...", Location = new Point(30, 295), Height = 100, Multiline = true };
            styleInput(txtNota);

            // Botão de Ação (Mantido com cor fixa para não ser afetado pelo ThemeManager)
            btnConfirmar = new Guna2Button
            {
                Name = "btnConfirmar", // Nome importante para o ThemeManager ignorar
                Text = "ENVIAR TAREFAS",
                Location = new Point(30, 420),
                Width = 340,
                Height = 45,
                BorderRadius = 10,
                FillColor = Color.FromArgb(10, 132, 255), // Azul Apple
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            btnConfirmar.Click += (s, e) => {
                if (cmbTecnicos.SelectedItem == null) { MessageBox.Show("Selecione um técnico!"); return; }
                if (Seriais.Count == 0) { MessageBox.Show("Insira ao menos um identificador!"); return; }
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.AddRange(new Control[] { txtSeriais, txtDiagnostico, cmbCategoria, cmbTecnicos, txtNota, btnConfirmar });

            // Aplica o tema global ao abrir
  
        }
    }
}