using System;
using SqlSugar;

namespace ClipSharp.Core.Database.Entity;

[SugarTable("ClipHistory")]
public class ClipHistory
{
    [SugarColumn(IsPrimaryKey = true)] public long Id { get; set; }
    public DateTime DateTime { get; set; }=DateTime.Now;
    public int ClipType { get; set; }
    public string Content { get; set; } = string.Empty;
}