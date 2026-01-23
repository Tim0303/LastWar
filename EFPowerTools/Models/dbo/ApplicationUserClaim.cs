using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace EFPowerTools.Models.dbo;

public class ApplicationUserClaim : IdentityUserClaim<Guid>
{

    public ApplicationUser User { get; set; } = null!;
}

