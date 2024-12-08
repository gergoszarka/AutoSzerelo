using System.Net.Http.Json;
using AutoSzerelo.Shared;

namespace AutoSzerelo.UI.Services;

public class UgyfelekService : IUgyfelekService
{
    
    private readonly HttpClient _httpClient;
    
    public UgyfelekService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Ugyfel>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Ugyfel>>("ugyfel");
    }

    public async Task AddAsync(Ugyfel ugyfel)
    {
        await _httpClient.PostAsJsonAsync("ugyfel", ugyfel);
    }

    public async Task<Ugyfel> GetAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Ugyfel>($"ugyfel/{id}");
    }

    public async Task DeleteAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"ugyfel/{id}");
    }

    public async Task UpdateAsync(Ugyfel ugyfel)
    {
        await _httpClient.PutAsJsonAsync<Ugyfel>($"ugyfel/{ugyfel.Id}", ugyfel);
    }
}