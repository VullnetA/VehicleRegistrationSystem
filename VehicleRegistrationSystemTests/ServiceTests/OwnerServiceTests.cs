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
    public class OwnerServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IOwnerRepository> _mockRepository;
        private readonly OwnerService _service;

        public OwnerServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IOwnerRepository>();
            _service = new OwnerService(_mockMapper.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task AddOwner_ShouldCallRepositoryAddOwnerMethod()
        {
            // Arrange
            var inputOwner = new InputOwner();

            // Act
            await _service.AddOwner(inputOwner);

            // Assert
            _mockRepository.Verify(repo => repo.AddOwner(inputOwner), Times.Once);
        }

        [Fact]
        public async Task DeleteOwner_ShouldCallRepositoryDeleteOwnerMethod()
        {
            // Arrange
            int id = 1;

            // Act
            await _service.DeleteOwner(id);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteOwner(id), Times.Once);
        }

        [Fact]
        public async Task FindByCity_ShouldReturnMappedOwners()
        {
            // Arrange
            string placeOfBirth = "CityName";
            var owners = new List<Owner> { new Owner() };
            var ownerDtos = new List<OwnerDto> { new OwnerDto() };

            _mockRepository.Setup(repo => repo.FindByCity(placeOfBirth)).ReturnsAsync(owners);
            _mockMapper.Setup(mapper => mapper.Map<OwnerDto>(It.IsAny<Owner>())).Returns(ownerDtos.First());

            // Act
            var result = await _service.FindByCity(placeOfBirth);

            // Assert
            Assert.Equal(ownerDtos.Count, result.Count());
        }

        [Fact]
        public async Task FindOwnerByVehicle_ShouldReturnMappedOwners()
        {
            // Arrange
            string manufacturer = "Toyota";
            string model = "Corolla";
            var owners = new List<Owner> { new Owner() };
            var ownerDtos = new List<OwnerDto> { new OwnerDto() };

            _mockRepository.Setup(repo => repo.FindOwnerByVehicle(manufacturer, model)).ReturnsAsync(owners);
            _mockMapper.Setup(mapper => mapper.Map<OwnerDto>(It.IsAny<Owner>())).Returns(ownerDtos.First());

            // Act
            var result = await _service.FindOwnerByVehicle(manufacturer, model);

            // Assert
            Assert.Equal(ownerDtos.Count, result.Count());
        }

        [Fact]
        public async Task GetAllOwners_ShouldReturnMappedOwners()
        {
            // Arrange
            var owners = new List<Owner> { new Owner() };
            var ownerDtos = new List<OwnerDto> { new OwnerDto() };

            _mockRepository.Setup(repo => repo.GetAllOwners()).ReturnsAsync(owners);
            _mockMapper.Setup(mapper => mapper.Map<OwnerDto>(It.IsAny<Owner>())).Returns(ownerDtos.First());

            // Act
            var result = await _service.GetAllOwners();

            // Assert
            Assert.Equal(ownerDtos.Count, result.Count());
        }

        [Fact]
        public async Task GetLicensesByCity_ShouldReturnCount()
        {
            // Arrange
            string placeOfBirth = "CityName";
            _mockRepository.Setup(repo => repo.GetLicensesByCity(placeOfBirth)).ReturnsAsync(100);

            // Act
            var result = await _service.GetLicensesByCity(placeOfBirth);

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        public async Task UpdateOwner_ShouldCallRepositoryUpdateOwnerMethod()
        {
            // Arrange
            var editOwner = new EditOwner();
            int id = 1;

            // Act
            await _service.UpdateOwner(editOwner, id);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateOwner(editOwner, id), Times.Once);
        }
    }
}