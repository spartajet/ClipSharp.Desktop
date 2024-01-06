using System.Windows.Interop;
using Windows.Win32;
using Windows.Win32.Foundation;
using ClipSharp.Win.Clip;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Win.Service;

public class ClipboardService : IHostedService
{
    private readonly ILogger<ClipboardService> logger;

    
    private readonly HookWindows hookWindows;
    public ClipboardService(ILogger<ClipboardService> logger, HookWindows hookWindows)
    {
        this.logger = logger;
        this.hookWindows = hookWindows;
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