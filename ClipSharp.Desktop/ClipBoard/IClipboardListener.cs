using System;

namespace ClipSharp.Desktop.ClipBoard;

public interface IClipboardListener
{
    event EventHandler ClipboardUpdated;
    void RegisterListener();
    void UnRegisterListener();
}