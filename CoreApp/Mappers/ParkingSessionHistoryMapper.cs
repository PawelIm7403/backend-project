using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Mappers;

public static class ParkingSessionHistoryMapper
{
    public static ParkingSessionHistoryDto ToHistoryDto(this ParkingSession entity)
    {
        return new ParkingSessionHistoryDto(
            entity.Id,
            entity.Vehicle.ToDto(),
            entity.GateName,
            entity.EntryTime,
            entity.ExitTime,
            entity.ExitTime.HasValue
                ? entity.ExitTime.Value - entity.EntryTime
                : null,
            entity.ParkingFee,
            entity.IsActive
        );
    }
}