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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Vehicle_Registration_System.Tests.ControllerTests
{
    public class OwnerControllerTests
    {
        private UserManager<ApplicationUser> GetUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null).Object;
        }

        private RoleManager<IdentityRole> GetRoleManagerMock()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(
                store.Object, null, null, null, null).Object;
        }

        private OwnerController SetupController(Mock<IOwnerService> mockService, Mock<IVehicleService> mockVehicleService, Mock<IMemoryCache> mockMemoryCache, UserManager<ApplicationUser> mockUserManager, RoleManager<IdentityRole> mockRoleManager)
        {
            var controller = new OwnerController(mockService.Object, mockVehicleService.Object, mockMemoryCache.Object, mockUserManager, mockRoleManager)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "test@user.com") }, "mock")) }
                }
            };
            return controller;
        }

        [Fact]
        public async Task GetAllOwners_ShouldReturnOkWithOwnersFromCache()
        {
            // Arrange
            var mockService = new Mock<IOwnerService>();
            var mockVehicleService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var mockUserManager = GetUserManagerMock();
            var mockRoleManager = GetRoleManagerMock();
            var controller = SetupController(mockService, mockVehicleService, mockMemoryCache, mockUserManager, mockRoleManager);

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
            var mockVehicleService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null, null, null, null, null, null, null, null);
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                null, null, null, null);
            var controller = SetupController(mockService, mockVehicleService, mockMemoryCache, mockUserManager.Object, mockRoleManager.Object);

            var inputOwner = new InputOwner { FirstName = "Owner", Email = "owner@test.com", Password = "Password123!" };
            var createdOwner = new Owner { Id = 1, FirstName = "Owner" };

            mockService.Setup(service => service.AddOwner(inputOwner)).ReturnsAsync(createdOwner);
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), inputOwner.Password)).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Owner")).ReturnsAsync(IdentityResult.Success);
            mockRoleManager.Setup(rm => rm.RoleExistsAsync("Owner")).ReturnsAsync(false);
            mockRoleManager.Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.AddOwner(inputOwner);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualValue = okResult.Value as IDictionary<string, object>;
            Assert.Null(actualValue);

            mockService.Verify(service => service.AddOwner(inputOwner), Times.Once);
            mockUserManager.Verify(um => um.CreateAsync(It.IsAny<ApplicationUser>(), inputOwner.Password), Times.Once);
            mockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Owner"), Times.Once);
        }


        [Fact]
        public async Task DeleteOwner_ShouldReturnOk()
        {
            // Arrange
            var mockService = new Mock<IOwnerService>();
            var mockVehicleService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var mockUserManager = GetUserManagerMock();
            var mockRoleManager = GetRoleManagerMock();
            var controller = SetupController(mockService, mockVehicleService, mockMemoryCache, mockUserManager, mockRoleManager);

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
            var mockVehicleService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var mockUserManager = GetUserManagerMock();
            var mockRoleManager = GetRoleManagerMock();
            var controller = SetupController(mockService, mockVehicleService, mockMemoryCache, mockUserManager, mockRoleManager);

            int ownerId = 1;
            var editOwner = new EditOwner { Address = "Address1" };

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
            var mockVehicleService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var mockUserManager = GetUserManagerMock();
            var mockRoleManager = GetRoleManagerMock();
            var controller = SetupController(mockService, mockVehicleService, mockMemoryCache, mockUserManager, mockRoleManager);

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
            var mockVehicleService = new Mock<IVehicleService>();
            var mockMemoryCache = new Mock<IMemoryCache>();
            var mockUserManager = GetUserManagerMock();
            var mockRoleManager = GetRoleManagerMock();
            var controller = SetupController(mockService, mockVehicleService, mockMemoryCache, mockUserManager, mockRoleManager);

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
