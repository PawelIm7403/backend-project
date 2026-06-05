namespace CoreApp.Entities;

public class DriverAccount : EntityBase
{
    public string UserId { get; set; } = string.Empty;

    public decimal Balance { get; set; }
}