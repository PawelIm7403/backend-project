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
        await SeedActiveSessionAsync();
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
    
    private async Task SeedActiveSessionAsync()
    {
        var licensePlate = "KR12345";

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);

        if (vehicle is null)
        {
            vehicle = new Vehicle
            {
                Id = Guid.Parse("55555555-EEEE-4444-8888-555555555555"),
                LicensePlate = licensePlate,
                Brand = "BMW",
                Color = "Black"
            };

            _context.Vehicles.Add(vehicle);
        }

        var gate = await _context.ParkingGates
            .FirstOrDefaultAsync(g => g.Type == GateType.Entry);

        var tariff = await _context.ParkingTariffs
            .FirstOrDefaultAsync(t => t.IsActive);

        if (gate is null || tariff is null)
        {
            return;
        }

        var activeSession = await _context.ParkingSessions
            .FirstOrDefaultAsync(s => s.VehicleId == vehicle.Id && s.IsActive);

        if (activeSession is not null)
        {
            activeSession.EntryTime = DateTime.UtcNow.AddMinutes(-30);
            activeSession.ExitTime = null;
            activeSession.ParkingFee = 10m;
            activeSession.IsPaid = false;

            await _context.SaveChangesAsync();
            return;
        }

        var session = new ParkingSession
        {
            Id = Guid.Parse("66666666-FFFF-4444-8888-666666666666"),
            VehicleId = vehicle.Id,
            Vehicle = vehicle,
            GateName = gate.Name,
            EntryTime = DateTime.UtcNow.AddMinutes(-30),
            ExitTime = null,
            ParkingFee = 10m,
            IsPaid = false,
            IsActive = true,
            ParkingGateId = gate.Id,
            ParkingGate = gate,
            ParkingTariffId = tariff.Id,
            ParkingTariff = tariff
        };

        _context.ParkingSessions.Add(session);

        await _context.SaveChangesAsync();
    }
}
