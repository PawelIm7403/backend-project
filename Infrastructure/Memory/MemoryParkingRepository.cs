using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class InMemoryParkingSessionRepository 
    : MemoryGenericRepository<ParkingSession>, IParkingSessionRepository
{
    public Task<ParkingSession?> FindActiveSessionByLicensePlateAsync(string licensePlate)
    {
        var session = _data.Values
            .FirstOrDefault(s => s.Vehicle.LicensePlate == licensePlate && s.IsActive);

        return Task.FromResult(session);
    }

    public Task<IEnumerable<ParkingSession>> GetActiveSessionsAsync()
    {
        var result = _data.Values
            .Where(s => s.IsActive);

        return Task.FromResult(result);
    }

    public Task<IEnumerable<ParkingSession>> GetHistoryByLicensePlateAsync(string licensePlate)
    {
        var result = _data.Values
            .Where(s => s.Vehicle.LicensePlate == licensePlate);

        return Task.FromResult(result);
    }
}