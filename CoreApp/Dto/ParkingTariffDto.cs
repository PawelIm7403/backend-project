namespace CoreApp.Dto;

public record ParkingTariffDto(
    Guid Id,
    string Name,
    TimeSpan FreeParkingDuration,
    decimal HourlyRate,
    decimal DailyMaxRate,
    bool IsActive
);