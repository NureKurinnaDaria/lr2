using Microsoft.EntityFrameworkCore;
using RaceTracker.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RaceDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// п.5: автоматичне застосування міграцій під час запуску
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RaceDbContext>();
    db.Database.Migrate();
    DataSeeder.Seed(db);
}

app.MapGet("/", () => "RaceTracker is running");
app.Run();