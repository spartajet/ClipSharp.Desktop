using System;

namespace ClipSharp.Core.ClipBoard;

public interface IClipboardListener
{
    event EventHandler ClipboardUpdated;
    void RegisterListener();
    void UnRegisterListener();
}