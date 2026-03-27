namespace CoreApp.Dto;

public record ActiveParkingSessionDto(
    Guid SessionId,
    VehicleDto Vehicle,
    string GateName,
    DateTime EntryTime,
    TimeSpan CurrentDuration
);