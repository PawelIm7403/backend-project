using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Exceptions;
using CoreApp.Mappers;
using CoreApp.Repositories;
using CoreApp.Services;

namespace CoreApp.Services;

public class DriverService : IDriverService
{
    private readonly IParkingUnitOfWork _unit;

    public DriverService(IParkingUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task<DriverVehicleDto> RegisterVehicle(
        CreateDriverVehicleDto dto,
        string userId)
    {
        var vehicle = dto.ToEntity(userId);

        vehicle.Id = Guid.NewGuid();

        await _unit.DriverVehicles.AddAsync(vehicle);
        await _unit.SaveChangesAsync();

        return vehicle.ToDto();
    }

    public async Task<IEnumerable<DriverVehicleDto>> GetMyVehicles(
        string userId)
    {
        var vehicles =
            await _unit.DriverVehicles.GetByUserIdAsync(userId);

        return vehicles.Select(v => v.ToDto());
    }
    
    public async Task<IEnumerable<ParkingSessionHistoryDto>> GetVehicleSessions(
        string licensePlate,
        string userId)
    {
        var driverVehicle = await _unit.DriverVehicles.GetByLicensePlateAsync(licensePlate);

        if (driverVehicle is null || driverVehicle.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can view only sessions of your own vehicles.");
        }

        var sessions = await _unit.Sessions.GetHistoryByLicensePlateAsync(licensePlate);

        return sessions.Select(s => s.ToHistoryDto());
    }
    
    public async Task<ParkingSessionHistoryDto?> GetCurrentVehicleSession(
        string licensePlate,
        string userId)
    {
        var driverVehicle = await _unit.DriverVehicles.GetByLicensePlateAsync(licensePlate);

        if (driverVehicle is null || driverVehicle.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can view only sessions of your own vehicles.");
        }

        var session = await _unit.Sessions.FindActiveSessionByLicensePlateAsync(licensePlate);

        return session?.ToHistoryDto();
    }
    
    public async Task<IEnumerable<ParkingSessionHistoryDto>> GetPublicVehicleSessions(
        string licensePlate)
    {
        var sessions = await _unit.Sessions.GetHistoryByLicensePlateAsync(licensePlate);

        return sessions.Select(s => s.ToHistoryDto());
    }

    public async Task<ParkingSessionHistoryDto?> GetPublicCurrentVehicleSession(
        string licensePlate)
    {
        var session = await _unit.Sessions.FindActiveSessionByLicensePlateAsync(licensePlate);

        return session?.ToHistoryDto();
    }
    
    public async Task<PaymentResultDto> PayCurrentSession(string licensePlate)
    {
        var session = await _unit.Sessions.FindActiveSessionByLicensePlateAsync(licensePlate);

        if (session is null)
        {
            throw new BusinessRuleException($"Active session for vehicle {licensePlate} not found.");
        }

        if (session.IsPaid)
        {
            return new PaymentResultDto(
                session.Id,
                licensePlate,
                session.ParkingFee ?? 0,
                session.IsPaid,
                DateTime.UtcNow
            );
        }

        session.IsPaid = true;

        await _unit.Sessions.UpdateAsync(session);
        await _unit.SaveChangesAsync();

        return new PaymentResultDto(
            session.Id,
            licensePlate,
            session.ParkingFee ?? 0,
            session.IsPaid,
            DateTime.UtcNow
        );
    }
    
    public async Task<PaymentResultDto> PayMyCurrentSession(
        string licensePlate,
        string userId)
    {
        return await PayMyCurrentSessionFromAccount(licensePlate, userId);
    }
    
    public async Task<DriverAccountDto> GetAccount(string userId)
    {
        var account = await _unit.DriverAccounts.GetByUserIdAsync(userId);

        if (account is null)
        {
            account = new DriverAccount
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Balance = 0m
            };

            await _unit.DriverAccounts.AddAsync(account);
            await _unit.SaveChangesAsync();
        }

        return new DriverAccountDto(account.Balance);
    }
    
    public async Task<DriverAccountDto> TopUpAccount(
        string userId,
        TopUpAccountDto dto)
    {
        if (dto.Amount <= 0)
        {
            throw new BusinessRuleException("Amount must be greater than zero.");
        }

        var account = await _unit.DriverAccounts.GetByUserIdAsync(userId);
        var isNewAccount = account is null;

        if (account is null)
        {
            account = new DriverAccount
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Balance = 0m
            };

            await _unit.DriverAccounts.AddAsync(account);
        }

        account.Balance += dto.Amount;

        if (!isNewAccount)
        {
            await _unit.DriverAccounts.UpdateAsync(account);
        }

        await _unit.SaveChangesAsync();

