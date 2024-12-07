using AutoSzerelo.Shared;

namespace AutoSzerelo;

public interface IUgyfelService
{
    
    Task AddAsync(Ugyfel ugyfel);

    Task DeleteAsync(Guid id);

    Task<Ugyfel> GetAsync(Guid id);

    Task<List<Ugyfel>> GetAllAsync();

    Task UpdateAsync(Ugyfel newUgyfel);
}