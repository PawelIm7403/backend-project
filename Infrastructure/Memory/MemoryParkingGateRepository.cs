using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryParkingGateRepository 
    : MemoryGenericRepository<ParkingGate>, IParkingGateRepository
{
    public MemoryParkingGateRepository()
    {
        var gate1 = new ParkingGate()
        {
            Id = Guid.NewGuid(),
            Name = "Entry Gate",
            Type = GateType.Entry,
            Location = "Main Gate",
            IsOperational = false
        };
        
        
        var gate2 = new ParkingGate()
        {
            Id = Guid.NewGuid(),
            Name = "Exit Gate",
            Type = GateType.Exit,
            Location = "Back Gate",
            IsOperational = true
        };
        
        _data.Add(gate1.Id, gate1);
        _data.Add(gate2.Id, gate2);
    }

    public Task<ParkingGate?> FindByNameAsync(string name)
    {
        var gate = _data.Values
            .FirstOrDefault(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(gate);
    }
    
    public Task<IEnumerable<ParkingGate>> GetAllAsync()
    {
        return Task.FromResult(_data.Values.AsEnumerable());
    }
}