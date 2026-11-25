using Microsoft.EntityFrameworkCore;
using RaceTracker.Models;

namespace RaceTracker.Data;

public class RaceDbContext : DbContext
{
    public RaceDbContext(DbContextOptions<RaceDbContext> options) : base(options) { }

    public DbSet<Athlete> Athletes => Set<Athlete>();
    public DbSet<Race> Races => Set<Race>();
    public DbSet<Checkpoint> Checkpoints => Set<Checkpoint>();
    public DbSet<Result> Results => Set<Result>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Result>().Property(r => r.ElapsedTime).HasPrecision(10, 2);
        b.Entity<Result>().Property(r => r.SegmentSpeed).HasPrecision(6, 2);
        b.Entity<Result>().Property(r => r.GapToFirst).HasPrecision(10, 2);
        b.Entity<Race>().Property(r => r.DistanceKm).HasPrecision(6, 2);
        b.Entity<Checkpoint>().Property(c => c.DistanceKm).HasPrecision(6, 2);
    }
}