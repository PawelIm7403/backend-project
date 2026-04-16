using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Mappers;

public static class ParkingTariffMapper
{
    public static ParkingTariff ToEntity(this CreateTariffDto dto)
    {
        return new ParkingTariff
        {
            Name = dto.Name,
            FreeParkingDuration = TimeSpan.FromMinutes(dto.FreeMinutes),
            HourlyRate = dto.HourlyRate,
            DailyMaxRate = dto.DailyMaxRate,
            IsActive = false
        };
    }

    public static ParkingTariffDto ToDto(this ParkingTariff entity)
    {
        return new ParkingTariffDto(
            entity.Id,
            entity.Name,
            entity.FreeParkingDuration,
            entity.HourlyRate,
            entity.DailyMaxRate,
            entity.IsActive
        );
    }
}