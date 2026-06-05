using CoreApp.Entities;
using CoreApp.Enums;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Security;

namespace Infrastructure.EntityFramework.Context;

public class ParkingDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<ParkingGate> ParkingGates { get; set; }
    public DbSet<ParkingSession> ParkingSessions { get; set; }
    public DbSet<ParkingTariff> ParkingTariffs { get; set; }
    public DbSet<CameraCapture> CameraCaptures { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public DbSet<DriverVehicle> DriverVehicles { get; set; }
    
    public DbSet<DriverAccount> DriverAccounts { get; set; }
    
    public DbSet<DriverDiscount> DriverDiscounts { get; set; }

    public ParkingDbContext()
    {
    }

    public ParkingDbContext(DbContextOptions<ParkingDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=parking.db");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.FullName).HasMaxLength(200);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.Property(u => u.Status).HasConversion<string>();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        builder.Entity<AppRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(20);
        });

        builder.Entity<Vehicle>(entity =>
        {
            entity.Property(v => v.LicensePlate).HasMaxLength(20).IsRequired();
            entity.Property(v => v.Brand).HasMaxLength(50);
            entity.Property(v => v.Color).HasMaxLength(30);
            entity.HasIndex(v => v.LicensePlate).IsUnique();
        });

        builder.Entity<ParkingGate>(entity =>
        {
            entity.Property(g => g.Name).HasMaxLength(20).IsRequired();
            entity.Property(g => g.Type).HasConversion<string>();
            entity.Property(g => g.Location).HasMaxLength(50).IsRequired();
        });

        builder.Entity<ParkingTariff>(entity =>
        {
            entity.Property(t => t.Name).HasMaxLength(50).IsRequired();
            entity.Property(t => t.HourlyRate).HasColumnType("decimal(10,2)");
            entity.Property(t => t.DailyMaxRate).HasColumnType("decimal(10,2)");
        });

        builder.Entity<ParkingSession>(entity =>
        {
            entity.Property(s => s.GateName).HasMaxLength(50);
            entity.Property(s => s.ParkingFee).HasColumnType("decimal(10,2)");

            entity.HasOne(s => s.Vehicle)
                .WithMany(v => v.ParkingSessions)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.ParkingGate)
                .WithMany(g => g.ParkingSessions)
                .HasForeignKey(s => s.ParkingGateId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.ParkingTariff)
                .WithMany(t => t.ParkingSessions)
                .HasForeignKey(s => s.ParkingTariffId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<CameraCapture>(entity =>
        {
            entity.Property(c => c.GateName).HasMaxLength(50);
            entity.Property(c => c.LicensePlate).HasMaxLength(20).IsRequired();
            entity.Property(c => c.DetectedBrand).HasMaxLength(50);
            entity.Property(c => c.DetectedColor).HasMaxLength(30);
            entity.Property(c => c.ImagePath).HasMaxLength(255);
            entity.Property(c => c.Type).HasConversion<string>();

            entity.HasOne(c => c.ParkingGate)
                .WithMany(g => g.CameraCaptures)
                .HasForeignKey(c => c.ParkingGateId);
        });
        
        builder.Entity<DriverDiscount>(entity =>
        {
            entity.Property(d => d.Type).HasConversion<string>();
            entity.Property(d => d.PercentageDiscount).HasColumnType("decimal(5,2)");
        });
    }
}
