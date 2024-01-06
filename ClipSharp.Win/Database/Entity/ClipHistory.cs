using SqlSugar;

namespace ClipSharp.Win.Database.Entity;

[SugarTable("ClipHistory")]
public class ClipHistory
{
    [SugarColumn(IsPrimaryKey = true)]
    public long Id { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string ClipFormat { get; set; } = string.Empty;
    public string ClipData { get; set; } = string.Empty;
    public string DataClassName { get; set; } = string.Empty;
}