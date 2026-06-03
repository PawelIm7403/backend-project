using CoreApp.Module;
using CoreApp.Repositories;
using CoreApp.Services;
using Infrastructure.Memory;
using WebApi.WebApi;
using Infrastructure.EntityFramework;

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
        
       //(Pamięć) builder.Services.AddParkingMemoryModule();
        builder.Services.AddParkingEfModule(builder.Configuration); //(EF)
        builder.Services.AddCoreAppModule(builder.Configuration);
        
        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();

        
        //builder.Services.AddSingleton<IParkingGateRepository, MemoryParkingGateRepository>();
        //builder.Services.AddSingleton<IVehicleRepository, MemoryVehicleRepository>();
       // builder.Services.AddSingleton<IParkingSessionRepository, MemoryParkingSessionRepository>();

       
       // builder.Services.AddSingleton<IParkingUnitOfWork, MemoryParkingUnitOfWork>(); // UnitOfWork

        
      //  builder.Services.AddSingleton<IParkingGateService, ParkingGateService>(); // Service
       // builder.Services.AddSingleton<ICameraCaptureRepository, MemoryCameraCaptureRepository>();
       
       
        
        

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        
        app.UseExceptionHandler();
        
        app.MapControllers();
        app.Run();
    }
}