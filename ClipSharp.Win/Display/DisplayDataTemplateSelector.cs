using System.Windows;
using System.Windows.Controls;

namespace ClipSharp.Win.Display;

public class DisplayDataTemplateSelector : DataTemplateSelector
{
    public required DataTemplate TextTemplate { get; set; }
    public required DataTemplate RichTextTemplate { get; set; }
    public required DataTemplate HtmlTemplate { get; set; }
    public required DataTemplate ImageTemplate { get; set; }
    public required DataTemplate FileDropListTemplate { get; set; }
    public required DataTemplate AudioTemplate { get; set; }
    public required DataTemplate CustomFormatTemplate { get; set; }

    /// <inheritdoc />
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is ClipDisplayData data)
        {
            return data.Format switch
            {
                ClipDisplayFormat.Text => this.TextTemplate,
                ClipDisplayFormat.RichText => this.RichTextTemplate,
                ClipDisplayFormat.Html => this.HtmlTemplate,
                ClipDisplayFormat.Image => this.ImageTemplate,
                ClipDisplayFormat.FileDropList => this.FileDropListTemplate,
                ClipDisplayFormat.Audio => this.AudioTemplate,
                ClipDisplayFormat.CustomFormat => this.CustomFormatTemplate,
                _ => this.TextTemplate
            };
        }
        return base.SelectTemplate(item, container);
    }
}