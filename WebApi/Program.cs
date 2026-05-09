using CoreApp.Module;
using CoreApp.Repositories;
using Infrastructure.Memory;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthorization();
        builder.Services.AddCoreAppModule(builder.Configuration); // Moduł z walidatorami
        
        builder.Services.AddControllers(); // kontrolery REST
        
        builder.Services.AddOpenApi(); // OpenAPI

        // REPOZYTORIA (Singleton)
        builder.Services.AddSingleton<IParkingGateRepository, MemoryParkingGateRepository>();
        builder.Services.AddSingleton<IVehicleRepository, MemoryVehicleRepository>();
        builder.Services.AddSingleton<IParkingSessionRepository, MemoryParkingSessionRepository>();

       
        builder.Services.AddSingleton<IParkingUnitOfWork, MemoryParkingUnitOfWork>(); // UnitOfWork

        
        builder.Services.AddSingleton<IParkingGateService, MemoryParkingGateService>(); // Service
        builder.Services.AddSingleton<ICameraCaptureRepository, MemoryCameraCaptureRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}