using CoreApp.Entities;

namespace CoreApp.Repositories;

public interface IDriverAccountRepository
    : IGenericRepositoryAsync<DriverAccount>
{
    Task<DriverAccount?> GetByUserIdAsync(string userId);
}