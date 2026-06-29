using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Text.Json; // Adicionado para manipulação de JSON estruturado
using System.Text.Json.Serialization; // Adicionado para mapear o nome com hífen

namespace aether.Controle
{
    // --- CLASSES DE MODELAGEM DO JSON ---
    public class UserSettings
    {
        [JsonPropertyName("Theme")]
        public string Theme { get; set; } = "Light";
    }

    public class NetworkConfig
    {
        [JsonPropertyName("ServerIp")]
        public string ServerIp { get; set; } = "192.168.162.12";

        [JsonPropertyName("ServerPort")]
        public int ServerPort { get; set; } = 3356;
    }

    public class AppPreferences
    {
        [JsonPropertyName("User-Settings")]
        public UserSettings UserSettings { get; set; } = new UserSettings();

        [JsonPropertyName("NetworkConfig")]
        public NetworkConfig NetworkConfig { get; set; } = new NetworkConfig();
    }

    // --- GERENCIADOR DE CONFIGURAÇÕES (JSON) ---
    public static class SettingsManager
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metadados\\preferences.json");

        public static ThemeManager.ThemeMode LoadThemePreference()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    var prefs = JsonSerializer.Deserialize<AppPreferences>(json);

                    if (prefs?.UserSettings?.Theme == "Dark")
                    {
                        return ThemeManager.ThemeMode.Dark;
                    }
                }
            }
            catch
            {
                // Qualquer erro retorna o padrão seguro
            }
            return ThemeManager.ThemeMode.Light;
        }

        public static void SaveThemePreference(ThemeManager.ThemeMode theme)
        {
            try
            {
                string directory = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                AppPreferences prefs = new AppPreferences();

                // 1. Se o arquivo já existe, lê o estado atual para NÃO apagar o NetworkConfig do usuário
                if (File.Exists(FilePath))
                {
                    try
                    {
                        string jsonExistente = File.ReadAllText(FilePath);
                        prefs = JsonSerializer.Deserialize<AppPreferences>(jsonExistente) ?? prefs;
                    }
                    catch { /* Se o arquivo estiver corrompido, reinicia com o modelo padrão */ }
                }

                // 2. Atualiza apenas a propriedade do tema preservando o resto
                if (prefs.UserSettings == null) prefs.UserSettings = new UserSettings();
                prefs.UserSettings.Theme = theme.ToString();

                // 3. Serializa de volta formatado com recuo indetado
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonSalvar = JsonSerializer.Serialize(prefs, options);
                File.WriteAllText(FilePath, jsonSalvar);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar preferências: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    // --- GERENCIADOR DE TEMAS ESTÁTICO (APLICA EM TUDO, SALVA E MANTÉM TODO O SEU DESIGN) ---
    public static class ThemeManager
    {
        public enum ThemeMode { Light, Dark }
        public static ThemeMode CurrentTheme => SettingsManager.LoadThemePreference();

        // --- PALETA DE CORES DEFINITIVA: APPLE DARK MODE ---
        private static readonly Color DarkBg = Color.FromArgb(28, 28, 30);       // System Gray 6 (Fundo Principal)
        private static readonly Color DarkSide = Color.FromArgb(44, 44, 46);     // System Gray 5 (Paineis Elevados)
        private static readonly Color DarkInput = Color.FromArgb(58, 58, 60);    // System Gray 4 (Inputs e TextBoxes)
        private static readonly Color DarkText = Color.FromArgb(255, 255, 255);  // Branco Absoluto (Texto Primário)
        private static readonly Color DarkBorder = Color.FromArgb(72, 72, 74);   // System Gray 3 (Divisores e Bordas)

        // --- PALETA DE CORES DEFINITIVA: APPLE LIGHT MODE ---
        private static readonly Color LightBg = Color.FromArgb(242, 242, 247);   // System Gray 6 (Fundo Principal Mac/iOS)
        private static readonly Color LightSide = Color.FromArgb(255, 255, 255); // Pure White (Superfícies de Destaque)
        private static readonly Color LightInput = Color.FromArgb(255, 255, 255);
        private static readonly Color LightText = Color.FromArgb(0, 0, 0);       // Preto Absoluto (Texto Primário)
        private static readonly Color LightBorder = Color.FromArgb(229, 229, 234); // System Gray 5 (Divisores e Bordas)

        // Cor de Destaque Padrão Apple para seleção (Foco de Tabelas e Itens Ativos)
        private static readonly Color AppleAccentBlue = Color.FromArgb(10, 132, 255);

        /// <summary>
        /// Aplica o tema e salva no JSON. Também varre TODOS os formulários abertos e atualiza o design deles na mesma hora.
        /// Chame isso no botão de trocar de tema.
        /// </summary>
        public static void ApplyAndSaveThemeToAllForms(ThemeMode theme)
        {
            // 1. Salva no JSON preservando outras configurações
            SettingsManager.SaveThemePreference(theme);

            // 2. Atualiza todos os formulários abertos instantaneamente
            Form[] openForms = Application.OpenForms.Cast<Form>().ToArray();
            bool isDark = (theme == ThemeMode.Dark);

            foreach (Form frm in openForms)
            {
                if (frm != null && !frm.IsDisposed)
                {
                    RenderTransitionFrame(frm, isDark, 1.0);
                }
            }
        }

        /// <summary>
        /// Aplica o tema carregado do JSON.
        /// Chame isso no construtor de TODOS os seus formulários (logo após o InitializeComponent();)
        /// </summary>
        public static void InitializeTheme(Form form)
        {
            if (form == null) return;
            bool isDark = (CurrentTheme == ThemeMode.Dark);
            RenderTransitionFrame(form, isDark, 1.0);
        }

        private static void RenderTransitionFrame(Form form, bool toDark, double progress)
        {
            Color currentBg = InterpolateColor(toDark ? LightBg : DarkBg, toDark ? DarkBg : LightBg, progress);
            Color currentText = InterpolateColor(toDark ? LightText : DarkText, toDark ? DarkText : LightText, progress);

            form.BackColor = currentBg;
            form.ForeColor = currentText;

            ApplyFrameRecursive(form.Controls, toDark, progress);
        }

        private static void ApplyFrameRecursive(Control.ControlCollection controls, bool toDark, double progress)
        {
            Color currentBg = InterpolateColor(toDark ? LightBg : DarkBg, toDark ? DarkBg : LightBg, progress);
            Color currentSide = InterpolateColor(toDark ? LightSide : DarkSide, toDark ? DarkSide : LightSide, progress);
            Color currentInput = InterpolateColor(toDark ? LightInput : DarkInput, toDark ? DarkInput : LightInput, progress);
            Color currentText = InterpolateColor(toDark ? LightText : DarkText, toDark ? DarkText : LightText, progress);
            Color currentBorder = InterpolateColor(toDark ? LightBorder : DarkBorder, toDark ? DarkBorder : LightBorder, progress);

            foreach (Control c in controls)
            {
                string name = c.Name;

                if (name == "sidePanel" || name == "pnlCardAparencia" || name == "pnlCardRede" || name == "panel1" || name == "panel2" || name == "panel3")
                {
                    SetGunaPanelColor(c, currentSide);
                }
                else if (name == "lstPromptPing" && c is ListBox listBox)
                {
                    listBox.BackColor = currentInput;
                    listBox.ForeColor = currentText;
                }
                else if (name.StartsWith("label") || name.StartsWith("lbl"))
                {
                    c.ForeColor = currentText;
                    if (c is Label lbl) lbl.BackColor = Color.Transparent;
                }
                else if ((name.StartsWith("btn") || name.StartsWith("button")) && c is Button btn)
                {
                    btn.BackColor = currentSide;
                    btn.ForeColor = currentText;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = toDark ? 0 : 1;
                    btn.FlatAppearance.BorderColor = currentBorder;
                }
                else if (name == "txtIdentificador" || name == "txtPesquisa" || name == "txtEmail" || name == "txtCode" || name == "txtConection")
                {
                    SetGunaTextBoxColor(c, currentInput, currentText, currentBorder);
                }
                else if (name == "cbIPs" || c.GetType().Name.Contains("Guna2ComboBox"))
                {
                    var type = c.GetType();

                    var propFill = type.GetProperty("FillColor");
                    if (propFill != null) propFill.SetValue(c, currentInput);

                    c.ForeColor = currentText;

                    var propItemsBack = type.GetProperty("ItemsBackColor");
                    if (propItemsBack != null) propItemsBack.SetValue(c, currentInput);

                    var propBorder = type.GetProperty("BorderColor");
                    if (propBorder != null) propBorder.SetValue(c, currentBorder);
                }
                else if ((name == "checkBoxMultiline" || name == "CheckedListBox" || name == "chkAutoConectar") && (c is CheckBox || c.GetType().Name.Contains("Guna2CheckBox")))
                {
                    c.ForeColor = currentText;
                    c.BackColor = Color.Transparent;
                }
                else if (name == "dgvLogs" || c.GetType().Name.Contains("Guna2DataGridView"))
                {
                    if (progress >= 0.8) SetGunaDataGridViewTheme(c, toDark);
                }
                else if (c is Panel || c is TabControl || c is TabPage)
                {
                    c.BackColor = currentBg;
                    c.ForeColor = currentText;
                }

                if (c.Controls.Count > 0)
                {
                    ApplyFrameRecursive(c.Controls, toDark, progress);
                }
            }
        }

        private static Color InterpolateColor(Color from, Color to, double progress)
        {
            int r = (int)(from.R + (to.R - from.R) * progress);
            int g = (int)(from.G + (to.G - from.G) * progress);
            int b = (int)(from.B + (to.B - from.B) * progress);
            return Color.FromArgb(r, g, b);
        }

        private static void SetGunaPanelColor(Control c, Color backColor)
        {
            var propFill = c.GetType().GetProperty("FillColor");
            if (propFill != null) propFill.SetValue(c, backColor);
            else c.BackColor = backColor;
        }

        private static void SetGunaTextBoxColor(Control c, Color bgColor, Color textColor, Color borderColor)
        {
            var propFill = c.GetType().GetProperty("FillColor");
            var propBase = c.GetType().GetProperty("BaseColor");
            var propText = c.GetType().GetProperty("ForeColor");
            var propBorder = c.GetType().GetProperty("BorderColor");

            if (propFill != null) propFill.SetValue(c, bgColor);
            else if (propBase != null) propBase.SetValue(c, bgColor);

            if (propText != null) propText.SetValue(c, textColor);
            if (propBorder != null) propBorder.SetValue(c, borderColor);
        }

        private static void SetGunaDataGridViewTheme(Control c, bool isDark)
        {
            try
            {
                Color gridBg = isDark ? DarkBg : LightBg;
                Color rowBg = isDark ? Color.FromArgb(44, 44, 46) : Color.White;
                Color altRowBg = isDark ? Color.FromArgb(28, 28, 30) : Color.FromArgb(242, 242, 247);
                Color textColor = isDark ? DarkText : LightText;
                Color headerBg = isDark ? Color.FromArgb(58, 58, 60) : Color.FromArgb(229, 229, 234);

                c.BackColor = gridBg;
                if (c is DataGridView dgvNativo)
                {
                    dgvNativo.BackgroundColor = gridBg;
                    dgvNativo.GridColor = isDark ? DarkBorder : LightBorder;
                }

                dynamic dgv = c;
                dgv.BackgroundColor = gridBg;
                dgv.GridColor = isDark ? DarkBorder : LightBorder;

                dgv.ThemeStyle.RowsStyle.BackColor = rowBg;
                dgv.ThemeStyle.RowsStyle.ForeColor = textColor;
                dgv.ThemeStyle.RowsStyle.SelectionBackColor = AppleAccentBlue;
                dgv.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;

                dgv.ThemeStyle.AlternatingRowsStyle.BackColor = altRowBg;
                dgv.ThemeStyle.AlternatingRowsStyle.ForeColor = textColor;
                dgv.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = AppleAccentBlue;
                dgv.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.White;

                dgv.ThemeStyle.HeaderStyle.BackColor = headerBg;
                dgv.ThemeStyle.HeaderStyle.ForeColor = textColor;

                dgv.Refresh();
            }
            catch { }
        }
    }
}