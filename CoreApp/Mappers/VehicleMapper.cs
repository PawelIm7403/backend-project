using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Mappers;

public static class VehicleMapper
{
    public static VehicleDto ToDto(this Vehicle entity)
    {
        return new VehicleDto(
            entity.Id,
            entity.LicensePlate,
            entity.Brand,
            entity.Color
        );
    }

    public static Vehicle ToEntity(this VehicleDto dto)
    {
        return new Vehicle
        {
            Id = dto.Id,
            LicensePlate = dto.LicensePlate,
            Brand = dto.Brand,
            Color = dto.Color
        };
    }
}