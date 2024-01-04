using ClipSharp.Core.Platform.Windows;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Core.ClipBoard.Windows;

#if WINDOWS

using System.Windows.Forms;

#endif

#if WINDOWS
public class HookWindows: Form
{
    private ILogger<HookWindows> logger;

    public HookWindows(ILogger<HookWindows> logger)
    {
        this.logger = logger;
    }

    protected override void WndProc(ref Message m)
    {
        switch (m.Msg)
        {
            case NativeInvoke.WM_CLIPBOARDUPDATE:
                this.OnClipboardChanged();
                break;
            case  NativeInvoke.WM_HOTKEY:
                if (m.WParam.ToInt32()==NativeInvoke.MY_HOTKEY_ID)
                {
                    this.OnHotKey();
                }
                break;
                
        }
        base.WndProc(ref m);
    }

    private void OnClipboardChanged()
    {
        this.logger.LogInformation("ClipBoard Update!");
        // 处理剪切板内容变化
        // 例如: string clipboardText = Clipboard.GetText();
    }

    private void OnHotKey()
    {
        this.logger.LogInformation("Hotkey Triggered!");
    }

        
        
}
#endif