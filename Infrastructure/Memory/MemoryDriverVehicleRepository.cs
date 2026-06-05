using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryDriverVehicleRepository
    : MemoryGenericRepository<DriverVehicle>,
        IDriverVehicleRepository
{
    public Task<IEnumerable<DriverVehicle>> GetByUserIdAsync(string userId)
    {
        var vehicles = _data.Values
            .Where(v => v.UserId == userId);

        return Task.FromResult(vehicles.AsEnumerable());
    }

    public Task<DriverVehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        var vehicle = _data.Values
            .FirstOrDefault(v =>
                v.LicensePlate.Equals(
                    licensePlate,
                    StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(vehicle);
    }
}