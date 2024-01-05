using System.Threading;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using ClipSharp.Core.Platform.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Core.Service;

public class HotKeyService : IHostedService
{
    private readonly ILogger<HotKeyService> logger;
    public const int MY_HOTKEY_ID = 1234; // 热键的唯一标识符
    // public const int MOD_CONTROL = 0x0002; // CONTROL热键
    // public const int MOD_SHIFT = 0x0004; // SHIFT热键
    // public const int MOD_WIN = 0x0008; // WIN热键

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
        result = PInvoke.RegisterHotKey(new(this.hookWindows.Handle), MY_HOTKEY_ID, HOT_KEY_MODIFIERS.MOD_CONTROL| HOT_KEY_MODIFIERS.MOD_SHIFT, 0x56);
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
        result = PInvoke.UnregisterHotKey(new(this.hookWindows.Handle), MY_HOTKEY_ID);
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