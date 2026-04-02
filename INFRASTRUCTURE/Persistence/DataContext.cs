using DOMAIN.Entities;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Persistence;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
       {
           entity.HasKey(e => e.Id);
           entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
           entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
       });
    }
}
