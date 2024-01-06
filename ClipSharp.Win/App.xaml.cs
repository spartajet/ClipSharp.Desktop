using System.Configuration;
using System.IO;
using System.Windows;
using ClipSharp.Win.Clip;
using ClipSharp.Win.Database;
using ClipSharp.Win.Service;
using H.NotifyIcon;
using H.NotifyIcon.EfficiencyMode;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using SqlSugar;
using Application = System.Windows.Application;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ClipSharp.Win;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon? _taskbar;
    public static string ClipSharpFolder { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ClipSharp");

    public static string ConfigPath { get; } = Path.Combine(ClipSharpFolder, "config.json");

    // public static string DataFolder { get; } = Path.Combine(ClipSharpFolder, "Data");
    public static string DataBasePath { get; } = Path.Combine(ClipSharpFolder, "ClipSharp.db");
    public static string ImageFolder { get; } = Path.Combine(ClipSharpFolder, "Images");
    public static string LogFolder { get; } = Path.Combine(ClipSharpFolder, "Logs");
    
    private static readonly IHost Host =
        Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                 .ConfigureAppConfiguration(c => { c.SetBasePath(AppContext.BaseDirectory); })
                 .ConfigureServices(
                     (host, services) =>
                     {
                         services.AddHostedService<ClipboardService>();
                         services.AddHostedService<HotKeyService>();
                         services.AddHostedService<DatabaseService>();
                         // services.AddTransient<Views.DisplayWindow>();
                         // services.AddSingleton<DisplayWindowViewModel>();
                         services.AddTransient<MainWindow>();
                         // services.AddSingleton<MainWindowViewModel>();
                         // services.AddSingleton<Views.MainView>();
                         // services.AddSingleton<MainViewViewModel>();
                         services.AddSingleton<HookWindows>();
                         services.AddSingleton<ISqlSugarClient>(s =>
                         {
                             SqlSugarScope sqlSugar =
                                 new(new ConnectionConfig()
                                     {
                                         DbType = DbType.Sqlite,
                                         ConnectionString = $"DataSource={DataBasePath}",
                                         IsAutoCloseConnection = true,
                                         InitKeyType = InitKeyType.Attribute,
                                         MoreSettings = new()
                                         {
                                             SqliteCodeFirstEnableDefaultValue = true //启用默认值
                                         }
                                     },
                                     db => { db.Aop.OnLogExecuting = (sql, pars) => { }; });
                             return sqlSugar;
                         });
                         services.AddLogging(loggingBuilder =>
                         {
                             loggingBuilder.ClearProviders();
                             loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                             loggingBuilder.AddNLog();
                         });
                     }
                 )
                 .Build();
    
    /// <summary>
    /// Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null"/>.</returns>
    public static T? GetService<T>() where T : class
    {
        return Host.Services.GetService(typeof(T)) as T;
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await  Host.StopAsync();
        Host.Dispose();
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        this._taskbar = (TaskbarIcon?)this.FindResource("TaskbarIcon");
        EfficiencyModeUtilities.SetEfficiencyMode(true);
        this._taskbar?.ForceCreate();
        InitialFolders();
        NlogConfig();
        await Host.StartAsync();
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
            ArchiveFileName = Path.Combine(LogFolder, "Debug.{#}.log"),
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
        config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logConsole);
        config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Info, logFile);
        config.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, errorFile);
        // Apply config           
        LogManager.Configuration = config;
    }
}