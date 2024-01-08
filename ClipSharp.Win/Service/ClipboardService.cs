using System.Windows;
using System.Windows.Interop;
using Windows.Win32;
using Windows.Win32.Foundation;
using ClipSharp.Win.Clip;
using ClipSharp.Win.View;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Win.Service;

public class ClipboardService : IHostedService
{
    private readonly ILogger<ClipboardService> logger;


    private readonly HookWindows hookWindows;
    private ClipSelectWindow? clipSelectWindow;

    public ClipboardService(ILogger<ClipboardService> logger, HookWindows hookWindows)
    {
        this.logger = logger;
        this.hookWindows = hookWindows;
        // this.clipSelectWindow = clipSelectWindow;
        this.hookWindows.HotKeyAction = () =>
        {
            this.clipSelectWindow = App.GetService<ClipSelectWindow>();
            if (this.clipSelectWindow == null)
            {
                this.logger.LogError("ClipSelectWindow is null");
            }
            else
            {
                this.clipSelectWindow.Show();
                this.clipSelectWindow.Focus();

            }

        };
    }


    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        
        PInvoke.AddClipboardFormatListener(new(this.hookWindows.Handle));
        
        

        return Task.CompletedTask;
    }


    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        PInvoke.RemoveClipboardFormatListener(new(this.hookWindows.Handle));
        this.logger.LogInformation("Remove Clipboard FormatListener");
        return Task.CompletedTask;

    }


}