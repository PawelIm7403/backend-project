using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfDriverVehicleRepository(
    ParkingDbContext context)
    : EfGenericRepository<DriverVehicle>(context.DriverVehicles),
        IDriverVehicleRepository
{
    public async Task<IEnumerable<DriverVehicle>> GetByUserIdAsync(
        string userId)
    {
        return await context.DriverVehicles
            .Where(v => v.UserId == userId)
            .ToListAsync();
    }

    public async Task<DriverVehicle?> GetByLicensePlateAsync(
        string licensePlate)
    {
        return await context.DriverVehicles
            .FirstOrDefaultAsync(v =>
                v.LicensePlate == licensePlate);
    }
}