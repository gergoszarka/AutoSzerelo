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


    public Guid NewGuid()
    {
        return new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd");
    }

    public async ValueTask DisposeAsync()
    {
        await _inMemoryDbContext.DisposeAsync();
    }
    
   
}
