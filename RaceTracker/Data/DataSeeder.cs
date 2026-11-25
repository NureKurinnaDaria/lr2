using RaceTracker.Models;

namespace RaceTracker.Data;

public static class DataSeeder
{
    public static void Seed(RaceDbContext db)
    {
        if (db.Athletes.Any()) return;
        var rnd = new Random(42);

        var countries = new[] { "UKR", "ITA", "CZE", "FRA", "GER", "ESP", "POL", "NED" }
            .Select(c => new Country { Code = c }).ToList();
        var teams = Enumerable.Range(1, 8)
            .Select(i => new Team { Name = $"Team {i}" }).ToList();
        db.Countries.AddRange(countries);
        db.Teams.AddRange(teams);

        var athletes = Enumerable.Range(1, 50).Select(i => new Athlete
        {
            FullName = $"Athlete {i:00}",
            Country = countries[rnd.Next(countries.Count)],
            Team = teams[rnd.Next(teams.Count)],
            BirthDate = new DateOnly(1990 + rnd.Next(12), 1 + rnd.Next(12), 1 + rnd.Next(28))
        }).ToList();
        db.Athletes.AddRange(athletes);

        // базова швидкість спортсмена, км/год
        var ability = athletes.Select(_ => 38 + rnd.NextDouble() * 8).ToArray();

        var races = new List<Race>();
        for (int season = 2023; season <= 2026; season++)
            for (int n = 1; n <= 5; n++)
            {
                var race = new Race
                {
                    Name = $"Time Trial {season} #{n}",
                    Season = season,
                    RaceDate = new DateOnly(season, 3 + n, 10),
                    DistanceKm = 40,
                    Status = "Finished"
                };
                for (int c = 1; c <= 5; c++)
                    race.Checkpoints.Add(new Checkpoint { CheckpointNo = c, DistanceKm = c * 8 });
                races.Add(race);
            }
        db.Races.AddRange(races);

        foreach (var race in races)
        {
            var start = new DateTime(race.Season, race.RaceDate.Month, race.RaceDate.Day,
                                     10, 0, 0, DateTimeKind.Utc);
            var raceSpeed = ability.Select(a => a + (rnd.NextDouble() - 0.5) * 3).ToArray();
            var elapsed = new decimal[athletes.Count];

            foreach (var cp in race.Checkpoints.OrderBy(c => c.CheckpointNo))
            {
                var seg = new (int i, decimal speed)[athletes.Count];
                for (int i = 0; i < athletes.Count; i++)
                {
                    double v = raceSpeed[i] + (rnd.NextDouble() - 0.5) * 2;
                    elapsed[i] += Math.Round((decimal)(8.0 / v * 3600), 3);
                    seg[i] = (i, Math.Round((decimal)v, 2));
                }

                var ordered = Enumerable.Range(0, athletes.Count)
                    .OrderBy(i => elapsed[i]).ToList();
                decimal best = elapsed[ordered[0]];

                for (int rank = 1; rank <= ordered.Count; rank++)
                {
                    int i = ordered[rank - 1];
                    db.Results.Add(new Result
                    {
                        Athlete = athletes[i],
                        Race = race,
                        Checkpoint = cp,
                        CheckpointNo = cp.CheckpointNo,
                        AstroTime = start.AddSeconds(30.0 * i).AddSeconds((double)elapsed[i]),
                        ElapsedTime = elapsed[i],
                        SegmentSpeed = seg[i].speed,
                        Rank = rank,
                        GapToFirst = Math.Round(elapsed[i] - best, 2)
                    });
                }
            }
        }
        db.SaveChanges();
    }
}