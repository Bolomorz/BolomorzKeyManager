using Microsoft.EntityFrameworkCore;

namespace BolomorzKeyManager.Model;

public class KeyManagerContext : DbContext
{
    #region Authentication
    public DbSet<UserAccount>? UserAccounts { get; set; }
    #endregion

    #region  KeyManagement
    public DbSet<Key>? Keys { get; set; }
    public DbSet<Password>? Passwords { get; set; }
    #endregion

    private readonly string Path;

    public KeyManagerContext()
    {
        Path = $"Database/db.sqlite";

        Database.Migrate();
        SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => base.OnModelCreating(modelBuilder);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={Path}");
}