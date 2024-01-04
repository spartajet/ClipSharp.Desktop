using System.Threading;
using System.Threading.Tasks;
using ClipSharp.Core.ClipBoard.Windows;
using ClipSharp.Core.Platform.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Core.ClipBoard;

public class HotKeyService : IHostedService
{
    private readonly ILogger<HotKeyService> logger;


#if WINDOWS
    private readonly HookWindows hookWindows;
#endif

#if WINDOWS
    public HotKeyService(ILogger<HotKeyService> logger, HookWindows hookWindows)
    {
        this.logger = logger;
        this.hookWindows = hookWindows;
    }
#else
    public HotKeyService(ILogger<HotKeyService> logger)
    {
        this.logger = logger;
    }
#endif


    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        bool result = false;
#if WINDOWS
        // 0x56代表V键
        // https://learn.microsoft.com/zh-cn/windows/win32/inputdev/virtual-key-codes
        result = NativeInvoke.RegisterHotKey(this.hookWindows.Handle, NativeInvoke.MY_HOTKEY_ID, NativeInvoke.MOD_CONTROL | NativeInvoke.MOD_SHIFT, 0x56);
#endif
        
        if (result)
        {
            this.logger.LogInformation("RegistHotKey Success!");
        }
        else
        {
            this.logger.LogError("RegistHotKey Fail!");
        }
        
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        bool result = false;
#if WINDOWS
        result = NativeInvoke.UnregisterHotKey(this.hookWindows.Handle, NativeInvoke.MY_HOTKEY_ID);
#endif
        if (result)
        {
            this.logger.LogInformation("UnRegistHotKey Success!");
        }
        else
        {
            this.logger.LogError("UnRegistHotKey Fail!");
        }
        return Task.CompletedTask;
    }
}