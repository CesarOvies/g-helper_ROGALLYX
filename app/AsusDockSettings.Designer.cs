using GHelper.UI;

namespace GHelper
{
    partial class AsusDockSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelLighting = new Panel();
            labelLighting = new Label();
            tableLighting = new TableLayoutPanel();
            comboBoxLightingMode = new RComboBox();
            buttonColor = new RColorButton();
            buttonBrightness = new RButton();
            checkBoxSyncAura = new CheckBox();
            panelLighting.SuspendLayout();
            tableLighting.SuspendLayout();
            SuspendLayout();
            // 
            // panelLighting
            // 
            panelLighting.AutoSize = true;
            panelLighting.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelLighting.Controls.Add(checkBoxSyncAura);
            panelLighting.Controls.Add(tableLighting);
            panelLighting.Controls.Add(labelLighting);
            panelLighting.Dock = DockStyle.Top;
            panelLighting.Location = new Point(20, 20);
            panelLighting.Margin = new Padding(0);
            panelLighting.Name = "panelLighting";
            panelLighting.Padding = new Padding(10);
            panelLighting.Size = new Size(560, 180);
            panelLighting.TabIndex = 0;
            // 
            // labelLighting
            // 
            labelLighting.AutoSize = true;
            labelLighting.Dock = DockStyle.Top;
            labelLighting.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelLighting.Location = new Point(10, 10);
            labelLighting.Margin = new Padding(0);
            labelLighting.Name = "labelLighting";
            labelLighting.Padding = new Padding(0, 0, 0, 10);
            labelLighting.Size = new Size(82, 35);
            labelLighting.TabIndex = 0;
            labelLighting.Text = "Lighting";
            // 
            // tableLighting
            // 
            tableLighting.AutoSize = true;
            tableLighting.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLighting.ColumnCount = 3;
            tableLighting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLighting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLighting.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLighting.Controls.Add(comboBoxLightingMode, 0, 0);
            tableLighting.Controls.Add(buttonColor, 1, 0);
            tableLighting.Controls.Add(buttonBrightness, 2, 0);
            tableLighting.Dock = DockStyle.Top;
            tableLighting.Location = new Point(10, 45);
            tableLighting.Margin = new Padding(0);
            tableLighting.Name = "tableLighting";
            tableLighting.RowCount = 1;
            tableLighting.RowStyles.Add(new RowStyle());
            tableLighting.Size = new Size(540, 48);
            tableLighting.TabIndex = 1;
            // 
            // comboBoxLightingMode
            // 
            comboBoxLightingMode.BorderColor = Color.White;
            comboBoxLightingMode.ButtonColor = Color.FromArgb(255, 255, 255);
            comboBoxLightingMode.Dock = DockStyle.Fill;
            comboBoxLightingMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxLightingMode.FlatStyle = FlatStyle.Flat;
            comboBoxLightingMode.Font = new Font("Segoe UI", 9F);
            comboBoxLightingMode.FormattingEnabled = true;
            comboBoxLightingMode.Location = new Point(4, 4);
            comboBoxLightingMode.Margin = new Padding(4);
            comboBoxLightingMode.Name = "comboBoxLightingMode";
            comboBoxLightingMode.Size = new Size(208, 40);
            comboBoxLightingMode.TabIndex = 0;
            // 
            // buttonColor
            // 
            buttonColor.Activated = false;
            buttonColor.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            buttonColor.BackColor = SystemColors.ButtonHighlight;
            buttonColor.BorderColor = Color.Transparent;
            buttonColor.BorderRadius = 2;
            buttonColor.Dock = DockStyle.Fill;
            buttonColor.FlatStyle = FlatStyle.Flat;
            buttonColor.ForeColor = SystemColors.ControlText;
            buttonColor.Location = new Point(220, 4);
            buttonColor.Margin = new Padding(4);
            buttonColor.Name = "buttonColor";
            buttonColor.Secondary = false;
            buttonColor.Size = new Size(154, 40);
            buttonColor.TabIndex = 1;
            buttonColor.Text = "Color";
            buttonColor.UseVisualStyleBackColor = false;
            // 
            // buttonBrightness
            // 
            buttonBrightness.Activated = false;
            buttonBrightness.BackColor = SystemColors.ControlLightLight;
            buttonBrightness.BorderColor = Color.Transparent;
            buttonBrightness.BorderRadius = 4;
            buttonBrightness.Dock = DockStyle.Fill;
            buttonBrightness.FlatAppearance.BorderSize = 0;
            buttonBrightness.FlatStyle = FlatStyle.Flat;
            buttonBrightness.ForeColor = SystemColors.ControlText;
            buttonBrightness.Image = Properties.Resources.backlight;
            buttonBrightness.Location = new Point(382, 4);
            buttonBrightness.Margin = new Padding(4);
            buttonBrightness.Name = "buttonBrightness";
            buttonBrightness.Secondary = false;
            buttonBrightness.Size = new Size(154, 40);
            buttonBrightness.TabIndex = 2;
            buttonBrightness.Text = "100%";
            buttonBrightness.TextImageRelation = TextImageRelation.ImageBeforeText;
            buttonBrightness.UseVisualStyleBackColor = false;
            // 
            // checkBoxSyncAura
            // 
            checkBoxSyncAura.AutoSize = true;
            checkBoxSyncAura.Dock = DockStyle.Top;
            checkBoxSyncAura.Location = new Point(10, 93);
            checkBoxSyncAura.Margin = new Padding(4);
            checkBoxSyncAura.Name = "checkBoxSyncAura";
            checkBoxSyncAura.Padding = new Padding(4, 12, 4, 4);
            checkBoxSyncAura.Size = new Size(540, 45);
            checkBoxSyncAura.TabIndex = 2;
            checkBoxSyncAura.Text = "Sync with Aura";
            checkBoxSyncAura.UseVisualStyleBackColor = true;
            // 
            // AsusDockSettings
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(600, 260);
            MinimumSize = new Size(460, 200);
            Controls.Add(panelLighting);
            FormBorderStyle = FormBorderStyle.Sizable;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AsusDockSettings";
            Padding = new Padding(20);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ROG Bulwark Dock";
            panelLighting.ResumeLayout(false);
            panelLighting.PerformLayout();
            tableLighting.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelLighting;
        private Label labelLighting;
        private TableLayoutPanel tableLighting;
        private RComboBox comboBoxLightingMode;
        private RColorButton buttonColor;
        private RButton buttonBrightness;
        private CheckBox checkBoxSyncAura;
    }
}
