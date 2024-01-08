using System.IO;

namespace ClipSharp.Win.Display;

public class ClipDisplayData
{
    public int Index { get; init; }
    public ClipDisplayFormat Format { get; set; }
    public DateTime DateTime { get; set; }
}

public class ClipDisplayText : ClipDisplayData
{
    public string Text { get; set; } = string.Empty;
}

public class ClipDisplayRichText : ClipDisplayData
{
    public string Text { get; set; } = string.Empty;
}

public class ClipDisplayHtml : ClipDisplayData
{
    public string Html { get; set; } = string.Empty;
}

public class ClipDisplayImage : ClipDisplayData
{
    public Image? Image { get; set; }
}

public class ClipDisplayFileDropList : ClipDisplayData
{
    public FileInfo? FileInfo { get; set; }
}