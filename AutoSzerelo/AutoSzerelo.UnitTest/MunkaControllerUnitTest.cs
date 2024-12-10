using AutoSzerelo.Controllers;
using AutoSzerelo.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AutoSzerelo.UnitTest
{
    public class MunkaControllerUnitTest
    {
        private readonly Mock<IMunkaService> _mockMunkaService;
        private readonly MunkaController _controller;

        public MunkaControllerUnitTest()
        {
            // Mock the IMunkaService
            _mockMunkaService = new Mock<IMunkaService>();
            
            // Instantiate the controller with the mocked service
            _controller = new MunkaController(_mockMunkaService.Object);
        }

        [Fact]
        public async Task Add_ExistingMunka_ReturnsConflict()
        {
            // Arrange
            var existingMunka = new Munka { Id = Guid.NewGuid() };
            _mockMunkaService.Setup(service => service.GetAsync(existingMunka.Id))
                             .ReturnsAsync(existingMunka);

            // Act
            var result = await _controller.Add(existingMunka);

            // Assert
            var conflictResult = Assert.IsType<ConflictResult>(result);
        }

        [Fact]
        public async Task Add_NewMunka_ReturnsOk()
        {
            // Arrange
            var newMunka = new Munka { Id = Guid.NewGuid() };
            _mockMunkaService.Setup(service => service.GetAsync(newMunka.Id))
                             .ReturnsAsync((Munka)null); // Ensuring it doesn't exist yet

            // Act
            var result = await _controller.Add(newMunka);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockMunkaService.Verify(service => service.AddAsync(newMunka), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingMunka_ReturnsOk()
        {
            // Arrange
            var munkaId = Guid.NewGuid();
            var munka = new Munka { Id = munkaId };
            _mockMunkaService.Setup(service => service.GetAsync(munkaId))
                             .ReturnsAsync(munka);

            // Act
            var result = await _controller.Delete(munkaId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockMunkaService.Verify(service => service.DeleteAsync(munkaId), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistentMunka_ReturnsNotFound()
        {
            // Arrange
            var munkaId = Guid.NewGuid();
            _mockMunkaService.Setup(service => service.GetAsync(munkaId))
                             .ReturnsAsync((Munka)null);

            // Act
            var result = await _controller.Delete(munkaId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ValidMunka_ReturnsOk()
        {
            // Arrange
            var munkaId = Guid.NewGuid();
            var existingMunka = new Munka { Id = munkaId };
            var updatedMunka = new Munka { Id = munkaId, Leiras = "Updated" };
            _mockMunkaService.Setup(service => service.GetAsync(munkaId))
                             .ReturnsAsync(existingMunka);

            // Act
            var result = await _controller.Update(munkaId, updatedMunka);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockMunkaService.Verify(service => service.UpdateAsync(updatedMunka), Times.Once);
        }

        [Fact]
        public async Task Update_MismatchedId_ReturnsBadRequest()
        {
            // Arrange
            var munkaId = Guid.NewGuid();
            var updatedMunka = new Munka { Id = Guid.NewGuid() }; // Mismatched ID
            _mockMunkaService.Setup(service => service.GetAsync(munkaId))
                             .ReturnsAsync((Munka)null);

            // Act
            var result = await _controller.Update(munkaId, updatedMunka);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }
    }
}
