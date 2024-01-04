using System.Threading;
using System.Threading.Tasks;
using ClipSharp.Core.ClipBoard.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
#if WINDOWS
using ClipSharp.Core.Platform.Windows;
#endif


namespace ClipSharp.Core.ClipBoard;

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
        NativeInvoke.AddClipboardFormatListener(this.hookWindows.Handle);

        return Task.CompletedTask;
#else
        return Task.CompletedTask;
#endif
    }


    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
#if WINDOWS
        NativeInvoke.RemoveClipboardFormatListener(this.hookWindows.Handle);
        this.logger.LogInformation("Remove Clipboard FormatListener");
        return Task.CompletedTask;

#else
        return Task.CompletedTask;
#endif
        
    }
}