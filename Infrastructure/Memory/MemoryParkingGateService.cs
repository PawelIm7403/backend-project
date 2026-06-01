using CoreApp.Dto;
using CoreApp.Enums;
using CoreApp.Mappers;
using CoreApp.Repositories;
using CoreApp.Entities;
using CoreApp.Exceptions;

namespace Infrastructure.Memory;

public class MemoryParkingGateService : IParkingGateService
{
    private readonly IParkingUnitOfWork _unit;

    public MemoryParkingGateService(IParkingUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task<PagedResult<ParkingGateDto>> GetAll(int pageNumber, int pageSize)
    {
        var paged = await _unit.Gates.FindPagedAsync(pageNumber, pageSize);

        var items = paged.Items
            .Select(g => g.ToDto())
            .ToList();

        return new PagedResult<ParkingGateDto>(
            items,
            paged.TotalCount,
            paged.Page,
            paged.PageSize
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
        {
            throw new Exception("Parking gate not found");
        }

        entity.IsOperational = isOperational;

        await _unit.Gates.UpdateAsync(entity);
        await _unit.SaveChangesAsync();
    }
    
    public async Task<CameraCaptureDto> AddCapture(Guid gateId, CreateCameraCaptureDto dto)
    {
        var gate = await _unit.Gates.FindByIdAsync(gateId);

        if (gate is null)
        {
            throw new GateNotFoundException($"Gate with id={gateId} not found!");
        }

        var capture = dto.ToEntity();

        capture.Id = Guid.NewGuid();
        capture.ParkingGateId = gate.Id;
        capture.ParkingGate = gate;
        capture.GateName = gate.Name;
        capture.Type = gate.Type == CoreApp.Enums.GateType.Entry
            ? CoreApp.Enums.CaptureType.Entry
            : CoreApp.Enums.CaptureType.Exit;

        gate.CameraCaptures.Add(capture);

        await _unit.CameraCaptures.AddAsync(capture);
        await _unit.Gates.UpdateAsync(gate);
        await _unit.SaveChangesAsync();

        return capture.ToDto();
    }
    
    public async Task<PagedResult<CameraCaptureDto>?> GetCaptures(Guid gateId, int page, int pageSize)
    {
        var gate = await _unit.Gates.FindByIdAsync(gateId);

        if (gate is null)
        {
            return null;
        }

        var all = await _unit.CameraCaptures.FindByGateIdAsync(gateId);

        var total = all.Count();

        var items = all
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => x.ToDto())
            .ToList();

        return new PagedResult<CameraCaptureDto>(
            items,
            total,
            page,
            pageSize
        );
    }
    
    public async Task DeleteCapture(Guid gateId, Guid captureId)
    {
        var gate = await _unit.Gates.FindByIdAsync(gateId);

        if (gate is null)
        {
            throw new GateNotFoundException($"Gate with id={gateId} not found!");
        }

        var capture = gate.CameraCaptures
            .FirstOrDefault(x => x.Id == captureId);

        if (capture is null)
        {
            throw new Exception($"Camera capture with id={captureId} not found!");
        }

        gate.CameraCaptures.Remove(capture);

        await _unit.CameraCaptures.RemoveByIdAsync(captureId);
        await _unit.Gates.UpdateAsync(gate);
        await _unit.SaveChangesAsync();
    }
}