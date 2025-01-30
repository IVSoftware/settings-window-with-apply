In your comment, you stated:

> I'm not the original author

And looking at that copied code, we ask: What were they thinking? So, seriously, there are two idiomatic concepts that they might have been trying to implement.

___

- **First:** The misuse of a static class might have come from an objective of making the settings accessible globally from anywhere in the app. If this were the case, however, it's far better to make a static _instance_ of a non-static class.

~~~
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
}
~~~

- **Second:** It's quite common to provide a chance to bail out when editing properties. So, suppose we have a `PropertyGrid` set up for our `AppSettings` class. Since the options here are to [Apply] _or_ [Cancel], we need to work with a copy of the settings, not the setting themselves. And this, I believe, is responsible for the tortured manner in which this copying was done in the original code. 

Here's the thing, the `PropertyGrid` is going to use reflection to populate itself, and we could do the same thing to make the working copy. 

~~~
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
    public string ExePath { get; set; } = string.Empty;

    public bool OptSaveUseBrowser { get; set; } = false;

    // Bindable property
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

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    public event PropertyChangedEventHandler? PropertyChanged;
}
~~~


