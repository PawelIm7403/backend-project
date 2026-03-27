using CoreApp.Entities;

namespace CoreApp.Repositories;

public interface IParkingSessionRepository : IGenericRepositoryAsync<ParkingSession>
{
    Task<ParkingSession?> FindActiveSessionByLicensePlateAsync(string licensePlate);
    Task<IEnumerable<ParkingSession>> GetActiveSessionsAsync();
    Task<IEnumerable<ParkingSession>> GetHistoryByLicensePlateAsync(string licensePlate);
}