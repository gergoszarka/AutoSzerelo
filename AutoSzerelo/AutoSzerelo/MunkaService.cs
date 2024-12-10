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
        existingMunka.MunkaOra = newMunka.MunkaOra;
        
        await _context.SaveChangesAsync();
    }
    
    public async Task MunkaOraAsync(Munka newMunka)
    {
        var existingMunka = await GetAsync(newMunka.Id);
        
        string munkakategoria = existingMunka.MunkaKategoria;
        int munkagyartasiev = existingMunka.GyartasiEv;
        int hibasulyossag = existingMunka.HibaSuly;
        
        float munkaora=0;

        switch (munkakategoria)
        {
            case "Karosszéria":
                munkaora = 3;
                break;
            case "Motor":
                munkaora = 8;
                break;
            case "Futómű":
                munkaora = 6;
                break;
            case "Fékberendezés":
                munkaora = 4;
                break;
            default:
                munkaora = 0;
                break;
        }

        int deltaev = 2024 - munkagyartasiev;

        if (deltaev <=5)
        {
            munkaora = munkaora * 0.5f;
        }
        else if (deltaev <=10 && deltaev > 5)
        {
            munkaora = munkaora * 1.0f;
        }
        else if (deltaev <=20 && deltaev > 10)
        {
            munkaora = munkaora * 1.5f;
        }
        else if (deltaev > 20)
        {
            munkaora = munkaora * 2.0f;
        }

        if (hibasulyossag <=2)
        {
            munkaora = munkaora * 0.2f;
        }
        else if (hibasulyossag <=4 && hibasulyossag >= 3)
        {
            munkaora = munkaora * 0.4f;
        }
        else if (hibasulyossag <=7 && hibasulyossag >= 5)
        {
            munkaora = munkaora * 0.6f;
        }
        else if (hibasulyossag <=9 && hibasulyossag >= 8)
        {
            munkaora = munkaora * 0.8f;
        }
        else if (hibasulyossag   >= 10)
        {
            munkaora = munkaora * 1.0f;
        }
        existingMunka.MunkaOra = munkaora;
        
        await _context.SaveChangesAsync();
    }

    
}