using AutoSzerelo.Contexts;
using AutoSzerelo.Shared;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AutoSzerelo.UnitTest
{
    public class CalculateMunkaOraUnitTest
    {
        public class MunkaServiceUnitTest
        {
            private readonly MunkaService _munkakService;
            private readonly AutoSzereloContext _context;

            public MunkaServiceUnitTest()
            {
                var options = new DbContextOptionsBuilder<AutoSzereloContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase")
                    .Options;

                _context = new AutoSzereloContext(options);

                _munkakService = new MunkaService(NullLogger<MunkaService>.Instance, _context);
            }

            [Fact]
            public async Task MunkaOraAsync_ValidInputs_CalculatesCorrectMunkaOra()
            {
                // Arrange
                var newMunka = new Munka
                {
                    Id = NewGuid(),
                    Rendszam = "ASD123",
                    MunkaKategoria = "Motor",
                    GyartasiEv = 2017,
                    Leiras = "asdasdasd",
                    HibaSuly = 10, 
                    UgyfelSzam = 1,
                    MunkaAllapota = 1
                };

                
                _context.Munkak.Add(newMunka);
                await _context.SaveChangesAsync();

                // Act
                await _munkakService.MunkaOraAsync(newMunka);

                // Assert
                Assert.Equal(8.0f, newMunka.MunkaOra);
            }
            [Fact]
            public async Task MunkaOraAsync_ValidInputs_CalculatesCorrectMunkaOra1()
            {
                // Arrange
                var newMunka = new Munka
                {
                    Id = NewGuid1(),
                    Rendszam = "ASD123",
                    MunkaKategoria = "Karosszéria",
                    GyartasiEv = 2023,
                    Leiras = "asdasdasd",
                    HibaSuly = 9, 
                    UgyfelSzam = 1,
                    MunkaAllapota = 1
                };

                
                _context.Munkak.Add(newMunka);
                await _context.SaveChangesAsync();

                // Act
                await _munkakService.MunkaOraAsync(newMunka);

                // Assert
                Assert.Equal(1.2f, newMunka.MunkaOra);
            }
            [Fact]
            public async Task MunkaOraAsync_ValidInputs_CalculatesCorrectMunkaOra2()
            {
                // Arrange
                var newMunka = new Munka
                {
                    Id = NewGuid2(),
                    Rendszam = "ASD123",
                    MunkaKategoria = "Futómű",
                    GyartasiEv = 1991,
                    Leiras = "asdasdasd",
                    HibaSuly = 5, 
                    UgyfelSzam = 1,
                    MunkaAllapota = 1
                };

                
                _context.Munkak.Add(newMunka);
                await _context.SaveChangesAsync();

                // Act
                await _munkakService.MunkaOraAsync(newMunka);

                // Assert
                Assert.Equal(7.20000029f, newMunka.MunkaOra);
            }
            [Fact]
            public async Task MunkaOraAsync_ValidInputs_CalculatesCorrectMunkaOra3()
            {
                // Arrange
                var newMunka = new Munka
                {
                    Id = NewGuid3(),
                    Rendszam = "ASD123",
                    MunkaKategoria = "Fékberendezés",
                    GyartasiEv = 2000,
                    Leiras = "asdasdasd",
                    HibaSuly = 2, 
                    UgyfelSzam = 1,
                    MunkaAllapota = 1
                };

                
                _context.Munkak.Add(newMunka);
                await _context.SaveChangesAsync();

                // Act
                await _munkakService.MunkaOraAsync(newMunka);

                // Assert
                Assert.Equal(1.6f, newMunka.MunkaOra);
            }

            public Guid NewGuid()
            {
                return new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd");
            }
            public Guid NewGuid1()
            {
                return new Guid("addddddd-dddd-dddd-dddd-dddddddddddd");
            }
            public Guid NewGuid2()
            {
                return new Guid("bddddddd-dddd-dddd-dddd-dddddddddddd");
            }
            public Guid NewGuid3()
            {
                return new Guid("cddddddd-dddd-dddd-dddd-dddddddddddd");
            }
        }
    }
}
