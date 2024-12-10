using System.ComponentModel.DataAnnotations;
using AutoSzerelo.Shared;
using Xunit;

namespace AutoSzerelo.UnitTest;

public class UgyfelUnitTests
{
    [Fact]
    public void ValidName_UgyfelValidation_IsValid()
    {
        // Arrange
        var ugyfel = new Ugyfel
        {
            Nev = "John Doe",
            Lakcim = "123 Main St",
            Email = "john.doe@example.com"
        };

        // Act
        var results = new List<ValidationResult>();
        var context = new ValidationContext(ugyfel, null, null);
        var result = Validator.TryValidateObject(ugyfel, context, results, true);

        // Assert
        Assert.True(result);
        Assert.Empty(results);
    }

    [Fact]
    public void InvalidName_UgyfelValidation_Fails()
    {
        // Arrange
        var ugyfel = new Ugyfel
        {
            Nev = new string('A', 16), // Name length exceeds max length
            Lakcim = "123 Main St",
            Email = "john.doe@example.com"
        };

        // Act
        var results = new List<ValidationResult>();
        var context = new ValidationContext(ugyfel, null, null);
        var result = Validator.TryValidateObject(ugyfel, context, results, true);

        // Assert
        Assert.False(result);
        Assert.Contains(results, r => r.MemberNames.Contains("Nev"));
    }

    [Fact]
    public void MissingName_UgyfelValidation_Fails()
    {
        // Arrange
        var ugyfel = new Ugyfel
        {
            Nev = null, // Missing required name
            Lakcim = "123 Main St",
            Email = "john.doe@example.com"
        };

        // Act
        var results = new List<ValidationResult>();
        var context = new ValidationContext(ugyfel, null, null);
        var result = Validator.TryValidateObject(ugyfel, context, results, true);

        // Assert
        Assert.False(result);
        Assert.Contains(results, r => r.MemberNames.Contains("Nev"));
    }

    [Fact]
    public void InvalidEmail_UgyfelValidation_Fails()
    {
        // Arrange
        var ugyfel = new Ugyfel
        {
            Nev = "John Doe",
            Lakcim = "123 Main St",
            Email = "invalid-email" // Invalid email format
        };

        // Act
        var results = new List<ValidationResult>();
        var context = new ValidationContext(ugyfel, null, null);
        var result = Validator.TryValidateObject(ugyfel, context, results, true);

        // Assert
        Assert.False(result);
        Assert.Contains(results, r => r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void MissingEmail_UgyfelValidation_Fails()
    {
        // Arrange
        var ugyfel = new Ugyfel
        {
            Nev = "John Doe",
            Lakcim = "123 Main St",
            Email = null // Missing required email
        };

        // Act
        var results = new List<ValidationResult>();
        var context = new ValidationContext(ugyfel, null, null);
        var result = Validator.TryValidateObject(ugyfel, context, results, true);

        // Assert
        Assert.False(result);
        Assert.Contains(results, r => r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void InvalidLakcim_UgyfelValidation_Fails()
    {
        // Arrange
        var ugyfel = new Ugyfel
        {
            Nev = "John Doe",
            Lakcim = null, // Missing required address
            Email = "john.doe@example.com"
        };

        // Act
        var results = new List<ValidationResult>();
        var context = new ValidationContext(ugyfel, null, null);
        var result = Validator.TryValidateObject(ugyfel, context, results, true);

        // Assert
        Assert.False(result);
        Assert.Contains(results, r => r.MemberNames.Contains("Lakcim"));
    }
}
