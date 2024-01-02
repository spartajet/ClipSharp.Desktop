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

    private const int WM_CLIPBOARDUPDATE = 0x031D;
    protected override void WndProc(ref Message m)
    {
        // WM_CLIPBOARDUPDATE 消息
        // const int WM_CLIPBOARDUPDATE = 0x031D;

        if (m.Msg == WM_CLIPBOARDUPDATE)
        {
            // 剪切板内容发生改变时的处理逻辑
            OnClipboardChanged();
        }
        base.WndProc(ref m);
    }

    private void OnClipboardChanged()
    {
        this.logger.LogInformation("ClipBoard Update!");
        // 处理剪切板内容变化
        // 例如: string clipboardText = Clipboard.GetText();
    }

        
        
}
#endif