using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace EFPowerTools.Models.dbo;

public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
{
    public int Id { get; set; }

    public Guid RoleId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }

    public ApplicationRole Role { get; set; } = null!;
}

