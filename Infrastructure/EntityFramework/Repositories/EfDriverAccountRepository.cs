using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfDriverAccountRepository(
    ParkingDbContext context)
    : EfGenericRepository<DriverAccount>(context.DriverAccounts),
        IDriverAccountRepository
{
    public async Task<DriverAccount?> GetByUserIdAsync(
        string userId)
    {
        return await context.DriverAccounts
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}