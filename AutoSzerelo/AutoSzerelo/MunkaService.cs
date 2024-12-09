using AutoSzerelo.Contexts;
using AutoSzerelo.Shared;
using Microsoft.EntityFrameworkCore;

namespace AutoSzerelo;

public class MunkaService : IMunkaService
{
    private AutoSzereloContext _context;
    private ILogger<MunkaService> _logger;

    public MunkaService(ILogger<MunkaService> logger, AutoSzereloContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task AddAsync(Munka munka)
    {
        _logger.LogInformation("Munka hozzáadása: {@Munka}", munka);

        await _context.AddAsync(munka);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var munka = await GetAsync(id);

        if (munka is null)
        {
            throw new KeyNotFoundException("Nincs ilyen munka");
        }

        _context.Remove(munka);
        await _context.SaveChangesAsync();
    }

    public async Task<Munka> GetAsync(Guid id)
    {
        return await _context.FindAsync<Munka>(id);
    }

    

    public async Task<List<Munka>> GetAllAsync()
    {
        _logger.LogInformation("All munka retrieved");
        return await _context.Munkak.ToListAsync();
    }
    

    public async Task UpdateAsync(Munka newMunka)
    {
        var existingMunka = await GetAsync(newMunka.Id);

        existingMunka.UgyfelSzam = newMunka.UgyfelSzam;
        existingMunka.Rendszam = newMunka.Rendszam;
        existingMunka.GyartasiEv = newMunka.GyartasiEv;
        existingMunka.MunkaKategoria = newMunka.MunkaKategoria;
        existingMunka.Leiras = newMunka.Leiras;
        existingMunka.HibaSuly = newMunka.HibaSuly;
        existingMunka.MunkaAllapota = newMunka.MunkaAllapota;
        
        await _context.SaveChangesAsync();
    }
}