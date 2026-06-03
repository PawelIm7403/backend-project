using CoreApp.Enums;

namespace CoreApp.Entities;

public class ParkingGate : EntityBase
{
    public string Name { get; set; }
    public GateType Type { get; set; }
    public string Location { get; set; }
    public bool IsOperational { get; set; }

    public ICollection<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();

    public ICollection<CameraCapture> CameraCaptures { get; set; } = new List<CameraCapture>();
}