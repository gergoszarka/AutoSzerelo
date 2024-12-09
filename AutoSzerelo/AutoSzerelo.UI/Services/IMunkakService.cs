using AutoSzerelo.Shared;
namespace AutoSzerelo.UI.Services;

public interface IMunkakService
{
    public Task<List<Munka>> GetAllAsync();
    
    public Task AddAsync(Munka ugyfel);
    
    public Task<Munka> GetAsync(Guid id);
    
    public Task DeleteAsync(Guid id);
    
    public Task UpdateAsync(Munka ugyfel);

    
}