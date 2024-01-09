using System.IO;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using ClipSharp.Win.Display;

namespace ClipSharp.Win.Display;

public class ClipDisplayData
{
    public int Index { get; init; }
    public ClipDisplayFormat Format { get; set; }
    public DateTime DateTime { get; set; }
    public string ClipFormatString { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    public FlowDocument Document
    {
        get
        {
            var document = new FlowDocument();
            document.Blocks.Add(new Paragraph(new Run(this.Text)));
            return document;
        }
    }

    public string Html { get; set; } = string.Empty;
    public string ImagePath { get; set; } = String.Empty;

    public BitmapImage? Image
    {
        get
        {
            if (!File.Exists(this.ImagePath))
                return null;

            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(this.ImagePath);
            image.EndInit();
            return image;
        }
    }

    public string FilePaths { get; set; } = string.Empty;

    public List<FileInfo> FileInfo
    {
        get { return this.FilePaths == string.Empty ? [] : this.FilePaths.Split(';').Select(filePath => new FileInfo(filePath)).ToList(); }
    }
}