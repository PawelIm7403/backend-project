using CoreApp.Entities;

namespace CoreApp.Dto;

public record CreateTariffDto(
    string Name,
    int FreeMinutes,
    decimal HourlyRate,
    decimal DailyMaxRate
)
{
    public ParkingTariff ToEntity()
    {
        return new ParkingTariff(){
            Name = Name,
            FreeParkingDuration = TimeSpan.FromMinutes(FreeMinutes),
            HourlyRate = HourlyRate,
            DailyMaxRate = DailyMaxRate,
            IsActive = false,
        };
    }
};