using AutoSzerelo.Shared;

namespace AutoSzerelo.UI.Services;

public interface IUgyfelekService
{
    public Task<List<Ugyfel>> GetAllAsync();
    
    public Task AddAsync(Ugyfel ugyfel);
    
    public Task<Ugyfel> GetAsync(Guid id);
    
    public Task DeleteAsync(Guid id);
    
    public Task UpdateAsync(Ugyfel ugyfel);
}