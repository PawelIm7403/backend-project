namespace CoreApp.Entities;

public class DriverVehicle : EntityBase
{
    public string UserId { get; set; } = string.Empty;

    public string LicensePlate { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}