        return new DriverAccountDto(account.Balance);
    }
    
    public async Task<PaymentResultDto> PayMyCurrentSessionFromAccount(
        string licensePlate,
        string userId)
    {
        var driverVehicle = await _unit.DriverVehicles.GetByLicensePlateAsync(licensePlate);

        if (driverVehicle is null || driverVehicle.UserId != userId)
        {
            throw new UnauthorizedAccessException("You can pay only for your own vehicles.");
        }

        var session = await _unit.Sessions.FindActiveSessionByLicensePlateAsync(licensePlate);

        if (session is null)
        {
            throw new BusinessRuleException($"Active session for vehicle {licensePlate} not found.");
        }

        if (session.IsPaid)
        {
            throw new BusinessRuleException("Session already paid.");
        }

        var amount = session.ParkingFee ?? 0;

        var registrationBonus = await _unit.DriverDiscounts.GetActiveByUserIdAndTypeAsync(
            userId,
            DriverDiscountType.RegistrationBonus);

        if (registrationBonus?.ExtraFreeMinutes is not null)
        {
            var duration = DateTime.UtcNow - session.EntryTime;

            if (duration.TotalMinutes <= registrationBonus.ExtraFreeMinutes.Value)
            {
                amount = 0;
            }
        }

        var loyaltyDiscount = await _unit.DriverDiscounts.GetActiveByUserIdAndTypeAsync(
            userId,
            DriverDiscountType.LoyaltyDiscount);

        if (amount > 0 && loyaltyDiscount?.PercentageDiscount is not null)
        {
            amount -= amount * (loyaltyDiscount.PercentageDiscount.Value / 100);
        }

        if (amount < 0)
        {
            amount = 0;
        }

        if (amount > 0)
        {
            var account = await _unit.DriverAccounts.GetByUserIdAsync(userId);

            if (account is null)
            {
                throw new BusinessRuleException("Driver account not found.");
            }

            if (account.Balance < amount)
            {
                throw new BusinessRuleException("Not enough funds on driver account.");
            }

            account.Balance -= amount;
            await _unit.DriverAccounts.UpdateAsync(account);
        }

        session.ParkingFee = amount;
        session.IsPaid = true;

        await _unit.Sessions.UpdateAsync(session);
        await _unit.SaveChangesAsync();

        return new PaymentResultDto(
            session.Id,
            licensePlate,
            amount,
            session.IsPaid,
            DateTime.UtcNow
        );
    }
    
    public async Task<IEnumerable<DriverDiscountDto>> GetMyDiscounts(string userId)
{
    var discounts = await _unit.DriverDiscounts.GetByUserIdAsync(userId);

    return discounts.Select(d => d.ToDto());
}

public async Task<DriverDiscountDto> ActivateRegistrationBonus(string userId)
{
    var active = await _unit.DriverDiscounts.GetActiveByUserIdAndTypeAsync(
        userId,
        DriverDiscountType.RegistrationBonus);

    if (active is not null)
    {
        throw new BusinessRuleException("Registration bonus is already active.");
    }

    var discount = new DriverDiscount
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Type = DriverDiscountType.RegistrationBonus,
        IsActive = true,
        ActivatedAt = DateTime.UtcNow,
        ExpiresAt = DateTime.UtcNow.AddDays(30),
        ExtraFreeMinutes = 60,
        PercentageDiscount = null
    };

    await _unit.DriverDiscounts.AddAsync(discount);
    await _unit.SaveChangesAsync();

    return discount.ToDto();
}

public async Task<DriverDiscountDto> ActivateLoyaltyDiscount(string userId)
{
    var sessions = await _unit.Sessions.FindAllAsync();

    var userVehicles = await _unit.DriverVehicles.GetByUserIdAsync(userId);
    var licensePlates = userVehicles
        .Select(v => v.LicensePlate)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    var userSessionCount = sessions
        .Count(s => s.Vehicle is not null &&
                    licensePlates.Contains(s.Vehicle.LicensePlate));

    if (userSessionCount < 100)
    {
        throw new InvalidOperationException("Loyalty discount requires at least 100 parking sessions.");
    }

    var active = await _unit.DriverDiscounts.GetActiveByUserIdAndTypeAsync(
        userId,
        DriverDiscountType.LoyaltyDiscount);

    if (active is not null)
    {
        throw new BusinessRuleException("Loyalty discount is already active.");
    }

    var discount = new DriverDiscount
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Type = DriverDiscountType.LoyaltyDiscount,
        IsActive = true,
        ActivatedAt = DateTime.UtcNow,
        ExpiresAt = DateTime.UtcNow.AddDays(30),
        ExtraFreeMinutes = null,
        PercentageDiscount = 20m
    };

    await _unit.DriverDiscounts.AddAsync(discount);
    await _unit.SaveChangesAsync();

    return discount.ToDto();
}
}
