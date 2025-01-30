using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using Newtonsoft.Json;

namespace settings_window_with_apply
{
    public partial class MainForm : Form
    {
        public static AppSettings CurrentSettings { get; private set; } = new AppSettings();

        // In this particular case, we're serializing to a json file.
        public string PathToSettings { get; } =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "StackOverflow",
                "SettingsDemo",
                "config.json");

        public MainForm()
        {
            if (File.Exists(PathToSettings))
            {
                CurrentSettings =
                    JsonConvert
                    .DeserializeObject<AppSettings>(File.ReadAllText(PathToSettings));
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(PathToSettings));
                File.WriteAllText(PathToSettings, JsonConvert.SerializeObject(CurrentSettings));
            }
            InitializeComponent();
            toolStrip.DataBindings.Add(
                nameof(toolStrip.BackColor),
                CurrentSettings,
                nameof(CurrentSettings.ToolStripBackColor),
                false,
                DataSourceUpdateMode.OnPropertyChanged);
            this.settingsButton.Click += SettingsButton_Click;
        }

        private void SettingsButton_Click(object? sender, EventArgs e)
        {
            using(var settingsUI = new SettingsWindow())
            {
                if(DialogResult.OK == settingsUI.ShowDialog(this, CurrentSettings))
                {
                    settingsUI.EditedSettings.CopyValues(CurrentSettings);
                    File.WriteAllText(PathToSettings, JsonConvert.SerializeObject(CurrentSettings));
                }
                else
                {   /* G T K */
                    // User has decided not to change the values after all.
                }
            }
        }
    }
    [Flags]
    public enum ColumnVisibility
    {
        None   = 0x00,
        Folder = 0x01,
        Global = 0x02,
    }

    public class AppSettings : INotifyPropertyChanged
    {
        public void CopyValues(AppSettings target)
        {
            foreach (var property in typeof(AppSettings).GetProperties()) 
            {
                property.SetValue(target, property.GetValue(this));
            }
        }


        [Editor(typeof(ExePathEditor), typeof(UITypeEditor))]
        public string ExePath { get; set; } = string.Empty;

        public bool OptSaveUseBrowser { get; set; } = false;

        // Bindable properties
        public Color ToolStripBackColor
        {
            get => _toolStripBackColor;
            set
            {
                if (!Equals(_toolStripBackColor, value))
                {
                    _toolStripBackColor = value;
                    OnPropertyChanged();
                }
            }
        }
        Color _toolStripBackColor = Color.LightBlue;
        public ColumnVisibility ColumnVisibility
        {
            get => _columnVisibility;
            set
            {
                if (!Equals(_columnVisibility, value))
                {
                    _columnVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        ColumnVisibility _columnVisibility = default;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
