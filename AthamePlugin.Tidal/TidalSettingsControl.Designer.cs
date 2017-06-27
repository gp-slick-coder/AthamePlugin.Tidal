namespace AthamePlugin.Tidal
{
    partial class TidalSettingsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.qLosslessRadioButton = new System.Windows.Forms.RadioButton();
            this.qHighRadioButton = new System.Windows.Forms.RadioButton();
            this.qLowRadioButton = new System.Windows.Forms.RadioButton();
            this.unlessAlbumVersionCheckBox = new System.Windows.Forms.CheckBox();
            this.appendVerCheckBox = new System.Windows.Forms.CheckBox();
            this.useOfflineUrlEndpointCheckbox = new System.Windows.Forms.CheckBox();
            this.enforceCCCheckBox = new System.Windows.Forms.CheckBox();
            this.countriesComboBox = new System.Windows.Forms.ComboBox();
            this.qMqaRadioButton = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Quality:";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.qMqaRadioButton);
            this.flowLayoutPanel2.Controls.Add(this.qLosslessRadioButton);
            this.flowLayoutPanel2.Controls.Add(this.qHighRadioButton);
            this.flowLayoutPanel2.Controls.Add(this.qLowRadioButton);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(59, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(330, 100);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // qLosslessRadioButton
            // 
            this.qLosslessRadioButton.AutoSize = true;
            this.qLosslessRadioButton.Location = new System.Drawing.Point(3, 28);
            this.qLosslessRadioButton.Name = "qLosslessRadioButton";
            this.qLosslessRadioButton.Size = new System.Drawing.Size(293, 19);
            this.qLosslessRadioButton.TabIndex = 0;
            this.qLosslessRadioButton.TabStop = true;
            this.qLosslessRadioButton.Text = "Lossless (FLAC, only available on HiFi subscription)";
            this.qLosslessRadioButton.UseVisualStyleBackColor = true;
            // 
            // qHighRadioButton
            // 
            this.qHighRadioButton.AutoSize = true;
            this.qHighRadioButton.Location = new System.Drawing.Point(3, 53);
            this.qHighRadioButton.Name = "qHighRadioButton";
            this.qHighRadioButton.Size = new System.Drawing.Size(132, 19);
            this.qHighRadioButton.TabIndex = 1;
            this.qHighRadioButton.TabStop = true;
            this.qHighRadioButton.Text = "High (320kbps AAC)";
            this.qHighRadioButton.UseVisualStyleBackColor = true;
            // 
            // qLowRadioButton
            // 
            this.qLowRadioButton.AutoSize = true;
            this.qLowRadioButton.Location = new System.Drawing.Point(3, 78);
            this.qLowRadioButton.Name = "qLowRadioButton";
            this.qLowRadioButton.Size = new System.Drawing.Size(122, 19);
            this.qLowRadioButton.TabIndex = 2;
            this.qLowRadioButton.TabStop = true;
            this.qLowRadioButton.Text = "Low (96kbps AAC)";
            this.qLowRadioButton.UseVisualStyleBackColor = true;
            // 
            // unlessAlbumVersionCheckBox
            // 
            this.unlessAlbumVersionCheckBox.AutoSize = true;
            this.unlessAlbumVersionCheckBox.Location = new System.Drawing.Point(77, 143);
            this.unlessAlbumVersionCheckBox.Name = "unlessAlbumVersionCheckBox";
            this.unlessAlbumVersionCheckBox.Size = new System.Drawing.Size(217, 19);
            this.unlessAlbumVersionCheckBox.TabIndex = 10;
            this.unlessAlbumVersionCheckBox.Text = "...unless it contains \"Album Version\"";
            this.unlessAlbumVersionCheckBox.UseVisualStyleBackColor = true;
            this.unlessAlbumVersionCheckBox.CheckedChanged += new System.EventHandler(this.unlessAlbumVersionCheckBox_CheckedChanged);
            // 
            // appendVerCheckBox
            // 
            this.appendVerCheckBox.AutoSize = true;
            this.appendVerCheckBox.Location = new System.Drawing.Point(59, 118);
            this.appendVerCheckBox.Name = "appendVerCheckBox";
            this.appendVerCheckBox.Size = new System.Drawing.Size(146, 19);
            this.appendVerCheckBox.TabIndex = 8;
            this.appendVerCheckBox.Text = "Append version to title";
            this.appendVerCheckBox.UseVisualStyleBackColor = true;
            this.appendVerCheckBox.CheckedChanged += new System.EventHandler(this.appendVerCheckBox_CheckedChanged);
            // 
            // useOfflineUrlEndpointCheckbox
            // 
            this.useOfflineUrlEndpointCheckbox.AutoSize = true;
            this.useOfflineUrlEndpointCheckbox.Location = new System.Drawing.Point(59, 184);
            this.useOfflineUrlEndpointCheckbox.Name = "useOfflineUrlEndpointCheckbox";
            this.useOfflineUrlEndpointCheckbox.Size = new System.Drawing.Size(192, 19);
            this.useOfflineUrlEndpointCheckbox.TabIndex = 11;
            this.useOfflineUrlEndpointCheckbox.Text = "Use \'offlineUrl\' (recommended)";
            this.useOfflineUrlEndpointCheckbox.UseVisualStyleBackColor = true;
            this.useOfflineUrlEndpointCheckbox.CheckedChanged += new System.EventHandler(this.useOfflineUrlEndpointCheckbox_CheckedChanged);
            // 
            // enforceCCCheckBox
            // 
            this.enforceCCCheckBox.AutoSize = true;
            this.enforceCCCheckBox.Location = new System.Drawing.Point(59, 222);
            this.enforceCCCheckBox.Name = "enforceCCCheckBox";
            this.enforceCCCheckBox.Size = new System.Drawing.Size(142, 19);
            this.enforceCCCheckBox.TabIndex = 12;
            this.enforceCCCheckBox.Text = "Enforce country code:";
            this.enforceCCCheckBox.UseVisualStyleBackColor = true;
            this.enforceCCCheckBox.Visible = false;
            // 
            // countriesComboBox
            // 
            this.countriesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.countriesComboBox.FormattingEnabled = true;
            this.countriesComboBox.Location = new System.Drawing.Point(207, 220);
            this.countriesComboBox.Name = "countriesComboBox";
            this.countriesComboBox.Size = new System.Drawing.Size(268, 23);
            this.countriesComboBox.TabIndex = 13;
            this.countriesComboBox.Visible = false;
            // 
            // qMqaRadioButton
            // 
            this.qMqaRadioButton.AutoSize = true;
            this.qMqaRadioButton.Location = new System.Drawing.Point(3, 3);
            this.qMqaRadioButton.Name = "qMqaRadioButton";
            this.qMqaRadioButton.Size = new System.Drawing.Size(324, 19);
            this.qMqaRadioButton.TabIndex = 3;
            this.qMqaRadioButton.TabStop = true;
            this.qMqaRadioButton.Text = "Master (FLAC, 24-bit, only available on HiFi subscription)";
            this.qMqaRadioButton.UseVisualStyleBackColor = true;
            // 
            // TidalSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.countriesComboBox);
            this.Controls.Add(this.enforceCCCheckBox);
            this.Controls.Add(this.useOfflineUrlEndpointCheckbox);
            this.Controls.Add(this.unlessAlbumVersionCheckBox);
            this.Controls.Add(this.appendVerCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TidalSettingsControl";
            this.Size = new System.Drawing.Size(576, 313);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RadioButton qLosslessRadioButton;
        private System.Windows.Forms.RadioButton qHighRadioButton;
        private System.Windows.Forms.RadioButton qLowRadioButton;
        private System.Windows.Forms.CheckBox unlessAlbumVersionCheckBox;
        private System.Windows.Forms.CheckBox appendVerCheckBox;
        private System.Windows.Forms.CheckBox useOfflineUrlEndpointCheckbox;
        private System.Windows.Forms.CheckBox enforceCCCheckBox;
        private System.Windows.Forms.ComboBox countriesComboBox;
        private System.Windows.Forms.RadioButton qMqaRadioButton;
    }
}
