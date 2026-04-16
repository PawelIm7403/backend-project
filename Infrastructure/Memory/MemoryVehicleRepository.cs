using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryVehicleRepository 
    : MemoryGenericRepository<Vehicle>, IVehicleRepository
{
    public Task<Vehicle?> FindByLicensePlateAsync(string licensePlate)
    {
        var result = _data.Values
            .FirstOrDefault(v => v.LicensePlate == licensePlate);

        return Task.FromResult(result);
    }
}