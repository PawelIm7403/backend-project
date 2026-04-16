using CoreApp.Entities;
using CoreApp.Enums;

namespace CoreApp.Dto;

public record ParkingGateDto(
    Guid Id,
    string Name,
    string Type,
    string Location,
    bool IsOperational
);

    
