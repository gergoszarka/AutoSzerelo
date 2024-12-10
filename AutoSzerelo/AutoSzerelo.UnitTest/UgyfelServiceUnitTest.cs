using AutoSzerelo;
using AutoSzerelo.Contexts;
using AutoSzerelo.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AutoSzerelo.UnitTest;

public sealed class UgyfelServiceUnitTests : IAsyncDisposable
{
    private readonly AutoSzereloContext _inMemoryDbContext;

    public UgyfelServiceUnitTests()
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
    public async Task AddAsync_ValidUgyfel_AddsUgyfelToDatabase()
    {
        // Arrange
        var ugyfelService = new UgyfelService(NullLogger<UgyfelService>.Instance, _inMemoryDbContext);
        var newUgyfel = new Ugyfel
        {
            Id = NewGuid1(),
            Nev = "John Doe",
            Lakcim = "123 Main St",
            Email = "john.doe@example.com"
        };

        // Act
        await ugyfelService.AddAsync(newUgyfel);

        // Assert
        var fetchedUgyfel = await ugyfelService.GetAsync(newUgyfel.Id);
        Assert.NotNull(fetchedUgyfel);
        Assert.Equal("John Doe", fetchedUgyfel.Nev);
        Assert.Equal("123 Main St", fetchedUgyfel.Lakcim);
        Assert.Equal("john.doe@example.com", fetchedUgyfel.Email);
    }

    [Fact]
    public async Task DeleteAsync_ExistingUgyfel_RemovesUgyfelFromDatabase()
    {
        // Arrange
        var ugyfelService = new UgyfelService(NullLogger<UgyfelService>.Instance, _inMemoryDbContext);
        var id = NewGuid3();
        var newUgyfel = new Ugyfel
        {
            Id = id,
            Nev = "Jane Doe",
            Lakcim = "456 Another St",
            Email = "jane.doe@example.com"
        };
        await ugyfelService.AddAsync(newUgyfel);

        // Act
        await ugyfelService.DeleteAsync(id);

        // Assert
        var deletedUgyfel = await ugyfelService.GetAsync(id);
        Assert.Null(deletedUgyfel);
    }

    [Fact]
    public async Task DeleteAsync_NonExistentUgyfel_ThrowsKeyNotFoundException()
    {
        // Arrange
        var ugyfelService = new UgyfelService(NullLogger<UgyfelService>.Instance, _inMemoryDbContext);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => ugyfelService.DeleteAsync(NewGuid2()));
        Assert.Equal("Nincs ilyen ügyfél", exception.Message);
    }

    [Fact]
    public async Task GetAsync_ValidId_ReturnsCorrectUgyfel()
    {
        // Arrange
        var ugyfelService = new UgyfelService(NullLogger<UgyfelService>.Instance, _inMemoryDbContext);
        var id = NewGuid1();
        var newUgyfel = new Ugyfel
        {
            Id = id,
            Nev = "Test User",
            Lakcim = "789 Test St",
            Email = "test.user@example.com"
        };
        await ugyfelService.AddAsync(newUgyfel);

        // Act
        var fetchedUgyfel = await ugyfelService.GetAsync(id);

        // Assert
        Assert.NotNull(fetchedUgyfel);
        Assert.Equal(id, fetchedUgyfel.Id);
        Assert.Equal("Test User", fetchedUgyfel.Nev);
        Assert.Equal("789 Test St", fetchedUgyfel.Lakcim);
        Assert.Equal("test.user@example.com", fetchedUgyfel.Email);
    }


    [Fact]
    public async Task UpdateAsync_ValidUgyfel_UpdatesUgyfelCorrectly()
    {
        // Arrange
        var ugyfelService = new UgyfelService(NullLogger<UgyfelService>.Instance, _inMemoryDbContext);
        var id = NewGuid3();
        var newUgyfel = new Ugyfel
        {
            Id = id,
            Nev = "Original Name",
            Lakcim = "Original Address",
            Email = "original.email@example.com"
        };
        await ugyfelService.AddAsync(newUgyfel);

        var updatedUgyfel = new Ugyfel
        {
            Id = id,
            Nev = "Updated Name",
            Lakcim = "Updated Address",
            Email = "updated.email@example.com"
        };

        // Act
        await ugyfelService.UpdateAsync(updatedUgyfel);

        // Assert
        var fetchedUgyfel = await ugyfelService.GetAsync(id);
        Assert.NotNull(fetchedUgyfel);
        Assert.Equal("Updated Name", fetchedUgyfel.Nev);
        Assert.Equal("Updated Address", fetchedUgyfel.Lakcim);
        Assert.Equal("updated.email@example.com", fetchedUgyfel.Email);
    }

    public Guid NewGuid1()
    {
        return new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd");
    }
    public Guid NewGuid2()
    {
        return new Guid("addddddd-dddd-dddd-dddd-dddddddddddd");
    }
    public Guid NewGuid3()
    {
        return new Guid("3ddddddd-dddd-dddd-dddd-dddddddddddd");
    }

    public async ValueTask DisposeAsync()
    {
        await _inMemoryDbContext.DisposeAsync();
    }
}
