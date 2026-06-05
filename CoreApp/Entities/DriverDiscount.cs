using CoreApp.Enums;

namespace CoreApp.Entities;

public class DriverDiscount : EntityBase
{
    public string UserId { get; set; } = string.Empty;

    public DriverDiscountType Type { get; set; }

    public bool IsActive { get; set; }

    public DateTime ActivatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public int? ExtraFreeMinutes { get; set; }

    public decimal? PercentageDiscount { get; set; }
}