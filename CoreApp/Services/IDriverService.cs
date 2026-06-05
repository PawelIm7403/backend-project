using CoreApp.Dto;

namespace CoreApp.Services;

public interface IDriverService
{
    Task<DriverVehicleDto> RegisterVehicle(
        CreateDriverVehicleDto dto,
        string userId);

    Task<IEnumerable<DriverVehicleDto>> GetMyVehicles(
        string userId);
    
    Task<IEnumerable<ParkingSessionHistoryDto>> GetVehicleSessions(
        string licensePlate,
        string userId);
    
    Task<ParkingSessionHistoryDto?> GetCurrentVehicleSession(
        string licensePlate,
        string userId);
    
    Task<IEnumerable<ParkingSessionHistoryDto>> GetPublicVehicleSessions(
        string licensePlate);

    Task<ParkingSessionHistoryDto?> GetPublicCurrentVehicleSession(
        string licensePlate);
    
    Task<PaymentResultDto> PayCurrentSession(string licensePlate);

    Task<PaymentResultDto> PayMyCurrentSession(
        string licensePlate,
        string userId);
    
    Task<DriverAccountDto> TopUpAccount(
        string userId,
        TopUpAccountDto dto);

    Task<DriverAccountDto> GetAccount(
        string userId);
    
    Task<PaymentResultDto> PayMyCurrentSessionFromAccount(
        string licensePlate,
        string userId);
    
    Task<IEnumerable<DriverDiscountDto>> GetMyDiscounts(string userId);

    Task<DriverDiscountDto> ActivateRegistrationBonus(string userId);

    Task<DriverDiscountDto> ActivateLoyaltyDiscount(string userId);
}