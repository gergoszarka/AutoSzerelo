using AutoSzerelo.Controllers;
using AutoSzerelo.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AutoSzerelo.UnitTest
{
    public class UgyfelControllerUnitTest
    {
        private readonly Mock<IUgyfelService> _mockUgyfelService;
        private readonly UgyfelController _controller;

        public UgyfelControllerUnitTest()
        {
            // Mock the IUgyfelService
            _mockUgyfelService = new Mock<IUgyfelService>();
            
            // Instantiate the controller with the mocked service
            _controller = new UgyfelController(_mockUgyfelService.Object);
        }

        [Fact]
        public async Task Add_ExistingUgyfel_ReturnsConflict()
        {
            // Arrange
            var existingUgyfel = new Ugyfel { Id = NewGuid() };
            _mockUgyfelService.Setup(service => service.GetAsync(existingUgyfel.Id))
                             .ReturnsAsync(existingUgyfel);

            // Act
            var result = await _controller.Add(existingUgyfel);

            // Assert
            var conflictResult = Assert.IsType<ConflictResult>(result);
        }

        [Fact]
        public async Task Add_NewUgyfel_ReturnsOk()
        {
            // Arrange
            var newUgyfel = new Ugyfel { Id = NewGuid() };
            _mockUgyfelService.Setup(service => service.GetAsync(newUgyfel.Id))
                             .ReturnsAsync((Ugyfel)null); // Ensuring it doesn't exist yet

            // Act
            var result = await _controller.Add(newUgyfel);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockUgyfelService.Verify(service => service.AddAsync(newUgyfel), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingUgyfel_ReturnsOk()
        {
            // Arrange
            var ugyfelId = NewGuid();
            var ugyfel = new Ugyfel { Id = ugyfelId };
            _mockUgyfelService.Setup(service => service.GetAsync(ugyfelId))
                             .ReturnsAsync(ugyfel);

            // Act
            var result = await _controller.Delete(ugyfelId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockUgyfelService.Verify(service => service.DeleteAsync(ugyfelId), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistentUgyfel_ReturnsNotFound()
        {
            // Arrange
            var ugyfelId = NewGuid();
            _mockUgyfelService.Setup(service => service.GetAsync(ugyfelId))
                             .ReturnsAsync((Ugyfel)null);

            // Act
            var result = await _controller.Delete(ugyfelId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }



        [Fact]
        public async Task Update_ValidUgyfel_ReturnsOk()
        {
            // Arrange
            var ugyfelId = NewGuid();
            var existingUgyfel = new Ugyfel { Id = ugyfelId };
            var updatedUgyfel = new Ugyfel { Id = ugyfelId, Nev = "Updated" };
            _mockUgyfelService.Setup(service => service.GetAsync(ugyfelId))
                             .ReturnsAsync(existingUgyfel);

            // Act
            var result = await _controller.Update(ugyfelId, updatedUgyfel);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockUgyfelService.Verify(service => service.UpdateAsync(updatedUgyfel), Times.Once);
        }

        public Guid NewGuid()
        {
            return new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd");
        }
    }
}
