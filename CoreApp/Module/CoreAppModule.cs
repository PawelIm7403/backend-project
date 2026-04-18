using CoreApp.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApp.Module;

public static class CoreAppModule
{
    public static IServiceCollection AddCoreAppModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<ParkingGateValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateTariffValidator>();
        services.AddValidatorsFromAssemblyContaining<CameraCaptureValidator>();
        
        services.AddFluentValidationAutoValidation();

        return services;
    }
}