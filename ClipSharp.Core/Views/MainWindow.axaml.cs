using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using ClipSharp.Core.ViewModels;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Media;
using FluentAvalonia.UI.Windowing;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Core.Views;

public partial class MainWindow : AppWindow
{
    private readonly ILogger<MainWindow> _logger;
    private readonly MainWindowViewModel model;
    private readonly MainView mainView;
    public MainWindow(ILogger<MainWindow> logger, MainWindowViewModel model, MainView mainView)
    {
        this._logger = logger;
        this.model = model;
        this.mainView = mainView;
        this.InitializeComponent();
        this.DataContext = model;
        
        // this.Content = this.mainView;
        this.TitleBar.ExtendsContentIntoTitleBar = true;
        this.TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        Application.Current.ActualThemeVariantChanged += this.OnActualThemeVariantChanged;
    }

    private void OnActualThemeVariantChanged(object? sender, EventArgs e)
    {
        if (!this.IsWindows11)
            return;

        if (this.ActualThemeVariant != FluentAvaloniaTheme.HighContrastTheme)
        {
            // TryEnableMicaEffect();
        }
        else
        {
            this.ClearValue(BackgroundProperty);
            this.ClearValue(TransparencyBackgroundFallbackProperty);
        }
    }
}