using AuctriAPI.Core.Entitites;
using Microsoft.EntityFrameworkCore;

namespace AuctriAPI.Infrastructure.Persistence;

public class AuctriDbContext : DbContext
{
    public AuctriDbContext(DbContextOptions<AuctriDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Additional configurations
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });
    }
}