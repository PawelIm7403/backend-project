using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Mappers;

public static class ParkingSessionMapper
{
    public static ParkingSessionDto ToDto(this ParkingSession entity)
    {
        return new ParkingSessionDto(
            entity.Id,
            entity.VehicleId,
            entity.GateName,
            entity.EntryTime,
            entity.ExitTime,
            entity.ParkingFee,
            entity.IsActive,
            entity.ParkingGateId,
            entity.ParkingTariffId
        );
    }

    public static ParkingSession ToEntity(this ParkingSessionDto dto)
    {
        return new ParkingSession
        {
            Id = dto.Id,
            VehicleId = dto.VehicleId,
            GateName = dto.GateName,
            EntryTime = dto.EntryTime,
            ExitTime = dto.ExitTime,
            ParkingFee = dto.ParkingFee,
            IsActive = dto.IsActive,
            ParkingGateId = dto.ParkingGateId,
            ParkingTariffId = dto.ParkingTariffId
        };
    }
}