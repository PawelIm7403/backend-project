using CoreApp.Dto;
using CoreApp.Entities;

namespace CoreApp.Mappers;

public static class CameraCaptureMapper
{
    public static CameraCaptureDto ToDto(this CameraCapture entity)
    {
        return new CameraCaptureDto(
            entity.LicensePlate,
            entity.DetectedBrand,
            entity.DetectedColor,
            entity.GateName,
            entity.ImagePath
        );
    }

    public static CameraCapture ToEntity(this CreateCameraCaptureDto dto)
    {
        return new CameraCapture
        {
            LicensePlate = dto.LicensePlate,
            DetectedBrand = dto.Brand,
            DetectedColor = dto.Color,
            ImagePath = dto.ImagePath ?? string.Empty,
            CapturedAt = DateTime.UtcNow
        };
    }
}