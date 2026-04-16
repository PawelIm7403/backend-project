using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryParkingSessionRepository 
    : MemoryGenericRepository<ParkingSession>, IParkingSessionRepository
{
    public Task<ParkingSession?> FindActiveSessionByLicensePlateAsync(string licensePlate)
    {
        var result = _data.Values.FirstOrDefault(s =>
            s.IsActive &&
            s.Vehicle != null &&
            s.Vehicle.LicensePlate == licensePlate);

        return Task.FromResult(result);
    }

    public Task<IEnumerable<ParkingSession>> GetActiveSessionsAsync()
    {
        var result = _data.Values.Where(s => s.IsActive);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<ParkingSession>> GetHistoryByLicensePlateAsync(string licensePlate)
    {
        var result = _data.Values.Where(s =>
            s.Vehicle != null &&
            s.Vehicle.LicensePlate == licensePlate);

        return Task.FromResult(result);
    }
}