using Microsoft.AspNetCore.Identity;

namespace EFPowerTools.Models.dbo;

public class ApplicationUserToken : IdentityUserToken<Guid>
{
    public ApplicationUser User { get; set; } = null!;
}