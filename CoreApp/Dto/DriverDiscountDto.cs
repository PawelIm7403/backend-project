using CoreApp.Enums;

namespace CoreApp.Dto;

public record DriverDiscountDto(
    Guid Id,
    DriverDiscountType Type,
    bool IsActive,
    DateTime ActivatedAt,
    DateTime ExpiresAt,
    int? ExtraFreeMinutes,
    decimal? PercentageDiscount
);