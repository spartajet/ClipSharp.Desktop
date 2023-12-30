using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tmds.DBus.Protocol;

#if Windows
using Avalonia.Win32.Interop;
#endif

namespace ClipSharp.Desktop.ClipBoard;

public class WindowsClipboardListener: IClipboardListener
{
    #if Windows
    IntPtr _clipboardViewerNext;
    private IntPtr hook;
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    
    internal delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx", SetLastError = true)]
    internal static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);
    #endif
    

    
    // Windows 消息常数
    private const int WM_CLIPBOARDUPDATE = 0x031D;
    private const int WM_DRAWCLIPBOARD = 0x0308;
    private const int WM_CHANGECBCHAIN = 0x030D;
    /// <inheritdoc />
    public event EventHandler? ClipboardUpdated;

    private HookProc hookProc =new((code, w, l) =>
    {
        return 0;
    }) ;

    /// <inheritdoc />
    public void RegisterListener()
    {
        IntPtr handle= Process.GetCurrentProcess().Handle;
        this._clipboardViewerNext= SetClipboardViewer(handle);
        this.hook= SetWindowsHookEx(WM_CLIPBOARDUPDATE, this.hookProc , IntPtr.Zero, (uint) AppDomain.GetCurrentThreadId());
    }

    /// <inheritdoc />
    public void UnRegisterListener()
    {
        IntPtr handle= Process.GetCurrentProcess().Handle;
        ChangeClipboardChain(handle, this._clipboardViewerNext);
    }
}


