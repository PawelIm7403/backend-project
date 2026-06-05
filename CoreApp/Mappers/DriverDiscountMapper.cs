using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Mappers;

public static class DriverDiscountMapper
{
    public static DriverDiscountDto ToDto(this DriverDiscount entity)
    {
        return new DriverDiscountDto(
            entity.Id,
            entity.Type,
            entity.IsActive,
            entity.ActivatedAt,
            entity.ExpiresAt,
            entity.ExtraFreeMinutes,
            entity.PercentageDiscount
        );
    }
}