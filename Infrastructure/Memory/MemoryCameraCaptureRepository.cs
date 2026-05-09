using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryCameraCaptureRepository 
    : MemoryGenericRepository<CameraCapture>, ICameraCaptureRepository
{
    public Task<IEnumerable<CameraCapture>> FindByGateIdAsync(Guid gateId)
    {
        var result = _data.Values
            .Where(x => x.ParkingGateId == gateId);

        return Task.FromResult(result);
    }
}