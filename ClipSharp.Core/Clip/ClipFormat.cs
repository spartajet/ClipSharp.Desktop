namespace ClipSharp.Core.Clip;

public enum ClipFormat
{
#if WINDOWS
    Text,
    Tiff,
    UnicodeText,
    WaveAudio,
    DspText,
    DspBitmap,
    Rtf,
    Html,
    CommaSeparatedValue,
#endif
}