using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace EFPowerTools.Models.dbo;

/// <summary>
/// Log
/// </summary>
public class Log
{
    public Guid LogId { get; set; }

    /// <summary>
    /// Log時間
    /// </summary>
    public DateTime? LogTime { get; set; }

    /// <summary>
    /// Log來源名稱
    /// </summary>
    public string? Logger { get; set; }

    /// <summary>
    /// Log層級
    /// </summary>
    public string? LogLevel { get; set; }

    /// <summary>
    /// Log內容
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Http方法
    /// </summary>
    public string? HttpMethod { get; set; }

    /// <summary>
    /// Request路徑
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 參數
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// Request Body內容
    /// </summary>
    public string? RequestBody { get; set; }

    /// <summary>
    /// Request Form內容
    /// </summary>
    public string? RequestForm { get; set; }

    /// <summary>
    /// Http狀態碼
    /// </summary>
    public string? HttpStatusCode { get; set; }

    /// <summary>
    /// IP位置
    /// </summary>
    public string? Ip { get; set; }

    /// <summary>
    /// 使用者Id
    /// </summary>
    public Guid? UserId { get; set; }
}

