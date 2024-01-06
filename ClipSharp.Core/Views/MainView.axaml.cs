using Avalonia.Controls;
using ClipSharp.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Core.Views;

public partial class MainView : UserControl
{
    private readonly ILogger<MainView> _logger;
    private readonly MainViewViewModel model;

    public MainView(ILogger<MainView> logger, MainViewViewModel model)
    {
        this._logger = logger;
        this.model = model;
        this.InitializeComponent();
        this.DataContext = this.model;
    }
}