   In your comment, you stated:

> I'm not the original author

And looking at that copied code, we might ask: *What were they thinking?* 

I mean that literally. It is _plausible, fair and reasonable_ to posit that they might have had two idiomatic goals that are common in this scenario:

___

- **First:** The misuse of a static class might have come from an objective of *making the settings accessible globally from anywhere in the app*. If this were the case, however, it's far better to make a static _instance_ of a non-static class.

~~~
public partial class MainForm : Form
{
    // The AppSettings class is 'not' static, but the instance is.
    public static AppSettings CurrentSettings { get; private set; } =
        new AppSettings();

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

 - **Second:** It's quite common to provide a chance to bail out when editing properties. So, suppose we have a `PropertyGrid` set up for our `AppSettings` class. Since the options here are to **[Apply]** _or_ **[Cancel]**, we need to work with a _copy_ of the settings (not the settings themselves). *I believe that this need for a 'revert' is responsible for the tortured manner that the copying was done in the original code.*

[![property grid][2]][2]

###### So, here's the thing: 

The `PropertyGrid` already uses reflection to populate itself, and we could employ the same strategy to make the copy. Here, we have added a `CopyValues()` method that reflect the `public` properties. 

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
    .
    .
    .
    // Property declarations...
}
~~~
___

**Minimal Example**

Here, the `SettingsWindow` is a simple `Form` containing a `PropertyGrid`.

[![property edit flow][3]][3]

~~~
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
~~~
____
###### Call from MainForm Gear Icon

Call its custom `ShowDialog` overload, passing the `CurrentAppSettings` and updating them _only_ if the method returns `DialogResult.OK`.

~~~
public partial class MainForm : Form
{
    .
    .
    .

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
~~~


  [2]: https://i.sstatic.net/rUIuQuFk.png
  [3]: https://i.sstatic.net/DaGYlxy4.png