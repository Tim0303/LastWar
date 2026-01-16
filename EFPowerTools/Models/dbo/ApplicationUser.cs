using Microsoft.AspNetCore.Identity;

namespace EFPowerTools.Models.dbo;

/// <summary>
/// 使用者
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// 單位Id(FK Unit.UnitId)
    /// </summary>
    public Guid? UnitId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 職稱
    /// </summary>
    public string JobTitle { get; set; } = null!;

    /// <summary>
    /// 帳號類型
    /// </summary>
    public int UserType { get; set; }

    /// <summary>
    /// 是否為系統預設
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// 是否為臨時密碼
    /// </summary>
    public bool IsTemporaryPassword { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 停用時間
    /// </summary>
    public DateTime? DisabledTime { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string Remark { get; set; } = null!;

    /// <summary>
    /// 密碼過期時間
    /// </summary>
    public DateTime? PasswordExpiryTime { get; set; }

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
    /// 最後登入時間
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    public List<ApplicationUserClaim> ApplicationUserClaims { get; set; } = new List<ApplicationUserClaim>();

    public List<ApplicationUserLogin> ApplicationUserLogins { get; set; } = new List<ApplicationUserLogin>();

    public List<ApplicationUserToken> ApplicationUserTokens { get; set; } = new List<ApplicationUserToken>();

    public Unit? Unit { get; set; }

    public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
}