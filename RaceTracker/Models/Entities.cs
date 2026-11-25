namespace RaceTracker.Models;

public class Athlete
{
    public int AthleteId { get; set; }
    public string FullName { get; set; } = "";
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
    public string? Team { get; set; }
    public List<Result> Results { get; set; } = new();
    public DateOnly? BirthDate { get; set; }
}

public class Race
{
    public int RaceId { get; set; }
    public string Name { get; set; } = "";
    public int Season { get; set; }
    public DateOnly RaceDate { get; set; }
    public decimal DistanceKm { get; set; }
    public List<Checkpoint> Checkpoints { get; set; } = new();
}

public class Checkpoint
{
    public int CheckpointId { get; set; }
    public int RaceId { get; set; }
    public Race Race { get; set; } = null!;
    public int CheckpointNo { get; set; }
    public decimal DistanceKm { get; set; }
}

public class Result
{
    public int ResultId { get; set; }
    public int AthleteId { get; set; }
    public Athlete Athlete { get; set; } = null!;
    public int RaceId { get; set; }
    public Race Race { get; set; } = null!;
    public int CheckpointId { get; set; }
    public Checkpoint Checkpoint { get; set; } = null!;
    public int CheckpointNo { get; set; }
    public DateTime AstroTime { get; set; }
    public decimal ElapsedTime { get; set; }
    public decimal SegmentSpeed { get; set; }
    public int Rank { get; set; }
    public decimal GapToFirst { get; set; }
}

public class Country
{
    public int CountryId { get; set; }
    public string Code { get; set; } = "";
    public string? Name { get; set; }
    public List<Athlete> Athletes { get; set; } = new();
}