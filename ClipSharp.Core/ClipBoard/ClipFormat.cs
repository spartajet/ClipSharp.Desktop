namespace ClipSharp.Core.ClipBoard;

public enum ClipFormat
{
#if WINDOWS
    Text = 1,
    Tiff = 6,
    UnicodeText = 13,
    WaveAudio = 12,
    DspText = 0x0081,
    DspBitmap = 0x0082,
#endif
}