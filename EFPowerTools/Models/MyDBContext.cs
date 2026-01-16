using EFPowerTools.Models.dbo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EFPowerTools.Models;

public partial class MyDBContext(DbContextOptions<MyDBContext> options, IHttpContextAccessor _httpContextAccessor) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, IdentityUserRole<Guid>, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>(options)
{
    /// <summary>
    /// 角色
    /// </summary>
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }

    /// <summary>
    /// ApplicationRoleClaims
    /// </summary>
    public DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }

    /// <summary>
    /// 使用者
    /// </summary>
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    /// <summary>
    /// ApplicationUserClaims
    /// </summary>
    public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }

    /// <summary>
    /// ApplicationUserLogins
    /// </summary>
    public DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }

    /// <summary>
    /// ApplicationUserTokens
    /// </summary>
    public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }

    /// <summary>
    /// Units
    /// </summary>
    public DbSet<Unit> Units { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 明確指定 IdentityUserRole<Guid> 對應 AspNetUserRoles 資料表
        modelBuilder.Entity<IdentityUserRole<Guid>>(b =>
        {
            b.ToTable("AspNetUserRoles");
            b.HasKey(r => new { r.UserId, r.RoleId });
        });

        modelBuilder.ApplyConfiguration(new Configurations.ApplicationRoleConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ApplicationRoleClaimConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ApplicationUserClaimConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ApplicationUserLoginConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ApplicationUserTokenConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.UnitConfiguration());
        OnModelCreatingGeneratedFunctions(modelBuilder);
        //OnModelCreatingPartial(modelBuilder);
    }

    // private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    /// <summary>
    /// override SaveChanges
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges()
    {
        SetAuditableValues();
        return base.SaveChanges();
    }

    /// <summary>
    /// override SaveChangesAsync
    /// </summary>
    /// <param name = "cancellationToken"></param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditableValues();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditableValues()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => (e.Entity is IModifyRecord || e.Entity is ISoftDelete)
                && (e.State == EntityState.Added || e.State == EntityState.Modified)
            );

        var now = DateTime.Now;
        var userId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var loginUser = string.IsNullOrWhiteSpace(userId) ? Guid.Empty : new Guid(userId);

        foreach (var entry in entries)
        {
            if ((entry.Entity is IModifyRecord entity))
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreateTime = entity.CreateTime == default ? now : entity.CreateTime;
                    entity.UpdateTime = entity.UpdateTime == default ? now : entity.UpdateTime;
                    entity.CreateUserId = loginUser;
                }
                else
                {
                    Entry(entity).Property(p => p.CreateTime).IsModified = false;
                    Entry(entity).Property(p => p.CreateUserId).IsModified = false;

                    var updateTimeIsModified = Entry(entity).Property(p => p.UpdateTime).IsModified;
                    entity.UpdateTime = updateTimeIsModified ? entity.UpdateTime : now;
                }
                entity.UpdateUserId = loginUser;
            }

            if (entry.Entity is ISoftDelete deleteEntity)
            {
                if (entry.State == EntityState.Modified && deleteEntity.IsDeleted == true)
                {
                    deleteEntity.DeletedTime = now;
                    deleteEntity.DeleteUserId = loginUser;
                }
            }
        }
    }
}