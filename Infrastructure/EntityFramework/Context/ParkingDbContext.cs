using CoreApp.Entities;
using CoreApp.Enums;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Context;

public class ParkingDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<ParkingGate> ParkingGates { get; set; }
    public DbSet<ParkingSession> ParkingSessions { get; set; }
    public DbSet<ParkingTariff> ParkingTariffs { get; set; }
    public DbSet<CameraCapture> CameraCaptures { get; set; }

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
                .WithMany()
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.ParkingGate)
                .WithMany()
                .HasForeignKey(s => s.ParkingGateId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.ParkingTariff)
                .WithMany()
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

        var adminRoleId = "11111111-1111-1111-1111-111111111111";
        var registeredRoleId = "22222222-2222-2222-2222-222222222222";
        var anonymousRoleId = "33333333-3333-3333-3333-333333333333";

        builder.Entity<AppRole>().HasData(
            new AppRole(UserRole.Administrator.ToString(), "System administrator")
            {
                Id = adminRoleId,
                NormalizedName = UserRole.Administrator.ToString().ToUpper()
            },
            new AppRole(UserRole.RegisteredUser.ToString(), "Registered parking user")
            {
                Id = registeredRoleId,
                NormalizedName = UserRole.RegisteredUser.ToString().ToUpper()
            },
            new AppRole(UserRole.AnonymousUser.ToString(), "Anonymous parking user")
            {
                Id = anonymousRoleId,
                NormalizedName = UserRole.AnonymousUser.ToString().ToUpper()
            }
        );

        var adminUserId = "44444444-4444-4444-4444-444444444444";
        var userId = "55555555-5555-5555-5555-555555555555";

        var passwordHasher = new PasswordHasher<AppUser>();

        var admin = new AppUser
        {
            Id = adminUserId,
            UserName = "admin@parking.local",
            NormalizedUserName = "ADMIN@PARKING.LOCAL",
            Email = "admin@parking.local",
            NormalizedEmail = "ADMIN@PARKING.LOCAL",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "Parking",
            FullName = "Admin Parking",
            Department = "Administration",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2026, 1, 1)
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123!");

        var user = new AppUser
        {
            Id = userId,
            UserName = "user@parking.local",
            NormalizedUserName = "USER@PARKING.LOCAL",
            Email = "user@parking.local",
            NormalizedEmail = "USER@PARKING.LOCAL",
            EmailConfirmed = true,
            FirstName = "Jan",
            LastName = "Kowalski",
            FullName = "Jan Kowalski",
            Department = "Parking",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2026, 1, 1)
        };

        user.PasswordHash = passwordHasher.HashPassword(user, "User123!");

        builder.Entity<AppUser>().HasData(admin, user);

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = adminUserId,
                RoleId = adminRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = userId,
                RoleId = registeredRoleId
            }
        );
    }
}