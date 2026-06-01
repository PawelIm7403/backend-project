using CoreApp.Dto;

namespace CoreApp.Repositories;

public interface IParkingGateService
{
    Task<PagedResult<ParkingGateDto>> GetAll(int pageNumber, int pageSize);
    
    Task<ParkingGateDto?> GetById(Guid id);
    
    Task<ParkingGateDto?> GetByName(string name);
    
    Task Add(ParkingGateDto dto);
    
    Task UpdateOperationalStatus(Guid id, bool isOperational);
    
    Task<CameraCaptureDto> AddCapture(Guid gateId, CreateCameraCaptureDto dto);
    
    Task<PagedResult<CameraCaptureDto>?> GetCaptures(Guid gateId, int page, int pageSize);
    
    Task DeleteCapture(Guid gateId, Guid captureId);

}