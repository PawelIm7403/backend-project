namespace CoreApp.Dto;

public record ParkingSessionDto(
    Guid Id,
    Guid VehicleId,
    string GateName,
    DateTime EntryTime,
    DateTime? ExitTime,
    decimal? ParkingFee,
    bool IsActive,
    Guid ParkingGateId,
    Guid ParkingTariffId
);