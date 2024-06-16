using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Repositories.Interfaces;
using Vehicle_Registration_System.Services.Implementations;
using Xunit;

namespace Vehicle_Registration_System.Tests.ServiceTests
{
    public class VehicleServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IVehicleRepository> _mockRepository;
        private readonly VehicleService _service;

        public VehicleServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IVehicleRepository>();
            _service = new VehicleService(_mockMapper.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task AddVehicle_ShouldCallRepositoryAddVehicleMethod()
        {
            // Arrange
            var registerVehicle = new RegisterVehicle();

            // Act
            await _service.AddVehicle(registerVehicle);

            // Assert
            _mockRepository.Verify(repo => repo.AddVehicle(registerVehicle), Times.Once);
        }

        [Fact]
        public async Task CheckRegistration_ShouldReturnTrueIfRegistered()
        {
            // Arrange
            int id = 1;
            _mockRepository.Setup(repo => repo.CheckRegistration(id)).ReturnsAsync(true);

            // Act
            var result = await _service.CheckRegistration(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CountByBrand_ShouldReturnCount()
        {
            // Arrange
            string manufacturer = "Toyota";
            _mockRepository.Setup(repo => repo.CountByBrand(manufacturer)).ReturnsAsync(10);

            // Act
            var result = await _service.CountByBrand(manufacturer);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task CountRegistered_ShouldReturnCount()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.CountRegistered()).ReturnsAsync(100);

            // Act
            var result = await _service.CountRegistered();

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        public async Task CountTransmission_ShouldReturnCount()
        {
            // Arrange
            string transmission = "Automatic";
            _mockRepository.Setup(repo => repo.CountTransmission(transmission)).ReturnsAsync(50);

            // Act
            var result = await _service.CountTransmission(transmission);

            // Assert
            Assert.Equal(50, result);
        }

        [Fact]
        public async Task CountUnregistered_ShouldReturnCount()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.CountUnregistered()).ReturnsAsync(20);

            // Act
            var result = await _service.CountUnregistered();

            // Assert
            Assert.Equal(20, result);
        }

        [Fact]
        public async Task DeleteVehicle_ShouldCallRepositoryDeleteVehicleMethod()
        {
            // Arrange
            int id = 1;

            // Act
            await _service.DeleteVehicle(id);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteVehicle(id), Times.Once);
        }

        [Fact]
        public async Task FindAllByOwner_ShouldReturnMappedVehicles()
        {
            // Arrange
            int ownerId = 1;
            var vehicles = new List<Vehicle> { new Vehicle() };
            var vehicleDtos = new List<VehicleDto> { new VehicleDto() };

            _mockRepository.Setup(repo => repo.FindAllByOwner(ownerId)).ReturnsAsync(vehicles);
            _mockMapper.Setup(mapper => mapper.Map<VehicleDto>(It.IsAny<Vehicle>())).Returns(vehicleDtos.First());

            // Act
            var result = await _service.FindAllByOwner(ownerId);

            // Assert
            Assert.Equal(vehicleDtos.Count, result.Count());
        }

        [Fact]
        public async Task FindByBrand_ShouldReturnMappedVehicles()
        {
            // Arrange
            string brand = "Toyota";
            var vehicles = new List<Vehicle> { new Vehicle() };
            var vehicleDtos = new List<VehicleDto> { new VehicleDto() };

            _mockRepository.Setup(repo => repo.FindByBrand(brand)).ReturnsAsync(vehicles);
            _mockMapper.Setup(mapper => mapper.Map<VehicleDto>(It.IsAny<Vehicle>())).Returns(vehicleDtos.First());

            // Act
            var result = await _service.FindByBrand(brand);

            // Assert
            Assert.Equal(vehicleDtos.Count, result.Count());
        }

        [Fact]
        public async Task FindByFuelType_ShouldReturnMappedVehicles()
        {
            // Arrange
            string fuelType = "Diesel";
            var vehicles = new List<Vehicle> { new Vehicle() };
            var vehicleDtos = new List<VehicleDto> { new VehicleDto() };

            _mockRepository.Setup(repo => repo.FindByFuelType(fuelType)).ReturnsAsync(vehicles);
            _mockMapper.Setup(mapper => mapper.Map<VehicleDto>(It.IsAny<Vehicle>())).Returns(vehicleDtos.First());

            // Act
            var result = await _service.FindByFuelType(fuelType);

            // Assert
            Assert.Equal(vehicleDtos.Count, result.Count());
        }

        [Fact]
        public async Task FindByHorsepower_ShouldReturnMappedVehicles()
        {
            // Arrange
            int horsepower = 100;
            var vehicles = new List<Vehicle> { new Vehicle() };
            var vehicleDtos = new List<VehicleDto> { new VehicleDto() };

            _mockRepository.Setup(repo => repo.FindByHorsepower(horsepower)).ReturnsAsync(vehicles);
            _mockMapper.Setup(mapper => mapper.Map<VehicleDto>(It.IsAny<Vehicle>())).Returns(vehicleDtos.First());

            // Act
            var result = await _service.FindByHorsepower(horsepower);

            // Assert
            Assert.Equal(vehicleDtos.Count, result.Count());
        }

        [Fact]
        public async Task FindByYear_ShouldReturnMappedVehicles()
        {
            // Arrange
            int year = 2020;
            var vehicles = new List<Vehicle> { new Vehicle() };
            var vehicleDtos = new List<VehicleDto> { new VehicleDto() };

            _mockRepository.Setup(repo => repo.FindByYear(year)).ReturnsAsync(vehicles);
            _mockMapper.Setup(mapper => mapper.Map<VehicleDto>(It.IsAny<Vehicle>())).Returns(vehicleDtos.First());

            // Act
            var result = await _service.FindByYear(year);

            // Assert
            Assert.Equal(vehicleDtos.Count, result.Count());
        }

        [Fact]
        public async Task GetAllVehicles_ShouldReturnMappedVehicles()
        {
            // Arrange
            var vehicles = new List<Vehicle> { new Vehicle() };
            var vehicleDtos = new List<VehicleDto> { new VehicleDto() };

            _mockRepository.Setup(repo => repo.GetAllVehicles()).ReturnsAsync(vehicles);
            _mockMapper.Setup(mapper => mapper.Map<VehicleDto>(It.IsAny<Vehicle>())).Returns(vehicleDtos.First());

            // Act
            var result = await _service.GetAllVehicles();

            // Assert
            Assert.Equal(vehicleDtos.Count, result.Count());
        }

        [Fact]
        public async Task UpdateVehicle_ShouldCallRepositoryUpdateVehicleMethod()
        {
            // Arrange
            var editVehicle = new EditVehicle();
            int id = 1;

            // Act
            await _service.UpdateVehicle(editVehicle, id);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateVehicle(editVehicle, id), Times.Once);
        }
    }
}
