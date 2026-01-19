using Microsoft.AspNetCore.Identity;

namespace EFPowerTools.Models.dbo;

public class ApplicationUserLogin : IdentityUserLogin<Guid>
{
    public ApplicationUser User { get; set; } = null!;
}