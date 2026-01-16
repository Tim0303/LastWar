using Microsoft.AspNetCore.Identity;

namespace EFPowerTools.Models.dbo;

/// <summary>
/// 角色
/// </summary>
public class ApplicationRole : IdentityRole<Guid>
{
    public Guid Id { get; set; }

    /// <summary>
    /// 個人首頁區塊
    /// </summary>
    public int HomePageType { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string Remark { get; set; } = null!;

    /// <summary>
    /// 是否為系統預設
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 建立人Id
    /// </summary>
    public Guid CreateUserId { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改人Id
    /// </summary>
    public Guid UpdateUserId { get; set; }

    /// <summary>
    /// 修改時間
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 是否刪除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 刪除人Id
    /// </summary>
    public Guid? DeleteUserId { get; set; }

    /// <summary>
    /// 刪除時間
    /// </summary>
    public DateTime? DeletedTime { get; set; }

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public List<ApplicationRoleClaim> ApplicationRoleClaims { get; set; } = new List<ApplicationRoleClaim>();

    public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}