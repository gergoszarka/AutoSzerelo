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
    public async Task UpdateMunkaAsync(Munka munka)
    {
        await _httpClient.PutAsJsonAsync<Munka>($"munka/{munka.Id}", munka);
    }
    
    public float MunkaOra { get; set; }
}