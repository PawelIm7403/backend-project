using CoreApp.Repositories;
using CoreApp.Services;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Entities;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Memory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Security;
using CoreApp.Authorization;
using CoreApp.Enums;
using Infrastructure.EntityFramework.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.EntityFramework;

public static class ParkingInfrastructureModule
{
    public static IServiceCollection AddParkingEfModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ParkingDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("ParkingDb")));

        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<ParkingDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IParkingGateRepository, EfParkingGateRepository>();
        services.AddScoped<IVehicleRepository, EfVehicleRepository>();
        services.AddScoped<IParkingSessionRepository, EfParkingSessionRepository>();
        services.AddScoped<ICameraCaptureRepository, EfCameraCaptureRepository>();

        services.AddScoped<IParkingUnitOfWork, EfParkingUnitOfWork>();

        services.AddScoped<IParkingGateService, ParkingGateService>();
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<JwtSettings>();
        
        services.AddScoped<IDataSeeder, IdentityDbSeeder>();
        services.AddScoped<IDataSeeder, ParkingDbSeeder>();

        return services;
        
    }

    public static IServiceCollection AddParkingMemoryModule(
        this IServiceCollection services)
    {
        services.AddSingleton<IParkingGateRepository, MemoryParkingGateRepository>();
        services.AddSingleton<IVehicleRepository, MemoryVehicleRepository>();
        services.AddSingleton<IParkingSessionRepository, MemoryParkingSessionRepository>();
        services.AddSingleton<ICameraCaptureRepository, MemoryCameraCaptureRepository>();

        services.AddSingleton<IParkingUnitOfWork, MemoryParkingUnitOfWork>();

        services.AddSingleton<IParkingGateService, ParkingGateService>();

        return services;
    }
    
    public static IServiceCollection AddJwt(
        this IServiceCollection services,
        JwtSettings jwtOptions)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = jwtOptions.GetSymmetricKey(),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicies.AdminOnly.ToString(), policy =>
                policy.RequireRole(UserRole.Administrator.ToString()));

            options.AddPolicy(AppPolicies.RegisteredUserOnly.ToString(), policy =>
                policy.RequireRole(UserRole.RegisteredUser.ToString()));

            options.AddPolicy(AppPolicies.AnonymousUserOnly.ToString(), policy =>
                policy.RequireRole(UserRole.AnonymousUser.ToString()));

            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}