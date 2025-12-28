using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TokenEntity> Tokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TokenEntity>().HasKey(t => new { t.UserId, t.Key });
    }
}

public class TokenEntity
{
    public string UserId { get; set; } = null!;
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}