namespace CoreApp.Entities;

public class Vehicle : EntityBase
{
    public string LicensePlate { get; set; }
    public string Brand  { get; set; }
    public string Color { get; set; }
    
    public ICollection<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();
}