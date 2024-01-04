using System.Threading;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using ClipSharp.Core.Platform.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace ClipSharp.Core.Service;

public class ClipboardService : IHostedService
{
    private readonly ILogger<ClipboardService> logger;


#if WINDOWS
    private readonly HookWindows hookWindows;
    public ClipboardService(ILogger<ClipboardService> logger, HookWindows hookWindows)
    {
        this.logger = logger;
        this.hookWindows = hookWindows;
    }
#else
    public ClipboardListener(ILogger<ClipboardListener> logger)
    {
        this.logger = logger;
    }
#endif


    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
#if WINDOWS
        PInvoke.AddClipboardFormatListener(new(this.hookWindows.Handle));


        return Task.CompletedTask;
#else
        return Task.CompletedTask;
#endif
    }


    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
#if WINDOWS
        PInvoke.RemoveClipboardFormatListener(new(this.hookWindows.Handle));
        this.logger.LogInformation("Remove Clipboard FormatListener");
        return Task.CompletedTask;

#else
        return Task.CompletedTask;
#endif
    }
}