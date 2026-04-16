using CoreApp.Enums;

namespace CoreApp.Dto;

public record CreateGateDto(
    string Name,
    GateType Type,
    string Location
);