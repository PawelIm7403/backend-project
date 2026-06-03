using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfCameraCaptureRepository(ParkingDbContext context)
    : EfGenericRepository<CameraCapture>(context.CameraCaptures), ICameraCaptureRepository
{
    public async Task<IEnumerable<CameraCapture>> FindByGateIdAsync(Guid gateId)
    {
        return await context.CameraCaptures
            .Where(c => c.ParkingGateId == gateId)
            .ToListAsync();
    }
}