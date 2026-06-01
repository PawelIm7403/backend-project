namespace CoreApp.Dto;

public record CameraCaptureDto(
    Guid Id,
    string LicensePlate,
    string Brand,
    string Color,
    string GateName,
    string? ImagePath = null
);