using AutoSzerelo.Contexts;
using AutoSzerelo.Shared;
using Microsoft.EntityFrameworkCore;

namespace AutoSzerelo;

public class UgyfelService
{
    private AutoSzereloContext _context;
    private ILogger<UgyfelService> _logger;

    public UgyfelService(ILogger<UgyfelService> logger, AutoSzereloContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task AddAsync(Ugyfel ugyfel)
    {
        _logger.LogInformation("Ügyfél hozzáadása: {@Ugyfel}", ugyfel);

        await _context.AddAsync(ugyfel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var ugyfel = await GetAsync(id);

        if (ugyfel is null)
        {
            throw new KeyNotFoundException("Nincs ilyen ügyfél");
        }

        _context.Remove(ugyfel);
        await _context.SaveChangesAsync();
    }

    public async Task<Ugyfel> GetAsync(Guid id)
    {
        return await _context.FindAsync<Ugyfel>(id);
    }

    public async Task<List<Ugyfel>> GetAllAsync()
    {
        _logger.LogInformation("All people retrieved");
        return await _context.Ugyfelek.ToListAsync();
    }
    

    public async Task UpdateAsync(Ugyfel newUgyfel)
    {
        var existingUgyfel = await GetAsync(newUgyfel.Id);

        existingUgyfel.Nev = newUgyfel.Nev;
        existingUgyfel.Lakcim = newUgyfel.Lakcim;
        existingUgyfel.Email = newUgyfel.Email;
        
        await _context.SaveChangesAsync();
    }
}