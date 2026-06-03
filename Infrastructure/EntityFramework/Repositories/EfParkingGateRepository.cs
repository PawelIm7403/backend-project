using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfParkingGateRepository(ParkingDbContext context)
    : EfGenericRepository<ParkingGate>(context.ParkingGates), IParkingGateRepository
{
    public async Task<ParkingGate?> FindByNameAsync(string name)
    {
        return await context.ParkingGates
            .FirstOrDefaultAsync(g => g.Name == name);
    }
}