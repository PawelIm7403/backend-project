namespace CoreApp.Repositories;

public interface IParkingUnitOfWork
{
    IVehicleRepository Vehicles { get; }
    IParkingGateRepository Gates { get; }
    IParkingSessionRepository Sessions { get; }
    
    ICameraCaptureRepository CameraCaptures { get; }
    
    IDriverVehicleRepository DriverVehicles { get; }
    
    IDriverAccountRepository DriverAccounts { get; }
    
    IDriverDiscountRepository DriverDiscounts { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}