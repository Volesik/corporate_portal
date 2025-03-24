using CorporatePortal.DL.Abstractions.Extensions;
using CorporatePortal.DL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.DL.EntityFramework;

public class CorporatePortalContext : DbContext
{
    public CorporatePortalContext() {}

    public CorporatePortalContext(DbContextOptions<CorporatePortalContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfo>().ConfigureBaseProperties();
        
        modelBuilder.Entity<UserInfo>()
            .HasIndex(u => u.UniqueId)
            .IsUnique();
    }

    public DbSet<UserInfo> UserInfos { get; set; } = null!;
}