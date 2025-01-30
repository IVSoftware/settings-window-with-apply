using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace settings_window_with_apply
{
    public partial class SettingsWindow : Form
    {
        public AppSettings EditedSettings { get; } = new AppSettings();
        public SettingsWindow()
        {
            InitializeComponent();
            buttonApply.Click += (sender, e) =>DialogResult = DialogResult.OK;
            buttonCancel.Click += (sender, e) =>DialogResult = DialogResult.Cancel;
            propertyGrid.SelectedObject = EditedSettings;
        }
        public DialogResult ShowDialog(IWin32Window owner, AppSettings currentSettings)
        {
            currentSettings.CopyValues(EditedSettings);
            return base.ShowDialog(owner);
        }
    }
}
