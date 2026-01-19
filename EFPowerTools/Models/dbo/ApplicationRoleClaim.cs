using Microsoft.AspNetCore.Identity;

namespace EFPowerTools.Models.dbo;

public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
{
    public ApplicationRole Role { get; set; } = null!;
}