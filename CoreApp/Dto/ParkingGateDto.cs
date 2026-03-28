using CoreApp.Entities;
using CoreApp.Enums;

namespace CoreApp.Dto;

public record ParkingGateDto(
    Guid Id,
    string Name,
    string Type,
    string Location,
    bool IsOperational
)
{
    public ParkingGate ToEntity(ParkingGateDto dto)
    {
        return new ParkingGate
        {
            Id = dto.Id,
            Name = dto.Name,
            Type = Enum.Parse<GateType>(dto.Type),
            Location = dto.Location,
            IsOperational = dto.IsOperational
        };
    }
};
