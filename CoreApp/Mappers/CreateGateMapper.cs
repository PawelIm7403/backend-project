using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Mappers;

public static class CreateGateMapper
{
    public static ParkingGate ToEntity(this CreateGateDto dto)
    {
        return new ParkingGate
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Type = dto.Type,
            Location = dto.Location,
            IsOperational = false
        };
    }
}