using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vehicle_Registration_System.Controllers;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Services.Interfaces;
using Xunit;

namespace Vehicle_Registration_System.Tests.ControllerTests
{
    public class OwnerControllerTests
    {
        [Fact]
        public async Task GetAllOwners_ShouldReturnOkWithOwnersFromCache()
        {
            // Arrange
            var mockService = new Mock<IOwnerService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new OwnerController(mockService.Object, mockMemoryCache.Object);

            var cachedOwners = new List<Owner> { new Owner { Id = 1, FirstName = "Test" } };
            object cacheValue = cachedOwners;

            mockMemoryCache.Setup(mc => mc.TryGetValue("AllOwners", out cacheValue)).Returns(true);

            // Act
            var result = await controller.GetAllOwners();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedOwners, okResult.Value);
        }

        [Fact]
        public async Task AddOwner_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IOwnerService>();
            var controller = new OwnerController(mockService.Object, null);

            var inputOwner = new InputOwner { FirstName = "Owner" };

            mockService.Setup(service => service.AddOwner(inputOwner)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.AddOwner(inputOwner);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockService.Verify(service => service.AddOwner(inputOwner), Times.Once);
        }

        [Fact]
        public async Task DeleteOwner_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IOwnerService>();
            var controller = new OwnerController(mockService.Object, null);

            int ownerId = 1;

            mockService.Setup(service => service.DeleteOwner(ownerId)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteOwner(ownerId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockService.Verify(service => service.DeleteOwner(ownerId), Times.Once);
        }

        [Fact]
        public async Task UpdateOwner_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IOwnerService>();
            var controller = new OwnerController(mockService.Object, null);

            int ownerId = 1;
            var editOwner = new EditOwner { Email = "Email@email.com" };

            mockService.Setup(service => service.UpdateOwner(editOwner, ownerId)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateOwner(editOwner, ownerId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockService.Verify(service => service.UpdateOwner(editOwner, ownerId), Times.Once);
        }

        [Fact]
        public async Task FindOwnerByVehicle_ShouldReturnOkWithOwnersFromCache()
        {
            // Arrange
            var mockService = new Mock<IOwnerService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new OwnerController(mockService.Object, mockMemoryCache.Object);

            string manufacturer = "Test Manufacturer";
            string model = "Test Model";
            var cachedOwners = new List<Owner> { new Owner { Id = 1, FirstName = "Test" } };
            object cacheValue = cachedOwners;

            mockMemoryCache.Setup(mc => mc.TryGetValue($"OwnersByVehicle_{manufacturer}_{model}", out cacheValue)).Returns(true);

            // Act
            var result = await controller.FindOwnerByVehicle(manufacturer, model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedOwners, okResult.Value);
        }

        [Fact]
        public async Task GetLicensesByCity_ShouldReturnOkWithCountFromCache()
        {
            // Arrange
            var mockService = new Mock<IOwnerService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new OwnerController(mockService.Object, mockMemoryCache.Object);

            string placeOfBirth = "Test City";
            float cachedCount = 10;
            object cacheValue = cachedCount;

            mockMemoryCache.Setup(mc => mc.TryGetValue($"LicensesByCity_{placeOfBirth}", out cacheValue)).Returns(true);

            // Act
            var result = await controller.GetLicensesByCity(placeOfBirth);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedCount, okResult.Value);
        }
    }
}
