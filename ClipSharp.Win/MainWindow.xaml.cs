using System.Windows;
using ClipSharp.Win.ViewModel;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Win;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly ILogger<MainWindow> logger;
    private readonly MainWindowViewModel model;

    public MainWindow(ILogger<MainWindow> logger, MainWindowViewModel model)
    {
        this.logger = logger;
        this.model = model;
        this.InitializeComponent();
        this.Loaded += this.MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        this.DataContext = this.model;
    }
}