using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.DesignerSupport.Remote.HtmlTransport;
using Avalonia.Markup.Xaml;
using ClipSharp.Desktop.ViewModels;
using ClipSharp.Desktop.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using Microsoft.Extensions.Logging;
using LogLevel=NLog.LogLevel;



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
                                                      (host, services) =>
                                                      {
                                                          // App Host
                                                          
                                                          services.AddHostedService<ApplicationHostService>();
                                                          services.AddLogging(loggingBuilder =>
                                                          {
                                                              loggingBuilder.ClearProviders();
                                                              loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                                                              loggingBuilder.AddNLog();
                                                          });
                                                      }
                                                  )
                                                  .Build();

    public override void Initialize()
    {
        Assets.Language.Resources.Culture = new("zh-Hans");
        InitialFolders();
        NlogConfig();
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

    private static void NlogConfig()
    {
        LoggingConfiguration config = new();

        // Targets where to log to: File and Console
        FileTarget logFile = new("logFile")
        {
            FileName = Path.Combine(LogFolder, "Debug.log"),
            ArchiveEvery = FileArchivePeriod.Day,
            ArchiveNumbering = ArchiveNumberingMode.Rolling,
            ArchiveFileName =Path.Combine(LogFolder, "Debug.{#}.log"),
            ArchiveDateFormat = "yyyyMMdd",
            MaxArchiveFiles = 7,
            AutoFlush = true
        };
        FileTarget errorFile = new("errorFile")
        {
            FileName = Path.Combine(LogFolder, "Error.log"),
            ArchiveEvery = FileArchivePeriod.Day,
            ArchiveNumbering = ArchiveNumberingMode.Rolling,
            ArchiveFileName = Path.Combine(LogFolder, "Error.{#}.log"),
            ArchiveDateFormat = "yyyyMMdd",
            MaxArchiveFiles = 7,
            AutoFlush = true
        };

        ConsoleTarget logConsole = new("logConsole");

        // Rules for mapping loggers to targets            
        config.AddRule(LogLevel.Info, LogLevel.Fatal, logConsole);
        config.AddRule(LogLevel.Debug, LogLevel.Info, logFile);
        config.AddRule(LogLevel.Warn, LogLevel.Fatal, errorFile);
        // Apply config           
        LogManager.Configuration = config;
    }
}