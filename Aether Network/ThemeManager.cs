using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace aether
{
    // --- GERENCIADOR DE CONFIGURAÇÕES (JSON) ---
    public static class SettingsManager
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "metadados\\preferences.json");

        public class UserPreferences
        {
            public string Theme { get; set; } = "Light";
        }

        public static ThemeManager.ThemeMode LoadThemePreference()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    SaveThemePreference(ThemeManager.ThemeMode.Light);
                    return ThemeManager.ThemeMode.Light;
                }

                string json = File.ReadAllText(FilePath);

                // Parse manual simples para evitar dependências externas pesadas (como Newtonsoft.Json)
                // Se você já usa Newtonsoft ou System.Text.Json no projeto, pode substituir essa linha.
                if (json.Contains("\"Theme\": \"Dark\"") || json.Contains("\"Theme\":\"Dark\""))
                {
                    return ThemeManager.ThemeMode.Dark;
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
                // Estrutura o JSON manualmente de forma limpa
                string json = $"{{\n  \"Theme\": \"{theme}\"\n}}";
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar preferências: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    // --- GERENCIADOR DE TEMAS ATUALIZADO ---
    public static class ThemeManager
    {
        public enum ThemeMode { Light, Dark }
        public static ThemeMode CurrentTheme { get; private set; } = ThemeMode.Light;

        // --- PALETA DE CORES (AETHER NETWORK) ---
        private static readonly Color DarkBg = Color.FromArgb(20, 26, 38);
        private static readonly Color DarkSide = Color.FromArgb(30, 41, 59);
        private static readonly Color DarkInput = Color.FromArgb(15, 23, 42);
        private static readonly Color DarkText = Color.FromArgb(241, 245, 249);
        private static readonly Color DarkBorder = Color.FromArgb(71, 85, 105);

        private static readonly Color LightBg = Color.FromArgb(245, 247, 250);
        private static readonly Color LightSide = Color.FromArgb(255, 255, 255);
        private static readonly Color LightInput = Color.FromArgb(255, 255, 255);
        private static readonly Color LightText = Color.FromArgb(30, 30, 30);
        private static readonly Color LightBorder = Color.FromArgb(209, 213, 219);

        // Variáveis de controle da animação
        private static System.Windows.Forms.Timer transitionTimer;
        private static double transitionProgress = 0.0;
        private static bool isTargetDark = false;
        private static Form activeForm;

        /// <summary>
        /// Inicializa o tema padrão do sistema baseado no arquivo JSON sem rodar a animação.
        /// Chame isso no evento Load do seu Form principal.
        /// </summary>
        public static void InitializeTheme(Form form)
        {
            CurrentTheme = SettingsManager.LoadThemePreference();
            isTargetDark = (CurrentTheme == ThemeMode.Dark);

            // Renderiza direto no frame final (progresso 1.0) para evitar lentidão ou flash na inicialização
            RenderTransitionFrame(form, isTargetDark, 1.0);
        }

        // --- MÉTODO PRINCIPAL COM ANIMAÇÃO REAL ---
        public static void ApplyThemeWithAnimation(Form form, ThemeMode theme)
        {
            // Salva a nova preferência no arquivo JSON imediatamente ao mudar
            SettingsManager.SaveThemePreference(theme);

            if (transitionTimer != null)
            {
                transitionTimer.Stop();
                transitionTimer.Dispose();
            }

            CurrentTheme = theme;
            isTargetDark = (theme == ThemeMode.Dark);
            activeForm = form;
            transitionProgress = 0.0;

            transitionTimer = new System.Windows.Forms.Timer();
            transitionTimer.Interval = 15; // Velocidade da atualização dos quadros (60 FPS aprox.)
            transitionTimer.Tick += TransitionTimer_Tick;
            transitionTimer.Start();
        }

        private static void TransitionTimer_Tick(object sender, EventArgs e)
        {
            transitionProgress += 0.08;

            if (transitionProgress >= 1.0)
            {
                transitionProgress = 1.0;
                transitionTimer.Stop();
                transitionTimer.Dispose();
                transitionTimer = null;
            }

            RenderTransitionFrame(activeForm, isTargetDark, transitionProgress);
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
            Color currentSide = InterpolateColor(toDark ? LightSide : DarkSide, toDark ? DarkSide : LightSide, progress);
            Color currentInput = InterpolateColor(toDark ? LightInput : DarkInput, toDark ? DarkInput : LightInput, progress);
            Color currentText = InterpolateColor(toDark ? LightText : DarkText, toDark ? DarkText : LightText, progress);
            Color currentBorder = InterpolateColor(toDark ? LightBorder : DarkBorder, toDark ? DarkBorder : LightBorder, progress);

            foreach (Control c in controls)
            {
                string name = c.Name;

                if (name == "sidePanel")
                {
                    SetGunaPanelColor(c, currentSide);
                }
                else if (name == "lstPromptPing" && c is ListBox listBox)
                {
                    listBox.BackColor = currentInput;
                    listBox.ForeColor = currentText;
                }
               
                else if (name == "label6" || name == "lblHorario" || name == "label7" || name == "label1" || name == "label8" ||
                         name == "lblRede" || name == "lblTotal" || name == "lblVersion" || name == "lblAl1")
                {
                    c.ForeColor = currentText;
                    if (c is Label lbl) lbl.BackColor = Color.Transparent;
                }
                else if ((name == "btnBackup" || name == "btnImportBackup" ||
                          name == "btnAetherAI" || name == "btnAdicionarIP") && c is Button btn)
                {
                    btn.BackColor = currentSide;
                    btn.ForeColor = currentText;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = toDark ? 0 : 1;
                    btn.FlatAppearance.BorderColor = currentBorder;
                }
                else if (name == "txtIdentificador" || name == "txtPesquisa")
                {
                    SetGunaTextBoxColor(c, currentInput, currentText, currentBorder);
                }
                else if (name == "cbIPs" && c is ComboBox cb)
                {
                    cb.BackColor = currentInput;
                    cb.ForeColor = currentText;
                }
                else if (name == "checkBoxMultiline" && c is CheckBox chk)
                {
                    chk.ForeColor = currentText;
                    chk.BackColor = Color.Transparent;
                }
                else if (name == "dgvLogs")
                {
                    if (progress >= 0.8) SetGunaDataGridViewTheme(c, toDark);
                }
                else if (c is Panel || c is TabControl || c is TabPage)
                {
                    c.BackColor = InterpolateColor(toDark ? LightBg : DarkBg, toDark ? DarkBg : LightBg, progress);
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
                Color rowBg = isDark ? Color.FromArgb(26, 36, 51) : Color.White;
                Color altRowBg = isDark ? Color.FromArgb(30, 41, 59) : Color.FromArgb(245, 247, 250);
                Color textColor = isDark ? DarkText : LightText;
                Color headerBg = isDark ? Color.FromArgb(15, 23, 42) : Color.FromArgb(230, 235, 240);

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
                dgv.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
                dgv.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;

                dgv.ThemeStyle.AlternatingRowsStyle.BackColor = altRowBg;
                dgv.ThemeStyle.AlternatingRowsStyle.ForeColor = textColor;
                dgv.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
                dgv.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.White;

                dgv.ThemeStyle.HeaderStyle.BackColor = headerBg;
                dgv.ThemeStyle.HeaderStyle.ForeColor = textColor;

                dgv.Refresh();
            }
            catch { }
        }
    }
}