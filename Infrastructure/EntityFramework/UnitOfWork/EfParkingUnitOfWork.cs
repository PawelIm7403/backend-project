using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.UnitOfWork;

public class EfParkingUnitOfWork(
    IVehicleRepository vehicles,
    IParkingGateRepository gates,
    IParkingSessionRepository sessions,
    ICameraCaptureRepository cameraCaptures,
    ParkingDbContext context,
    IDriverVehicleRepository driverVehicles,
    IDriverAccountRepository driverAccounts,
    IDriverDiscountRepository driverDiscounts
) : IParkingUnitOfWork, IAsyncDisposable
{
    public IVehicleRepository Vehicles => vehicles;

    public IParkingGateRepository Gates => gates;

    public IParkingSessionRepository Sessions => sessions;

    public ICameraCaptureRepository CameraCaptures => cameraCaptures;
    
    public IDriverVehicleRepository DriverVehicles => driverVehicles;
    
    public IDriverAccountRepository DriverAccounts => driverAccounts;
    
    public IDriverDiscountRepository DriverDiscounts => driverDiscounts;

    public ValueTask DisposeAsync()
    {
        return context.DisposeAsync();
    }

    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public Task BeginTransactionAsync()
    {
        return context.Database.BeginTransactionAsync();
    }

    public Task CommitTransactionAsync()
    {
        return context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return context.Database.RollbackTransactionAsync();
    }
}