using System.Windows;
using System.Windows.Input;
using ClipSharp.Win.ViewModel;
using Microsoft.Extensions.Logging;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace ClipSharp.Win.View;

public partial class ClipSelectWindow : Window
{
    private readonly ILogger<ClipSelectWindow> logger;
    private readonly ClipSelectViewModel model;

    public ClipSelectWindow(ClipSelectViewModel model, ILogger<ClipSelectWindow> logger)
    {
        this.model = model;
        this.logger = logger;
        this.InitializeComponent();
        this.Loaded += (_, _) => this.DataContext = model;
    }

    private void ClipSelectWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key==Key.Escape)
        {
            this.Close();
        }
    }

    private void ClipSelectWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.ReSize();
    }

    private void ReSize()
    {
        Screen? screen=Screen.PrimaryScreen;
        if (screen==null)
        {
            return;
        }
        this.Width = screen.WorkingArea.Width;
        this.Left = 0;
        this.Top = 200;
    }
}