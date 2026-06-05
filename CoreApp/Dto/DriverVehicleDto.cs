namespace CoreApp.Dto;

public record DriverVehicleDto
{
    public Guid Id { get; init; }

    public string LicensePlate { get; init; } = string.Empty;

    public string Brand { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}