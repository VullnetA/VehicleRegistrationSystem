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
    public class InsuranceControllerTests
    {
        [Fact]
        public async Task GetAllInsurances_ShouldReturnOkWithInsurancesFromCache()
        {
            // Arrange
            var mockService = new Mock<IInsuranceService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new InsuranceController(mockService.Object, mockMemoryCache.Object);

            var cachedInsurances = new List<Insurance> { new Insurance { Id = 1, InsuranceFee = 100 } };
            object cacheValue = cachedInsurances;

            mockMemoryCache.Setup(mc => mc.TryGetValue("AllInsurances", out cacheValue)).Returns(true);

            // Act
            var result = await controller.GetAllInsurances();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedInsurances, okResult.Value);
        }

        [Fact]
        public async Task GetInsuranceById_ShouldReturnOkWithInsuranceFromCache()
        {
            // Arrange
            var mockService = new Mock<IInsuranceService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new InsuranceController(mockService.Object, mockMemoryCache.Object);

            int insuranceId = 1;
            var cachedInsurance = new Insurance { Id = insuranceId, InsuranceFee = 100 };
            object cacheValue = cachedInsurance;

            mockMemoryCache.Setup(mc => mc.TryGetValue($"Insurance_{insuranceId}", out cacheValue)).Returns(true);

            // Act
            var result = await controller.GetInsuranceById(insuranceId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedInsurance, okResult.Value);
        }

        [Fact]
        public async Task AddInsurance_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IInsuranceService>();
            var controller = new InsuranceController(mockService.Object, null);

            var makeInsurance = new MakeInsurance { VehicleId = 1 };

            mockService.Setup(service => service.AddInsurance(makeInsurance)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.AddInsurance(makeInsurance);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockService.Verify(service => service.AddInsurance(makeInsurance), Times.Once);
        }

        [Fact]
        public async Task DeleteInsurance_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IInsuranceService>();
            var controller = new InsuranceController(mockService.Object, null);

            int insuranceId = 1;

            mockService.Setup(service => service.DeleteInsurance(insuranceId)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteInsurance(insuranceId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockService.Verify(service => service.DeleteInsurance(insuranceId), Times.Once);
        }

        [Fact]
        public async Task CountInsurances_ShouldReturnOkWithCountFromCache()
        {
            // Arrange
            var mockService = new Mock<IInsuranceService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new InsuranceController(mockService.Object, mockMemoryCache.Object);

            long cachedCount = 10;
            object cacheValue = cachedCount;

            mockMemoryCache.Setup(mc => mc.TryGetValue("CountInsurances", out cacheValue)).Returns(true);

            // Act
            var result = await controller.CountInsurances();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedCount, okResult.Value);
        }

        [Fact]
        public async Task FindExpiredInsurances_ShouldReturnOkWithExpiredInsurancesFromCache()
        {
            // Arrange
            var mockService = new Mock<IInsuranceService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var controller = new InsuranceController(mockService.Object, mockMemoryCache.Object);

            var cachedExpiredInsurances = new List<Vehicle> { new Vehicle { Id = 1, LicensePlate = "XYZ 1234" } };
            object cacheValue = cachedExpiredInsurances;

            mockMemoryCache.Setup(mc => mc.TryGetValue("ExpiredInsurances", out cacheValue)).Returns(true);

            // Act
            var result = await controller.FindExpiredInsurances();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(cachedExpiredInsurances, okResult.Value);
        }

        [Fact]
        public async Task UpdateInsurance_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IInsuranceService>();
            var controller = new InsuranceController(mockService.Object, null);

            int insuranceId = 1;
            var editInsurance = new EditInsurance { InsuranceFee = 100 };

            mockService.Setup(service => service.UpdateInsurance(editInsurance, insuranceId)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateInsurance(editInsurance, insuranceId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockService.Verify(service => service.UpdateInsurance(editInsurance, insuranceId), Times.Once);
        }
    }
}
