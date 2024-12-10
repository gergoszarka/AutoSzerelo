using AutoSzerelo;
using AutoSzerelo.Contexts;
using AutoSzerelo.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AutoSzerelo.UnitTest;

public sealed class MunkaServiceUnitTests : IAsyncDisposable
{
    private readonly AutoSzereloContext _inMemoryDbContext;

    public MunkaServiceUnitTests()
    {
        var contextOptions = new DbContextOptionsBuilder<AutoSzereloContext>()
            .UseInMemoryDatabase("AutoSzereloTestDb")
            .Options;
        
        _inMemoryDbContext = new AutoSzereloContext(contextOptions);

        _inMemoryDbContext.Database.EnsureDeleted();
        _inMemoryDbContext.Database.EnsureCreated();

        _inMemoryDbContext.SaveChanges();
    }

    [Fact]
    public async Task Motor_AddAsync_ContainsOneMunka()
    {
        // Arrange
        var munkaService = new MunkaService(NullLogger<MunkaService>.Instance, _inMemoryDbContext);

        await munkaService.AddAsync(new Munka
        {
            MunkaKategoria = "Motor",
            HibaSuly = 5,
            GyartasiEv = 2000,
            Rendszam = "XXX000",
            Leiras = "Motor hiba",
            Id = NewGuid(),
            UgyfelSzam = 1,
            MunkaOra = 8.0f*2.0f*0.6f
        });

        // Act
        var munka = await munkaService.GetAllAsync();

        // Assert
        Assert.Single(munka);
    }

    [Fact]
    public async Task Karosszeria_AddAsync_ContainsOneMunka()
    {
        // Arrange
        var munkaService = new MunkaService(NullLogger<MunkaService>.Instance, _inMemoryDbContext);

        await munkaService.AddAsync(new Munka
        {
            MunkaKategoria = "Karosszéria",
            HibaSuly = 8,
            GyartasiEv = 1999,
            Rendszam = "ABC123",
            Leiras = "Karcolás",
            Id = NewGuid(),
            UgyfelSzam = 2,
            MunkaOra = 4.0f
        });

        // Act
        var munkak = await munkaService.GetAllAsync();

        // Assert
        Assert.Single(munkak);
    }

    [Fact]
    public async Task GetAsync_ValidId_ReturnsCorrectMunka()
    {
        // Arrange
        var munkaService = new MunkaService(NullLogger<MunkaService>.Instance, _inMemoryDbContext);
        var id = NewGuid();
        await munkaService.AddAsync(new Munka
        {
            Id = id,
            MunkaKategoria = "Motor",
            HibaSuly = 5,
            GyartasiEv = 2000,
            Rendszam = "XXX000",
            Leiras = "Motor hiba",
            UgyfelSzam = 1,
            MunkaOra = 8.0f * 2.0f * 0.6f
        });

        // Act
        var munka = await munkaService.GetAsync(id);

        // Assert
        Assert.NotNull(munka);
        Assert.Equal(id, munka.Id);
    }

    [Fact]
    public async Task DeleteAsync_ExistingMunka_RemovesMunka()
    {
        // Arrange
        var munkaService = new MunkaService(NullLogger<MunkaService>.Instance, _inMemoryDbContext);
        var id = NewGuid();
        await munkaService.AddAsync(new Munka
        {
            Id = id,
            MunkaKategoria = "Motor",
            HibaSuly = 5,
            GyartasiEv = 2000,
            Rendszam = "XXX000",
            Leiras = "Motor hiba",
            UgyfelSzam = 1,
            MunkaOra = 8.0f * 2.0f * 0.6f
        });

        // Act
        await munkaService.DeleteAsync(id);

        // Assert
        var munka = await munkaService.GetAsync(id);
        Assert.Null(munka);  // The object should be null after deletion
    }

    [Fact]
    public async Task DeleteAsync_NonExistentMunka_ThrowsKeyNotFoundException()
    {
        // Arrange
        var munkaService = new MunkaService(NullLogger<MunkaService>.Instance, _inMemoryDbContext);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => munkaService.DeleteAsync(NewGuid()));
        Assert.Equal("Nincs ilyen munka", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_ValidMunka_UpdatesCorrectly()
    {
        // Arrange
        var munkaService = new MunkaService(NullLogger<MunkaService>.Instance, _inMemoryDbContext);
        var id = NewGuid();
        await munkaService.AddAsync(new Munka
        {
            Id = id,
            MunkaKategoria = "Motor",
            HibaSuly = 5,
            GyartasiEv = 2000,
            Rendszam = "XXX000",
            Leiras = "Motor hiba",
            UgyfelSzam = 1,
            MunkaOra = 8.0f * 2.0f * 0.6f
        });

        var updatedMunka = new Munka
        {
            Id = id,
            MunkaKategoria = "Motor",
            HibaSuly = 10, // Updated value
            GyartasiEv = 2001, // Updated value
            Rendszam = "XXX001", // Updated value
            Leiras = "Motor javítva", // Updated value
            UgyfelSzam = 1,
            MunkaOra = 9.0f * 2.0f * 0.6f // Updated value
        };

        // Act
        await munkaService.UpdateAsync(updatedMunka);

        // Assert
        var munka = await munkaService.GetAsync(id);
        Assert.NotNull(munka);
        Assert.Equal(10, munka.HibaSuly);  // Assert that the HibaSuly is updated
        Assert.Equal(2001, munka.GyartasiEv);  // Assert that the GyartasiEv is updated
        Assert.Equal("XXX001", munka.Rendszam);  // Assert that the Rendszam is updated
        Assert.Equal("Motor javítva", munka.Leiras);  // Assert that the Leiras is updated
    }

    public Guid NewGuid()
    {
        return new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd");
    }

    public async ValueTask DisposeAsync()
    {
        await _inMemoryDbContext.DisposeAsync();
    }
}
