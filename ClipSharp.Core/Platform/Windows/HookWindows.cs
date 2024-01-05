﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Windows.Win32;
using ClipSharp.Core.Database.Entity;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace ClipSharp.Core.Platform.Windows;

#if WINDOWS
public class HookWindows : Form
{
    public const int MY_HOTKEY_ID = 1234; // 热键的唯一标识符
    private readonly ILogger<HookWindows> logger;
    private readonly ISqlSugarClient db;

    public Action<Form>? ClipBoardUpdateAction { get; set; }
    public Action<Form>? HotKeyAction { get; set; }

    public HookWindows(ILogger<HookWindows> logger, ISqlSugarClient db)
    {
        this.logger = logger;
        this.db = db;
    }

    protected override void WndProc(ref Message m)
    {
        switch ((uint)m.Msg)
        {
            case PInvoke.WM_CLIPBOARDUPDATE:
                this.OnClipboardChanged();
                this.ClipBoardUpdateAction?.Invoke(this);
                break;
            case PInvoke.WM_HOTKEY:
                if (m.WParam.ToInt32() == MY_HOTKEY_ID)
                {
                    this.OnHotKey();
                    this.HotKeyAction?.Invoke(this);
                }
                break;
        }
        base.WndProc(ref m);
    }

    private void OnClipboardChanged()
    {
        this.logger.LogInformation("ClipBoard Update!");
        IDataObject? clipboardData = Clipboard.GetDataObject();

        if (clipboardData == null)
            return;

        string[] formats = clipboardData.GetFormats();
        string clipFormat = JsonSerializer.Serialize(formats);
        string? dataClassName = clipboardData.GetType().FullName;
        if (formats.Contains(DataFormats.UnicodeText))
        {
            string data = clipboardData.GetData(DataFormats.UnicodeText) as string ?? string.Empty;

            long id = this.db.Insertable(new ClipHistory() { DataClassName = dataClassName ?? "", ClipFormat = clipFormat, ClipData = data }).ExecuteReturnSnowflakeId();
            this.logger.LogInformation("Save Clip Data OK, Id:{Id}", id);
        }
        else if (formats.Contains(DataFormats.Bitmap))
        {
            if (clipboardData.GetData(DataFormats.Bitmap) is not Image image)
                return;

            string imagePath = Path.Combine(App.ImageFolder, $"{DateTime.Now:yyyyMMddHHmmssfff}.png");
            image.Save(imagePath);
            long id = this.db.Insertable(new ClipHistory()
            {
                DataClassName = dataClassName ?? "",
                ClipFormat = clipFormat,
                ClipData = imagePath
            }).ExecuteReturnSnowflakeId();
            this.logger.LogInformation("Save Clip Data OK, Id:{Id}", id);
        }
        else if (formats.Contains(DataFormats.FileDrop))
        {
            if (clipboardData.GetData(DataFormats.FileDrop) is not string[] data || data.Length == 0) return;

            long id = this.db.Insertable(new ClipHistory() { DataClassName = dataClassName ?? "", ClipFormat = clipFormat, ClipData = data[0] }).ExecuteReturnSnowflakeId();
            this.logger.LogInformation("Save Clip Data OK, Id:{Id}", id);
        }
        else
        {
            this.logger.LogWarning("Other DataFormat!");
        }
    }

    private void OnHotKey()
    {
        
        this.logger.LogInformation("Hotkey Triggered!");
    }
}
#endif