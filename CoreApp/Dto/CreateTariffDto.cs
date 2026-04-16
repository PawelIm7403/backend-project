using CoreApp.Entities;

namespace CoreApp.Dto;

public record CreateTariffDto(
    string Name,
    int FreeMinutes,
    decimal HourlyRate,
    decimal DailyMaxRate
);