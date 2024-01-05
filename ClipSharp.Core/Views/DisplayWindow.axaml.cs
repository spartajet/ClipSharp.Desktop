using Avalonia.Controls;
using Avalonia.Interactivity;
using ClipSharp.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Core.Views;

public partial class DisplayWindow : Window
{
    private ILogger<DisplayWindow> logger { get; set; }
    private DisplayWindowViewModel viewModel { get; set; }

    public DisplayWindow(ILogger<DisplayWindow> logger, DisplayWindowViewModel viewModel)
    {
        this.logger = logger;
        this.viewModel = viewModel;
        this.InitializeComponent();
    }


    private void SearchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}