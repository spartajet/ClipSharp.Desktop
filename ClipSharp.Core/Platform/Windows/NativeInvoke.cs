using System;
using System.Runtime.InteropServices;

namespace ClipSharp.Core.Platform.Windows;

public partial class NativeInvoke
{
#if WINDOWS
    
    // Windows 消息常数
    public const int WM_CLIPBOARDUPDATE = 0x031D;
    public const int WM_HOTKEY = 0x0312; //热键 
    public const int WM_DRAWCLIPBOARD = 0x0308;
    public const int WM_CHANGECBCHAIN = 0x030D;
    public const int WH_KEYBOARD_LL = 13; //键盘 
    public const int MY_HOTKEY_ID = 1234; // 热键的唯一标识符
    public const int MOD_ALT = 0x0001; // ALT热键
    public const int MOD_CONTROL = 0x0002; // CONTROL热键
    public const int MOD_SHIFT = 0x0004; // SHIFT热键
    public const int MOD_WIN = 0x0008; // WIN热键
    
    [StructLayout(LayoutKind.Sequential)]
    public struct KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
    
    
    // IntPtr _clipboardViewerNext;
    // private IntPtr hook = IntPtr.Zero;

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// nCode：表示钩子代码，是一个整数值。如果小于0，则是系统消息，需要将钩子信息传递给下一个钩子，如果大于等于0，则可拦截。
    /// wParam：表示一个消息或一个系统事件的参数，通常是一个虚拟键码或者一个句柄等。
    /// lParam：表示一个消息或一个系统事件的参数，通常是一个指向 MSG 或 KBDLLHOOKSTRUCT 等结构体的指针。
    /// </summary>
    public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx", SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", EntryPoint = "UnhookWindowsHookEx", SetLastError = true)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    public static extern bool AddClipboardFormatListener(IntPtr hwnd);

    [DllImport("user32.dll")]
    public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

    [DllImport("user32.dll")]
    public static extern int WaitMessage();

    [DllImport("user32.dll")]
    public static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string name);
    
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);


#endif
}