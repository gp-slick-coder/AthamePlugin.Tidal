using System;
using System.Windows.Forms;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal
{
    internal partial class TidalSettingsControl : UserControl
    {
        public TidalSettingsControl()
        {
            InitializeComponent();
        }

        private readonly TidalServiceSettings settings;

        public TidalSettingsControl(TidalServiceSettings settings)
        {
            InitializeComponent();
            this.settings = settings;
            var rbem = new RadioButtonEnumMapper();

            rbem.Assign(qMqaRadioButton, (int)StreamingQuality.HiRes);
            rbem.Assign(qLosslessRadioButton, (int)StreamingQuality.Lossless);
            rbem.Assign(qHighRadioButton, (int)StreamingQuality.High);
            rbem.Assign(qLowRadioButton, (int)StreamingQuality.Low);

            rbem.Select((int)settings.StreamQuality);
            appendVerCheckBox.Checked = settings.AppendVersionToTrackTitle;
            unlessAlbumVersionCheckBox.Enabled = appendVerCheckBox.Checked;
            unlessAlbumVersionCheckBox.Checked = settings.DontAppendAlbumVersion;
            useOfflineUrlEndpointCheckbox.Checked = settings.UseOfflineUrl;

            rbem.ValueChanged += (sender, args) => settings.StreamQuality = (StreamingQuality)rbem.Value;
            countriesComboBox.Items.AddRange(Country.AllCountries);
            countriesComboBox.SelectedIndex = 0;
        }

        private void appendVerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            settings.AppendVersionToTrackTitle = appendVerCheckBox.Checked;
            unlessAlbumVersionCheckBox.Enabled = appendVerCheckBox.Checked;
        }

        private void unlessAlbumVersionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            settings.DontAppendAlbumVersion = unlessAlbumVersionCheckBox.Checked;
        }

        private void useOfflineUrlEndpointCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseOfflineUrl = useOfflineUrlEndpointCheckbox.Checked;
        }
    }
}
