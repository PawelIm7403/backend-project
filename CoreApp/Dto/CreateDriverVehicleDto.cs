namespace CoreApp.Dto;

public record CreateDriverVehicleDto
{
    public string LicensePlate { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
}