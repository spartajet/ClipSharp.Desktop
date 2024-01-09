using System.Collections.ObjectModel;
using System.Text.Json;
using AvaloniaEdit.Utils;
using ClipSharp.Win.Database.Entity;
using ClipSharp.Win.Display;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace ClipSharp.Win.ViewModel;

public class ClipSelectViewModel : ObservableObject
{
    private readonly ILogger<ClipSelectViewModel> logger;
    private readonly ISqlSugarClient db;
    public ObservableCollection<ClipDisplayData> ClipDisplayData { get; set; } = [];


    public ClipSelectViewModel(ILogger<ClipSelectViewModel> logger, ISqlSugarClient db)
    {
        this.logger = logger;
        this.db = db;
        this.LoadData();
    }

    private void LoadData()
    {
        List<ClipHistory> clipHistories = this.db.Queryable<ClipHistory>().OrderByDescending(it => it.DateTime).Take(50).ToList();
        if (clipHistories.Count == 0)
        {
            this.logger.LogInformation("ClipHistories is empty");
            return;
        }

        List<ClipDisplayData> items = clipHistories.Select(it =>
        {
            if (it.ClipFormat is "" or "[]")
            {
                return new ClipDisplayData()
                {
                    Format = ClipDisplayFormat.Text,
                    DateTime = it.DateTime,
                    Text = it.ClipData,
                    ClipFormatString = it.ClipFormat
                };
            }
            List<string>? formats = JsonSerializer.Deserialize<List<string>>(it.ClipFormat);
            if (formats == null || formats.Count == 0)
            {
                return new ClipDisplayData
                {
                    Format = ClipDisplayFormat.Text,
                    DateTime = it.DateTime,
                    Text = it.ClipData,
                    ClipFormatString = it.ClipFormat
                };
            }
            if (formats.Contains("System.Drawing.Bitmap"))
            {
                return new ClipDisplayData
                {
                    Format = ClipDisplayFormat.Image,
                    DateTime = it.DateTime,
                    ImagePath = it.ClipData,
                    ClipFormatString = it.ClipFormat
                };
            }
            if (formats.Contains("FileDrop"))
            {
                return new ClipDisplayData
                {
                    Format = ClipDisplayFormat.FileDropList,
                    DateTime = it.DateTime,
                    FilePaths = it.ClipData,
                    ClipFormatString = it.ClipFormat
                };
            }
            if (formats.Contains("Rich Text Format"))
            {
                return new ClipDisplayData
                {
                    Format = ClipDisplayFormat.RichText,
                    DateTime = it.DateTime,
                    Text = it.ClipData,
                    ClipFormatString = it.ClipFormat
                };
            }
            // if (formats.Contains("System.String"))
            // {
            return new ClipDisplayData
            {
                Format = ClipDisplayFormat.Text,
                DateTime = it.DateTime,
                Text = it.ClipData,
                ClipFormatString = it.ClipFormat
            };
            // }

        }).ToList();
        this.ClipDisplayData.Clear();
        this.ClipDisplayData.AddRange(items);
    }
}