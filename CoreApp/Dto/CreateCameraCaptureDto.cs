namespace CoreApp.Dto;

public record CreateCameraCaptureDto(
    string LicensePlate,
    string Brand,
    string Color,
    string? ImagePath = null
);