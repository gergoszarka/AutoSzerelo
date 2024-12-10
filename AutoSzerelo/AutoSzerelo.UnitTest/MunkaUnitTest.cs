using System.ComponentModel.DataAnnotations;
using AutoSzerelo.Shared;
using Xunit;

namespace AutoSzerelo.UnitTest;

public class MunkaUnitTests
{
    [Fact]
    public void ValidName_MunkaValidation_IsValid()
    {
        // Arrange
        var munka = new Munka
        {
            MunkaKategoria = "Motor",
            HibaSuly = 5,
            GyartasiEv = 2000,
            Rendszam = "XXX000",
            UgyfelSzam = 1
        };

        // Act
        var results = new List<ValidationResult>();
        var context = new ValidationContext(munka, null, null);
        var result = Validator.TryValidateObject(munka, context, results, true);

        // Assert
        Assert.True(result);
        Assert.Empty(results);
    }
}