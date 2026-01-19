namespace EFPowerTools.Models.dbo;

/// <summary>
/// 玩家資料
/// </summary>
public class Player : IModifyRecord
{
    /// <summary>
    /// 玩家UID
    /// </summary>
    public string PlayerId { get; set; } = null!;

    /// <summary>
    /// 玩家名稱
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 總部等級
    /// </summary>
    public int HeadquartersLv { get; set; }

    /// <summary>
    /// 坦克戰力
    /// </summary>
    public decimal TankPower { get; set; }

    /// <summary>
    /// 飛機戰力
    /// </summary>
    public decimal AircraftPower { get; set; }

    /// <summary>
    /// 導彈戰力
    /// </summary>
    public decimal MissilePower { get; set; }

    /// <summary>
    /// T10 軍隊
    /// </summary>
    public bool HasT10 { get; set; }

    /// <summary>
    /// 特種部隊
    /// </summary>
    public int TechSpecialForces { get; set; }

    /// <summary>
    /// 攻城拔寨
    /// </summary>
    public int TechSiegeToSeize { get; set; }

    /// <summary>
    /// 英雄
    /// </summary>
    public int TechHero { get; set; }

    /// <summary>
    /// 防禦工作
    /// </summary>
    public int TechDefenseFortifications { get; set; }

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

    public List<PlayerSnapshot> PlayerSnapshots { get; set; } = new List<PlayerSnapshot>();
}