using CoreApp.Entities;
using CoreApp.Enums;

namespace CoreApp.Repositories;

public interface IDriverDiscountRepository : IGenericRepositoryAsync<DriverDiscount>
{
    Task<IEnumerable<DriverDiscount>> GetByUserIdAsync(string userId);

    Task<DriverDiscount?> GetActiveByUserIdAndTypeAsync(
        string userId,
        DriverDiscountType type);
}