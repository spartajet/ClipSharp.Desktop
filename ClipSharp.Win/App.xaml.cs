using System.Configuration;
using System.Data;
using System.Windows;
using H.NotifyIcon;
using H.NotifyIcon.EfficiencyMode;

namespace ClipSharp.Win;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon _taskbar;


    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        
    }

    /// <inheritdoc />
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        _taskbar = (TaskbarIcon) FindResource("TaskbarIcon");
        // EfficiencyModeUtilities.SetEfficiencyMode(true);
        _taskbar.ForceCreate();
        // WindowExtensions.Hide(this Window window, enableEfficiencyMode: true) // default value
        // WindowExtensions.Show(this Window window, disableEfficiencyMode: true) // default value
        // TaskbarIcon.ForceCreate(bool enablesEfficiencyMode = true); // default value
    }
}