﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;
using ClipSharp.Win.Display;
using ClipSharp.Win.ViewModel;
using Microsoft.Extensions.Logging;
using Clipboard = System.Windows.Clipboard;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextDataFormat = System.Windows.TextDataFormat;

namespace ClipSharp.Win.View;

public partial class ClipSelectWindow : Window
{
    private readonly ILogger<ClipSelectWindow> logger;
    private readonly ClipSelectViewModel model;
    private bool isEsc;
    private bool isEnter;

    public ClipSelectWindow(ClipSelectViewModel model, ILogger<ClipSelectWindow> logger)
    {
        this.model = model;
        this.logger = logger;
        this.InitializeComponent();
        this.Loaded += (_, _) =>
        {
            this.DataContext = model;
        };
        this.ContentRendered += (_, _) =>
        {

            this.HistoryListView.SelectedIndex = 0;
            System.Windows.Controls.ListViewItem firstItem = (System.Windows.Controls.ListViewItem)this.HistoryListView.ItemContainerGenerator.ContainerFromIndex(0);

            firstItem.Focus();
        };
    }


    private void ClipSelectWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Escape)
            return;

        this.isEsc = true;
        this.Close();
    }

    private void ClipSelectWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.ReSizeWindow();

    }

    private void ReSizeWindow()
    {
        Screen? screen = Screen.PrimaryScreen;
        if (screen == null)
        {
            return;
        }
        // visual 是我们准备找到缩放量的控件。
        // var ct = PresentationSource.FromVisual(visual)?.CompositionTarget;
        // var matrix = ct == null ? Matrix.Identity : ct.TransformToDevice;
        // PInvoke.GetMonitorInfo(new HMONITOR())
        this.Width = this.GetScreenWidth() - 100;
        this.Left = 50;
        this.Top = 200;
    }

    const double DpiPercent = 96;

    private double GetScreenWidth()
    {
        var intPtr = new WindowInteropHelper(this).Handle; //获取当前窗口的句柄
        var screen = Screen.FromHandle(intPtr); //获取当前屏幕

        double width = 0;
        using Graphics currentGraphics = Graphics.FromHwnd(intPtr);
        double dpiXRatio = currentGraphics.DpiX / DpiPercent;
        double dpiYRatio = currentGraphics.DpiY / DpiPercent;
        width = screen.WorkingArea.Width / dpiXRatio;
        //var width = screen.WorkingArea.Width / dpiXRatio;
        //var left = screen.WorkingArea.Left / dpiXRatio;
        //var top = screen.WorkingArea.Top / dpiYRatio;
        return width;
    }

    private void ClipSelectWindow_OnLostFocus(object sender, RoutedEventArgs e)
    {
        // this.Close();
    }

    private void ClipSelectWindow_OnDeactivated(object? sender, EventArgs e)
    {
        if (this.isEsc||this.isEnter)
        {
            return;
        }
        this.Close();
    }

    private void DisplayItem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;

        if (sender is System.Windows.Controls.ListViewItem item)
        {
            if (item.DataContext is ClipDisplayData data)
            {
                switch (data.Format)
                {

                    case ClipDisplayFormat.Text:
                        Clipboard.SetText(data.Text);
                        break;
                    case ClipDisplayFormat.RichText:
                        Clipboard.SetText(data.Text,TextDataFormat.Rtf);
                        
                        break;
                    case ClipDisplayFormat.Html:
                        break;
                    case ClipDisplayFormat.Image:
                        break;
                    case ClipDisplayFormat.FileDropList:
                        break;
                    case ClipDisplayFormat.Audio:
                        break;
                    case ClipDisplayFormat.CustomFormat:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        this.isEnter = true;
        this.logger.LogInformation($"Log Id:");
        this.Close();
    }
}