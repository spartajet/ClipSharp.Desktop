using System.Windows.Forms;
using Windows.Win32;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Core.Platform.Windows;

#if WINDOWS


#endif

#if WINDOWS
public class HookWindows: Form
{
    public const int MY_HOTKEY_ID = 1234; // 热键的唯一标识符
    private ILogger<HookWindows> logger;

    public HookWindows(ILogger<HookWindows> logger)
    {
        this.logger = logger;
    }

    protected override void WndProc(ref Message m)
    {
        switch ((uint)m.Msg)
        {
            case PInvoke.WM_CLIPBOARDUPDATE:
                this.OnClipboardChanged();
                break;
            case  PInvoke.WM_HOTKEY:
                if (m.WParam.ToInt32()==MY_HOTKEY_ID)
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