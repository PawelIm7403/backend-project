using CoreApp.Dto;

namespace CoreApp.Repositories;

public interface IParkingGateServices
{
    Task<PagedResult<ParkingGateDto>> GetAll(int pageNumber, int pageSize);
    
    Task<ParkingGateDto?> GetById(Guid id);
    
    Task<ParkingGateDto?> GetByName(string name);
    
    Task Add(ParkingGateDto dto);
    
    Task UpdateOperationalStatus(Guid id, bool isOperational);
    

}