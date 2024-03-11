using Microsoft.EntityFrameworkCore;
using PerformanceCounters.Hub.EF.Entity;

namespace PerformanceCounters.Hub.EF
{
  public class CountersDbContext : DbContext
  {
    public DbSet<DeviceEntity> Device { get; set; }
    public DbSet<ProcessEntity> Process { get; set; }
    public DbSet<CounterEntity> Counter { get; set; }

    public CountersDbContext(DbContextOptions options) : base(options)
    {
      Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<DeviceEntity>(entity => {
        entity.HasIndex(e => e.Name).IsUnique();
      });

      modelBuilder.Entity<ProcessEntity>(entity => {
        entity.HasIndex(e => new { e.Id, e.Name}).IsUnique();
      });

      modelBuilder.Entity<CounterEntity>()
        .Property(e => e.Type)
        .HasConversion<string>();

      base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
  }
}