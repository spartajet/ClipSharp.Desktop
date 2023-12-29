using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ClipSharp.Desktop.ViewModels;
using ClipSharp.Desktop.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClipSharp.Desktop;

public partial class App : Application
{
    public static string ClipSharpFolder { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClipSharp");

    public static string ConfigPath { get; } = Path.Combine(ClipSharpFolder, "config.json");

    // public static string DataFolder { get; } = Path.Combine(ClipSharpFolder, "Data");
    public static string DataBasePath { get; } = Path.Combine(ClipSharpFolder, "ClipSharp.db");
    public static string ImageFolder { get; } = Path.Combine(ClipSharpFolder, "Images");
    public static string LogFolder { get; } = Path.Combine(ClipSharpFolder, "Logs");
    
    private static readonly IHost Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                                                  .ConfigureAppConfiguration(c =>
                                                  {
                                                      c.SetBasePath(AppContext.BaseDirectory);
                                                  })
                                                  .ConfigureServices(
                                                      (_, services) =>
                                                      {
                                                          // App Host
                                                          services.AddHostedService<ApplicationHostService>();

                                                     
                                                      }
                                                  )
                                                  .Build();

    public override void Initialize()
    {
        Assets.Language.Resources.Culture = new("zh-Hans");
        InitialFolders();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        // {
        //     desktop.MainWindow = new MainWindow
        //     {
        //         DataContext = new MainWindowViewModel(),
        //     };
        // }
        Host.StartAsync();
        base.OnFrameworkInitializationCompleted();
    }

    private void ExitMenuItem_OnClick(object? sender, EventArgs e)
    {
        Exit();
    }

    public static void Exit()
    {
        Host.StopAsync().Wait();
        Host.Dispose();
    }

    private static void InitialFolders()
    {
        if (!Directory.Exists(ClipSharpFolder))
        {
            Directory.CreateDirectory(ClipSharpFolder);
        }

        if (!Directory.Exists(ImageFolder))
        {
            Directory.CreateDirectory(ImageFolder);
        }

        if (!Directory.Exists(LogFolder))
        {
            Directory.CreateDirectory(LogFolder);
        }
    }
}