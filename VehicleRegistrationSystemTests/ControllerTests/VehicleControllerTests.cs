using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vehicle_Registration_System.Controllers;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Services.Interfaces;
using Xunit;

namespace Vehicle_Registration_System.Tests.ControllerTests
{
    public class VehicleControllerTests
    {
        [Fact]
        public async Task GetAllVehicles_ShouldReturnOkWithVehiclesFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            var cachedVehicles = new List<VehicleDto> { new VehicleDto { Id = 1, Manufacturer = "Audi", Model = "A4" } };
            object cacheValue = cachedVehicles;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue("AllVehicles", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.GetAllVehicles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedVehicles, okResult.Value);
        }

        [Fact]
        public async Task GetVehicleById_ShouldReturnOkWithVehicleFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            int vehicleId = 1;
            var cachedVehicle = new VehicleDto { Id = vehicleId, Manufacturer = "Audi", Model = "A4" };
            object cacheValue = cachedVehicle;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"Vehicle_{vehicleId}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.GetVehicleById(vehicleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedVehicle, okResult.Value);
        }

        [Fact]
        public async Task AddVehicle_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            var registerVehicle = new RegisterVehicle { Manufacturer = "Audi", Model = "A4" };

            // Act
            var result = await controller.AddVehicle(registerVehicle);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockService.Verify(service => service.AddVehicle(registerVehicle), Times.Once);
            mockMemoryCache.Verify(mc => mc.Remove("AllVehicles"), Times.Once);
        }

        [Fact]
        public async Task DeleteVehicle_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            int vehicleId = 1;

            // Act
            var result = await controller.DeleteVehicle(vehicleId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockService.Verify(service => service.DeleteVehicle(vehicleId), Times.Once);
            mockMemoryCache.Verify(mc => mc.Remove("AllVehicles"), Times.Once);
            mockMemoryCache.Verify(mc => mc.Remove($"Vehicle_{vehicleId}"), Times.Once);
        }

        [Fact]
        public async Task UpdateVehicle_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            int vehicleId = 1;
            var editVehicle = new EditVehicle { LicensePlate = "ABC123", OwnerId = 1 };

            // Act
            var result = await controller.UpdateVehicle(editVehicle, vehicleId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockService.Verify(service => service.UpdateVehicle(editVehicle, vehicleId), Times.Once);
            mockMemoryCache.Verify(mc => mc.Remove("AllVehicles"), Times.Once);
            mockMemoryCache.Verify(mc => mc.Remove($"Vehicle_{vehicleId}"), Times.Once);
        }


        [Fact]
        public async Task GetVehiclesByOwner_ShouldReturnOkWithVehiclesFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            int ownerId = 1;
            var cachedVehicles = new List<VehicleDto> { new VehicleDto { Id = 1, Manufacturer = "Audi", Model = "A4" } };
            object cacheValue = cachedVehicles;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"VehiclesByOwner_{ownerId}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.GetVehiclesByOwner(ownerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedVehicles, okResult.Value);
        }

        [Fact]
        public async Task GetVehiclesByYear_ShouldReturnOkWithVehiclesFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            int year = 2020;
            var cachedVehicles = new List<VehicleDto> { new VehicleDto { Id = 1, Manufacturer = "Audi", Model = "A4", Year = year } };
            object cacheValue = cachedVehicles;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"VehiclesByYear_{year}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.GetVehiclesByYear(year);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedVehicles, okResult.Value);
        }

        [Fact]
        public async Task GetVehiclesWithMorePower_ShouldReturnOkWithVehiclesFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            int power = 200;
            var cachedVehicles = new List<VehicleDto> { new VehicleDto { Id = 1, Manufacturer = "Audi", Model = "A4" } };
            object cacheValue = cachedVehicles;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"VehiclesWithMorePower_{power}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.GetVehiclesWithMorePower(power);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedVehicles, okResult.Value);
        }

        [Fact]
        public async Task GetVehiclesByFuel_ShouldReturnOkWithVehiclesFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            string fuel = "Petrol";
            var cachedVehicles = new List<VehicleDto> { new VehicleDto { Id = 1, Manufacturer = "Audi", Model = "A4" } };
            object cacheValue = cachedVehicles;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"VehiclesByFuel_{fuel}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.GetVehiclesByFuel(fuel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedVehicles, okResult.Value);
        }

        [Fact]
        public async Task GetVehiclesByBrand_ShouldReturnOkWithVehiclesFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            string brand = "Audi";
            var cachedVehicles = new List<VehicleDto> { new VehicleDto { Id = 1, Manufacturer = brand, Model = "A4" } };
            object cacheValue = cachedVehicles;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"VehiclesByBrand_{brand}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.GetVehiclesByBrand(brand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedVehicles, okResult.Value);
        }

        [Fact]
        public async Task CountVehiclesByBrand_ShouldReturnOkWithCountFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            string brand = "Audi";
            long cachedCount = 5;
            object cacheValue = cachedCount;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"CountByBrand_{brand}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.CountVehiclesByBrand(brand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedCount, okResult.Value);
        }

        [Fact]
        public async Task CountUnregisteredVehicles_ShouldReturnOkWithCountFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            long cachedCount = 5;
            object cacheValue = cachedCount;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue("CountUnregistered", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.CountUnregisteredVehicles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedCount, okResult.Value);
        }

        [Fact]
        public async Task CountRegisteredVehicles_ShouldReturnOkWithCountFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            long cachedCount = 5;
            object cacheValue = cachedCount;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue("CountRegistered", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.CountRegisteredVehicles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedCount, okResult.Value);
        }

        [Fact]
        public async Task CountTransmission_ShouldReturnOkWithCountFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            string transmission = "Automatic";
            long cachedCount = 5;
            object cacheValue = cachedCount;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"CountTransmission_{transmission}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.CountTransmission(transmission);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedCount, okResult.Value);
        }

        [Fact]
        public async Task FindVehicleByLicensePlate_ShouldReturnOkWithVehicleFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            string licensePlate = "ABC123";
            var cachedVehicle = new VehicleDto { Id = 1, Manufacturer = "Audi", Model = "A4", LicensePlate = licensePlate };
            object cacheValue = cachedVehicle;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"VehicleByLicensePlate_{licensePlate}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.FindVehicleByLicensePlate(licensePlate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedVehicle, okResult.Value);
        }

        [Fact]
        public async Task CheckVehicleRegistration_ShouldReturnOkWithRegistrationStatusFromCache()
        {
            // Arrange
            var mockService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new VehicleController(mockService.Object, mockMemoryCache.Object);

            int vehicleId = 1;
            bool cachedStatus = true;
            object cacheValue = cachedStatus;

            mockMemoryCache
                .Setup(mc => mc.TryGetValue($"CheckRegistration_{vehicleId}", out cacheValue))
                .Returns(true);

            // Act
            var result = await controller.CheckVehicleRegistration(vehicleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedStatus, okResult.Value);
        }
    }
}
