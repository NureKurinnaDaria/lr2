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
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Team> Teams => Set<Team>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Result>().Property(r => r.ElapsedTime).HasPrecision(10, 3);
        b.Entity<Result>().Property(r => r.SegmentSpeed).HasPrecision(6, 2);
        b.Entity<Result>().Property(r => r.GapToFirst).HasPrecision(10, 2);
        b.Entity<Race>().Property(r => r.DistanceKm).HasPrecision(6, 2);
        b.Entity<Checkpoint>().Property(c => c.DistanceKm).HasPrecision(6, 2);
        b.Entity<Country>().Property(c => c.Code).HasMaxLength(3);
        b.Entity<Country>().HasIndex(c => c.Code).IsUnique();
        b.Entity<Team>().HasIndex(t => t.Name).IsUnique();
        b.Entity<Result>().HasIndex(r => new { r.RaceId, r.CheckpointNo });
        b.Entity<Result>().HasIndex(r => new { r.AthleteId, r.RaceId });
        b.Entity<Race>().Property(r => r.Status).HasMaxLength(20).HasDefaultValue("Scheduled");
    }
}