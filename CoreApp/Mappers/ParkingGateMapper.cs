using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Enums;

namespace CoreApp.Mappers;

public static class ParkingGateMapper
{
    // Entity -> DTO
    public static ParkingGateDto ToDto(this ParkingGate entity)
    {
        return new ParkingGateDto(
            entity.Id,
            entity.Name,
            entity.Type.ToString(), // enum -> string
            entity.Location,
            entity.IsOperational
        );
    }

    // DTO -> Entity
    public static ParkingGate ToEntity(this ParkingGateDto dto)
    {
        // parsowanie string -> GateType
        var type = Enum.Parse<GateType>(dto.Type, true);

        return new ParkingGate
        {
            Id = dto.Id,
            Name = dto.Name,
            Type = type,
            Location = dto.Location,
            IsOperational = dto.IsOperational
        };
    }
}