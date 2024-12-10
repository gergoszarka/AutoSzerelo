using System.Net.Http.Json;
using AutoSzerelo.Shared;

namespace AutoSzerelo.UI.Services;

public class MunkakService : IMunkakService
{
    private readonly HttpClient _httpClient;
    
    public MunkakService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Munka>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Munka>>("munka");
    }

    public async Task AddAsync(Munka munka)
    {
        await _httpClient.PostAsJsonAsync("munka", munka);
    }

    public async Task<Munka> GetAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Munka>($"munka/{id}");
    }

    public async Task DeleteAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"munka/{id}");
    }

    public async Task UpdateAsync(Munka munka)
    {
        await _httpClient.PutAsJsonAsync<Munka>($"munka/{munka.Id}", munka);
    }
    

    public async Task MunkaOraAsync(Munka munka)
    {
        string munkakategoria = munka.MunkaKategoria;
        int munkagyartasiev = munka.GyartasiEv;
        int hibasulyossag = munka.HibaSuly;
        
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
        munka.MunkaOra = munkaora;
        
        await _httpClient.PutAsJsonAsync<Munka>($"munka/{munka.Id}", munka);
    }
}