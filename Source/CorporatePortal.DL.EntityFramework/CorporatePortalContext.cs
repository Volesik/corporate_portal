using CorporatePortal.DL.Abstractions.Extensions;
using CorporatePortal.DL.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace CorporatePortal.DL.EntityFramework;

public class CorporatePortalContext : DbContext
{
    public CorporatePortalContext() {}

    public CorporatePortalContext(DbContextOptions<CorporatePortalContext> options) : base(options) { }
    
    [DbFunction("regexp_replace")]
    public static string RegexReplace(string input, string pattern, string replacement, string flags)
    {
        throw new NotImplementedException("Called client-side instead of in the database.");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserInfo>().ConfigureBaseProperties();
        
        modelBuilder.Entity<UserInfo>()
            .HasIndex(u => u.UniqueId)
            .IsUnique();
        
        modelBuilder.HasDbFunction(() => RegexReplace(default, default, default, default))
            .HasTranslation(args =>
            {
                return new SqlFunctionExpression(
                    "regexp_replace",
                    args,
                    nullable: true,
                    argumentsPropagateNullability: [false, false, false, false],
                    typeof(string),
                    null);
            });
    }

    public DbSet<UserInfo> UserInfos { get; set; } = null!;
}