﻿using System;
using SqlSugar;

namespace ClipSharp.Core.ClipBoard;

[SugarTable("DatabaseConfig")]
public class DatabaseConfig
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    public string Key { get; set; } = string.Empty;
    public int IntValue { get; set; }
    public long LongValue { get; set; }
    public string StringValue { get; set; } = string.Empty;
}