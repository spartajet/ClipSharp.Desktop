using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ClipSharp.Desktop.ViewModels;
using ClipSharp.Desktop.Views;

namespace ClipSharp.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        Assets.Language.Resources.Culture = new CultureInfo("en-US");
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        
        if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void NativeMenuItem_OnClick(object? sender, EventArgs e)
    {
        Environment.Exit(0);
    }
}