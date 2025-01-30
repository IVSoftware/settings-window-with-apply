using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

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
    public class ExePathEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService editorService)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";
                    openFileDialog.Title = "Select an Executable File";

                    if (value is string currentPath && !string.IsNullOrWhiteSpace(currentPath))
                    {
                        openFileDialog.FileName = currentPath; // Set previous value as default
                    }

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        return openFileDialog.FileName; // Return the selected file path
                    }
                }
            }
            return value;
        }
    }
}
