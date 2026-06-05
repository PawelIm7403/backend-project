using CoreApp.Entities;

namespace CoreApp.Repositories;

public interface IDriverVehicleRepository
    : IGenericRepositoryAsync<DriverVehicle>
{
    Task<IEnumerable<DriverVehicle>> GetByUserIdAsync(string userId);

    Task<DriverVehicle?> GetByLicensePlateAsync(
        string licensePlate);
}