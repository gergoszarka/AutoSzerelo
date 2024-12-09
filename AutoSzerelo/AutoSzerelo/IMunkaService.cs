using AutoSzerelo.Shared;

namespace AutoSzerelo;

public interface IMunkaService
{
    
    Task AddAsync(Munka munka);

    Task DeleteAsync(Guid id);

    Task<Munka> GetAsync(Guid id);

    Task<List<Munka>> GetAllAsync();

    Task UpdateAsync(Munka newMunka);
}