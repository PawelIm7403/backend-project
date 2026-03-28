using CoreApp.Dto;
using CoreApp.Mappers;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryParkingGateService : IParkingGateServices
{
    private readonly IParkingUnitOfWork _unit;

    public MemoryParkingGateService(IParkingUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task<PagedResult<ParkingGateDto>> GetAll(int pageNumber, int pageSize)
    {
        var all = await _unit.Gates.GetAllAsync();

        var totalCount = all.Count();

        var items = all
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(g => g.ToDto()) // mapowanie
            .ToList();

        return new PagedResult<ParkingGateDto>(
            items,
            totalCount,
            pageNumber,
            pageSize
        );
    }

    public async Task<ParkingGateDto?> GetById(Guid id)
    {
        var entity = await _unit.Gates.FindByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<ParkingGateDto?> GetByName(string name)
    {
        var entity = await _unit.Gates.FindByNameAsync(name);
        return entity?.ToDto();
    }

    public async Task Add(ParkingGateDto dto)
    {
        var entity = dto.ToEntity();
        await _unit.Gates.AddAsync(entity);
        await _unit.SaveChangesAsync();
    }

    public async Task UpdateOperationalStatus(Guid id, bool isOperational)
    {
        var entity = await _unit.Gates.FindByIdAsync(id);
        if (entity is null)
            throw new Exception("Parking gate not found");

        entity.IsOperational = isOperational;
        await _unit.Gates.UpdateAsync(entity);
        await _unit.SaveChangesAsync();
    }
}