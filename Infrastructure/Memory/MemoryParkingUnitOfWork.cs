using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryParkingUnitOfWork(
    IVehicleRepository vehicles,
    IParkingSessionRepository sessions,
    IParkingGateRepository gates,
    ICameraCaptureRepository cameraCaptures,
    IDriverVehicleRepository driverVehicles,
    IDriverAccountRepository driverAccounts,
    IDriverDiscountRepository driverDiscounts
    // pozostałe repozytoria
): IParkingUnitOfWork
{
    public IVehicleRepository Vehicles => vehicles;
    public IParkingGateRepository Gates => gates;
    public IParkingSessionRepository Sessions => sessions;
    public ICameraCaptureRepository CameraCaptures => cameraCaptures;
    
    public IDriverVehicleRepository DriverVehicles => driverVehicles;
    
    public IDriverAccountRepository DriverAccounts => driverAccounts;
    
    public IDriverDiscountRepository DriverDiscounts => driverDiscounts;
    // pozostałe repozytoria
    
    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }
}