using GHelper.Input;
using GHelper.Peripherals.Dock;
using GHelper.UI;
using GHelper.USB;

namespace GHelper
{
    public partial class AsusDockSettings : RForm
    {
        private readonly AsusDock dock;
        private bool loading = true;

        public AsusDockSettings(AsusDock dock)
        {
            this.dock = dock;
            InitializeComponent();
            InitTheme(true);

            Text = dock.GetDisplayName();
            labelLighting.Text = Properties.Strings.Lighting;
            buttonColor.Text = Properties.Strings.Color;
            checkBoxSyncAura.Text = (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "pt")
                ? "Sincronizar com Aura"
                : "Sync with Aura";

            comboBoxLightingMode.DataSource = new BindingSource(Aura.GetBulwarkModes(), null);
            comboBoxLightingMode.DisplayMember = "Value";
            comboBoxLightingMode.ValueMember = "Key";

            comboBoxLightingMode.SelectedValueChanged += ComboBoxLightingMode_SelectedValueChanged;
            buttonColor.Click += ButtonColor_Click;
            buttonColor.Swatch2Click += ButtonColor2_Click;
            buttonBrightness.Click += ButtonBrightness_Click;
            checkBoxSyncAura.CheckedChanged += CheckBoxSyncAura_CheckedChanged;

            loading = false;
            Visualise();
        }

        public void Visualise()
        {
            if (InvokeRequired)
            {
                Invoke(Visualise);
                return;
            }

            loading = true;

            bool isSynced = AppConfig.IsDockAuraSync();
            checkBoxSyncAura.Checked = isSynced;

            int brightness = isSynced ? Aura.GetBrightness() : AppConfig.Get("dock_brightness", 3);
            buttonBrightness.Text = Math.Round((double)brightness * 33.33).ToString() + "%";

            if (isSynced)
            {
                comboBoxLightingMode.Enabled = false;
                buttonColor.Enabled = false;
                buttonBrightness.Enabled = false;
                comboBoxLightingMode.SelectedValue = Aura.Mode;
                buttonColor.SwatchColor = Aura.Color1;
                buttonColor.SwatchColor2 = Aura.HasSecondColor() ? Aura.Color2 : (Color?)null;
            }
            else
            {
                comboBoxLightingMode.Enabled = true;
                buttonColor.Enabled = true;
                buttonBrightness.Enabled = true;
                AuraMode mode = (AuraMode)AppConfig.Get("dock_mode", (int)AuraMode.AuraRainbow);
                comboBoxLightingMode.SelectedValue = mode;
                buttonColor.SwatchColor = Color.FromArgb(AppConfig.Get("dock_color", Color.Red.ToArgb()));
                buttonColor.SwatchColor2 = (mode == AuraMode.AuraBreathe)
                    ? Color.FromArgb(AppConfig.Get("dock_color2", Color.Blue.ToArgb()))
                    : (Color?)null;
            }

            loading = false;
        }

        private void CheckBoxSyncAura_CheckedChanged(object? sender, EventArgs e)
        {
            if (loading) return;
            bool isSynced = checkBoxSyncAura.Checked;
            AppConfig.Set("dock_aura_sync", isSynced ? 1 : 0);
            if (isSynced)
            {
                AppConfig.Set("dock_brightness", Aura.GetBrightness());
            }
            ApplyLighting();
            Visualise();
        }

        private void ComboBoxLightingMode_SelectedValueChanged(object? sender, EventArgs e)
        {
            if (loading || comboBoxLightingMode.SelectedValue == null) return;
            if (AppConfig.IsDockAuraSync()) return;
            AppConfig.Set("dock_mode", (int)comboBoxLightingMode.SelectedValue);
            ApplyLighting();
            Visualise();
        }

        private void ButtonColor_Click(object? sender, EventArgs e)
        {
            if (AppConfig.IsDockAuraSync()) return;
            Color initial = Color.FromArgb(AppConfig.Get("dock_color", Color.Red.ToArgb()));
            RColorPicker colorDlg = new RColorPicker(initial, false);
            colorDlg.ColorChanged += c =>
            {
                AppConfig.Set("dock_color", c.ToArgb());
                ApplyLighting();
                Visualise();
            };
            colorDlg.ShowDialog(this);
        }

        private void ButtonColor2_Click(object? sender, EventArgs e)
        {
            if (AppConfig.IsDockAuraSync()) return;
            Color initial = Color.FromArgb(AppConfig.Get("dock_color2", Color.Blue.ToArgb()));
            RColorPicker colorDlg = new RColorPicker(initial, false);
            colorDlg.ColorChanged += c =>
            {
                AppConfig.Set("dock_color2", c.ToArgb());
                ApplyLighting();
                Visualise();
            };
            colorDlg.ShowDialog(this);
        }

        private void ButtonBrightness_Click(object? sender, EventArgs e)
        {
            if (AppConfig.IsDockAuraSync()) return;
            int bl = (AppConfig.Get("dock_brightness", 3) + 1) % 4;
            AppConfig.Set("dock_brightness", bl);
            ApplyLighting();
            Visualise();
        }

        private void ApplyLighting()
        {
            int speed = (Aura.Speed == AuraSpeed.Normal) ? 0xeb : (Aura.Speed == AuraSpeed.Fast) ? 0xf5 : 0xe1;
            if (AppConfig.IsDockAuraSync())
            {
                Aura.ApplyBulwark(Aura.Mode, Aura.Color1, Aura.Color2, speed, Aura.GetBrightness(), force: true);
            }
            else
            {
                AuraMode mode = (AuraMode)AppConfig.Get("dock_mode", (int)AuraMode.AuraRainbow);
                Color c1 = Color.FromArgb(AppConfig.Get("dock_color", Color.Red.ToArgb()));
                Color c2 = Color.FromArgb(AppConfig.Get("dock_color2", Color.Blue.ToArgb()));
                int bl = AppConfig.Get("dock_brightness", 3);
                Aura.ApplyBulwark(mode, c1, c2, speed, bl, force: true);
            }
        }
    }
}
