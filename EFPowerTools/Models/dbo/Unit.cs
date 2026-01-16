using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace EFPowerTools.Models.dbo;

public class Unit : IEnable, IModifyRecord, ISoftDelete, IDefault
{
    public Guid UnitId { get; set; }

    public string UnitName { get; set; } = null!;

    public bool IsExternal { get; set; }

    public bool IsDefault { get; set; }

    public bool IsEnabled { get; set; }

    public Guid CreateUserId { get; set; }

    public DateTime CreateTime { get; set; }

    public Guid UpdateUserId { get; set; }

    public DateTime UpdateTime { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? DeleteUserId { get; set; }

    public DateTime? DeletedTime { get; set; }

    public List<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}

