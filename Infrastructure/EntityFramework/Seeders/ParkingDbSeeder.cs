using CoreApp.Entities;
using CoreApp.Enums;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.EntityFramework.Seeders;

public class ParkingDbSeeder : IDataSeeder
{
    public int Order => 2;

    private readonly ParkingDbContext _context;
    private readonly ILogger<ParkingDbSeeder> _logger;

    public ParkingDbSeeder(
        ParkingDbContext context,
        ILogger<ParkingDbSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedGatesAsync();
        await SeedTariffsAsync();
    }

    private async Task SeedGatesAsync()
    {
        if (await _context.ParkingGates.AnyAsync())
        {
            _logger.LogInformation("Bramki już istnieją — pomijam.");
            return;
        }

        _context.ParkingGates.AddRange(
            new ParkingGate
            {
                Id = Guid.Parse("11111111-AAAA-4444-8888-111111111111"),
                Name = "Entry Gate",
                Type = GateType.Entry,
                Location = "Main Gate",
                IsOperational = true
            },
            new ParkingGate
            {
                Id = Guid.Parse("22222222-BBBB-4444-8888-222222222222"),
                Name = "Exit Gate",
                Type = GateType.Exit,
                Location = "Back Gate",
                IsOperational = true
            }
        );

        await _context.SaveChangesAsync();
    }

    private async Task SeedTariffsAsync()
    {
        if (await _context.ParkingTariffs.AnyAsync())
        {
            _logger.LogInformation("Taryfy już istnieją — pomijam.");
            return;
        }

        _context.ParkingTariffs.AddRange(
            new ParkingTariff
            {
                Id = Guid.Parse("33333333-CCCC-4444-8888-333333333333"),
                Name = "Standard",
                FreeParkingDuration = TimeSpan.FromMinutes(15),
                HourlyRate = 5m,
                DailyMaxRate = 50m,
                IsActive = true
            },
            new ParkingTariff
            {
                Id = Guid.Parse("44444444-DDDD-4444-8888-444444444444"),
                Name = "Premium",
                FreeParkingDuration = TimeSpan.FromMinutes(10),
                HourlyRate = 8m,
                DailyMaxRate = 80m,
                IsActive = false
            }
        );

        await _context.SaveChangesAsync();
    }
}