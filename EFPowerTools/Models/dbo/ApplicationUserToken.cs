using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace EFPowerTools.Models.dbo;

public class ApplicationUserToken : IdentityUserToken<Guid>
{
    public Guid UserId { get; set; }

    public string LoginProvider { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    public ApplicationUser User { get; set; } = null!;
}

