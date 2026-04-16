using CoreApp.Repositories;
using Infrastructure.Memory;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 🔹 kontrolery REST
        builder.Services.AddControllers();

        // 🔹 OpenAPI
        builder.Services.AddOpenApi();

        // 🔹 REPOZYTORIA (Singleton)
        builder.Services.AddSingleton<IParkingGateRepository, MemoryParkingGateRepository>();
        builder.Services.AddSingleton<IVehicleRepository, MemoryVehicleRepository>();
        builder.Services.AddSingleton<IParkingSessionRepository, MemoryParkingSessionRepository>();

        // 🔹 UnitOfWork
        builder.Services.AddSingleton<IParkingUnitOfWork, MemoryParkingUnitOfWork>();

        // 🔹 Service
        builder.Services.AddSingleton<IParkingGateService, MemoryParkingGateService>();

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