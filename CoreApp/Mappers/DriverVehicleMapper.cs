using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Mappers;

public static class DriverVehicleMapper
{
    public static DriverVehicle ToEntity(
        this CreateDriverVehicleDto dto,
        string userId)
    {
        return new DriverVehicle
        {
            UserId = userId,
            LicensePlate = dto.LicensePlate,
            Brand = dto.Brand
        };
    }

    public static DriverVehicleDto ToDto(
        this DriverVehicle vehicle)
    {
        return new DriverVehicleDto
        {
            Id = vehicle.Id,
            LicensePlate = vehicle.LicensePlate,
            Brand = vehicle.Brand,
            CreatedAt = vehicle.CreatedAt
        };
    }
}