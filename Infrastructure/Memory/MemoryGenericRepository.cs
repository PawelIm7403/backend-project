using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class MemoryGenericRepository<T>: IGenericRepositoryAsync<T> 
    where T: EntityBase 
{
    protected Dictionary<Guid, T> _data = new();
    
    public Task<T?> FindByIdAsync(Guid id)
    {
        var result = _data.TryGetValue(id, out var value) ? value : null;
        return Task.FromResult(result);
    }

    public Task<IEnumerable<T>> FindAllAsync()
    {
        return Task.FromResult(
            _data.Values.AsEnumerable());
    }

    public Task RemoveByIdAsync(Guid id)
    {
        if (!_data.ContainsKey(id))
            throw new KeyNotFoundException("Entity not found");

        _data.Remove(id);
        
        return Task.CompletedTask;
    }

    public Task<T> UpdateAsync(T entity)
    {
        if (!_data.ContainsKey(entity.Id))
            throw new KeyNotFoundException("Entity not found");
        
        _data[entity.Id] = entity;
        
        return Task.FromResult(entity);
    }
    // pozostałe metody

    public Task<T> AddAsync(T entity)
    {
        if (_data.ContainsKey(entity.Id))
            throw new InvalidOperationException("Entity with this ID already exists");
        
        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        var total = _data.Count;
        
        var items = _data.Values
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PagedResult<T>(
            items,
            total,
            page,
            pageSize);
        
        return Task.FromResult(result);
    }
}