using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using ClipSharp.Core.ClipBoard.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Core.ClipBoard;

public class ClipboardListener : IHostedService
{
#if WINDOWS
    IntPtr _clipboardViewerNext;
    private IntPtr hook = IntPtr.Zero;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// nCode：表示钩子代码，是一个整数值。如果小于0，则是系统消息，需要将钩子信息传递给下一个钩子，如果大于等于0，则可拦截。
    /// wParam：表示一个消息或一个系统事件的参数，通常是一个虚拟键码或者一个句柄等。
    /// lParam：表示一个消息或一个系统事件的参数，通常是一个指向 MSG 或 KBDLLHOOKSTRUCT 等结构体的指针。
    /// </summary>
    internal delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx", SetLastError = true)]
    internal static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", SetLastError = true)]
    internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern bool AddClipboardFormatListener(IntPtr hwnd);

    [DllImport("user32.dll")]
    private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

    [DllImport("user32.dll")]
    private static extern int WaitMessage();

    [DllImport("user32.dll")]
    public static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string name);

#endif


    // Windows 消息常数
    private const int WM_CLIPBOARDUPDATE = 0x031D;
    private const int WM_DRAWCLIPBOARD = 0x0308;
    private const int WM_CHANGECBCHAIN = 0x030D;
    private const int WH_KEYBOARD_LL = 13; //键盘 

    /// <inheritdoc />
    // public event EventHandler? ClipboardUpdated;
    private ILogger<ClipboardListener> logger;

    private Task? listenerTask;
    private CancellationTokenSource? listeningCancellationTokenSource;
    private HookWindows hookWindows;

#if WINDOWS
    public ClipboardListener(ILogger<ClipboardListener> logger, HookWindows hookWindows)
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

        AddClipboardFormatListener(this.hookWindows.Handle);
        // IntPtr handle = Process.GetCurrentProcess().Handle;
        // Window window = new Window();
        // // this._clipboardViewerNext = SetClipboardViewer(handle);
        // IntPtr h = GetModuleHandle(null);
        // IntPtr h1 = window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
        // bool reuslt = AddClipboardFormatListener(h);
        //
        // this.hook = SetWindowsHookEx(WM_CLIPBOARDUPDATE, this.MessageOperate, h, 0);
        //
        // if (this.hook == IntPtr.Zero)
        // {
        //     int error = Marshal.GetLastWin32Error();
        //     this.logger.LogError("registe clipboard hook error:0x{Error:X}", error);
        // }
        //
        // if (this.hook != IntPtr.Zero)
        // {
        //     this.logger.LogInformation("Start Clipboard Listener");
        // }
        // else
        // {
        //     this.logger.LogError("Start Clipboard Listener Failed");
        // }

        return Task.CompletedTask;
#endif
    }


    private int MessageOperate(int code, IntPtr wParam, IntPtr lParam)
    {
        if (code < 0) return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        int para = wParam.ToInt32();
        switch (para)
        {
            case WM_DRAWCLIPBOARD:
                this.logger.LogInformation("Clipboard Updated! Para:{Para}", para);
                break;
            case WM_CHANGECBCHAIN:
                this.logger.LogInformation("Clipboard Changed! Para:{Para}", para);
                break;
        }

        return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        ;
    }


    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
#if WINDOWS
        RemoveClipboardFormatListener(this.hookWindows.Handle);
        // IntPtr handle = Process.GetCurrentProcess().Handle;
        // // ChangeClipboardChain(handle, this._clipboardViewerNext);
        // bool result = RemoveClipboardFormatListener(handle);
        // if (this.hook != IntPtr.Zero)
        // {
        //     UnhookWindowsHookEx(this.hook);
        //     this.logger.LogInformation("Stop Clipboard Listener");
        // }


#endif
    }


    // private class HookWindow: Window
    // {
    //     public HookWindow()
    //     {
    //         // 监听系统消息
    //         PlatformImpl.AddSysMessageHook(SystemMessageHook);
    //     }
    //
    //     private void SystemMessageHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
    //     {
    //         // 检查系统消息是否为 WM_SYSCOMMAND
    //         if (msg == NativeMethods.WM_SYSCOMMAND)
    //         {
    //             // 检查系统命令是否为 SC_CLOSE
    //             if ((int)wParam == NativeMethods.SC_CLOSE)
    //             {
    //                 // 取消关闭窗口
    //                 CancelClose();
    //             }
    //         }
    //     }
    // }
}