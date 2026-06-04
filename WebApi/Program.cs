using CoreApp.Module;
using CoreApp.Repositories;
using CoreApp.Services;
using Infrastructure.Memory;
using WebApi.WebApi;
using Infrastructure.EntityFramework;
using Infrastructure.Security;
using Infrastructure.EntityFramework.Seeders;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthorization();
        builder.Services.AddCoreAppModule(builder.Configuration); // Moduł z walidatorami
        
        builder.Services.AddControllers(); // kontrolery REST
        
        builder.Services.AddOpenApi(); // OpenAPI
        
       //(Pamięć) builder.Services.AddParkingMemoryModule();
        builder.Services.AddParkingEfModule(builder.Configuration); //(EF)
        builder.Services.AddCoreAppModule(builder.Configuration);
        
        builder.Services.AddSingleton<JwtSettings>();
        builder.Services.AddJwt(new JwtSettings(builder.Configuration));
        
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

            using var scope = app.Services.CreateScope();

            var seeders = scope.ServiceProvider
                .GetServices<IDataSeeder>()
                .OrderBy(seeder => seeder.Order);

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync();
            }
        }

        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseExceptionHandler();
        
        app.MapControllers();
        app.Run();
    }
}