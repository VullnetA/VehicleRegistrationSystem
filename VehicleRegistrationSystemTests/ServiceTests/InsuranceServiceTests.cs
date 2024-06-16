using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Enums;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Repositories.Interfaces;
using Vehicle_Registration_System.Services.Implementations;
using Xunit;

namespace Vehicle_Registration_System.Tests.ServiceTests
{
    public class InsuranceServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IInsuranceRepository> _mockInsuranceRepository;
        private readonly Mock<IVehicleRepository> _mockVehicleRepository;
        private readonly InsuranceService _service;

        public InsuranceServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockInsuranceRepository = new Mock<IInsuranceRepository>();
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _service = new InsuranceService(_mockMapper.Object, _mockInsuranceRepository.Object, _mockVehicleRepository.Object);
        }

        [Fact]
        public async Task AddInsurance_ShouldThrowExceptionIfVehicleNotFound()
        {
            // Arrange
            var makeInsurance = new MakeInsurance { VehicleId = 1 };
            _mockVehicleRepository.Setup(repo => repo.GetVehicleById(makeInsurance.VehicleId)).ReturnsAsync((Vehicle)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.AddInsurance(makeInsurance));
        }

        [Fact]
        public async Task AddInsurance_ShouldCallRepositoryAddInsuranceMethod()
        {
            // Arrange
            var vehicle = new Vehicle { Id = 1, Category = Category.Sedan, Power = 150 };
            var makeInsurance = new MakeInsurance { VehicleId = vehicle.Id };

            _mockVehicleRepository.Setup(repo => repo.GetVehicleById(makeInsurance.VehicleId)).ReturnsAsync(vehicle);
            _mockInsuranceRepository.Setup(repo => repo.AddInsurance(makeInsurance, It.IsAny<float>())).Returns(Task.CompletedTask);

            // Act
            await _service.AddInsurance(makeInsurance);

            // Assert
            _mockInsuranceRepository.Verify(repo => repo.AddInsurance(makeInsurance, It.IsAny<float>()), Times.Once);
        }

        [Fact]
        public async Task CountInsurance_ShouldReturnCount()
        {
            // Arrange
            _mockInsuranceRepository.Setup(repo => repo.CountInsurance()).ReturnsAsync(100);

            // Act
            var result = await _service.CountInsurance();

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        public async Task DeleteInsurance_ShouldCallRepositoryDeleteInsuranceMethod()
        {
            // Arrange
            int id = 1;

            // Act
            await _service.DeleteInsurance(id);

            // Assert
            _mockInsuranceRepository.Verify(repo => repo.DeleteInsurance(id), Times.Once);
        }

        [Fact]
        public async Task FindExpiredInsurance_ShouldReturnMappedVehicles()
        {
            // Arrange
            var expiredInsurances = new List<Vehicle> { new Vehicle() };
            var vehicleDtos = new List<VehicleDto> { new VehicleDto() };

            _mockInsuranceRepository.Setup(repo => repo.FindExpiredInsurance()).ReturnsAsync(expiredInsurances);
            _mockMapper.Setup(mapper => mapper.Map<VehicleDto>(It.IsAny<Vehicle>())).Returns(vehicleDtos.First());

            // Act
            var result = await _service.FindExpiredInsurance();

            // Assert
            Assert.Equal(vehicleDtos.Count, result.Count());
        }

        [Fact]
        public async Task GetAllInsurances_ShouldReturnMappedInsurances()
        {
            // Arrange
            var insurances = new List<Insurance> { new Insurance() };
            var insuranceDtos = new List<InsuranceDto> { new InsuranceDto() };

            _mockInsuranceRepository.Setup(repo => repo.GetAllInsurances()).ReturnsAsync(insurances);
            _mockMapper.Setup(mapper => mapper.Map<InsuranceDto>(It.IsAny<Insurance>())).Returns(insuranceDtos.First());

            // Act
            var result = await _service.GetAllInsurances();

            // Assert
            Assert.Equal(insuranceDtos.Count, result.Count());
        }

        [Fact]
        public async Task UpdateInsurance_ShouldCallRepositoryUpdateInsuranceMethod()
        {
            // Arrange
            var editInsurance = new EditInsurance();
            int id = 1;

            // Act
            await _service.UpdateInsurance(editInsurance, id);

            // Assert
            _mockInsuranceRepository.Verify(repo => repo.UpdateInsurance(editInsurance, id), Times.Once);
        }

        [Fact]
        public void CalculateInsuranceFee_ShouldReturnCorrectFee()
        {
            // Arrange
            var vehicle = new Vehicle { Category = Category.Sedan, Power = 150 };
            var expectedFee = 3700; // Based on the given fee structure

            // Act
            var fee = _service.CalculateInsuranceFee(vehicle);

            // Assert
            Assert.Equal(expectedFee, fee);
        }
    }
}
