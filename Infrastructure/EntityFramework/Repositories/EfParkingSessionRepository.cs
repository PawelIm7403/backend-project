using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfParkingSessionRepository(ParkingDbContext context)
    : EfGenericRepository<ParkingSession>(context.ParkingSessions), IParkingSessionRepository
{
    public async Task<ParkingSession?> FindActiveSessionByLicensePlateAsync(string licensePlate)
    {
        return await context.ParkingSessions
            .Include(s => s.Vehicle)
            .FirstOrDefaultAsync(s =>
                s.IsActive &&
                s.Vehicle.LicensePlate == licensePlate);
    }

    public async Task<IEnumerable<ParkingSession>> GetActiveSessionsAsync()
    {
        return await context.ParkingSessions
            .Where(s => s.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<ParkingSession>> GetHistoryByLicensePlateAsync(string licensePlate)
    {
        return await context.ParkingSessions
            .Include(s => s.Vehicle)
            .Where(s => s.Vehicle.LicensePlate == licensePlate)
            .ToListAsync();
    }
}