using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicle_Registration_System.Data;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Enums;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Repositories.Implementations;
using Xunit;

namespace Vehicle_Registration_System.Tests.RepositoryTests
{
    public class VehicleRepositoryTests
    {
        private DbContextOptions<AppDbContext> _options;

        public VehicleRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private async Task SeedDatabase(AppDbContext context)
        {
            context.Vehicles.AddRange(new List<Vehicle>
            {
                new Vehicle { Id = 1, Manufacturer = "Toyota", Model = "Camry", Year = 2020, Category = Category.Sedan, Transmission = Transmission.Automatic, Power = 200, Fuel = Fuel.Petrol, LicensePlate = "XYZ123", DateRegistered = DateTime.UtcNow, ExpirationDate = DateTime.UtcNow.AddYears(1), OwnerId = 1 },
                new Vehicle { Id = 2, Manufacturer = "Honda", Model = "Civic", Year = 2019, Category = Category.Sedan, Transmission = Transmission.Manual, Power = 180, Fuel = Fuel.Petrol, LicensePlate = "ABC789", DateRegistered = DateTime.UtcNow, ExpirationDate = DateTime.UtcNow.AddYears(1), OwnerId = 2 }
            });
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddVehicle_ShouldAddVehicle()
        {
            using (var context = new AppDbContext(_options))
            {
                var repository = new VehicleRepository(context);
                var request = new RegisterVehicle
                {
                    Manufacturer = "Ford",
                    Model = "Focus",
                    Year = 2021,
                    Category = Category.Hatchback,
                    Transmission = Transmission.Automatic,
                    Power = 150,
                    Fuel = Fuel.Diesel,
                    LicensePlate = "LMN456",
                    OwnerId = 3
                };

                await repository.AddVehicle(request);

                var vehicle = await context.Vehicles.FirstOrDefaultAsync(v =>
                    v.LicensePlate == request.LicensePlate &&
                    v.OwnerId == request.OwnerId);

                Assert.NotNull(vehicle);
                Assert.Equal(request.Manufacturer, vehicle.Manufacturer);
                Assert.Equal(request.Model, vehicle.Model);
                Assert.Equal(request.Year, vehicle.Year);
                Assert.Equal(request.Category, vehicle.Category);
                Assert.Equal(request.Transmission, vehicle.Transmission);
                Assert.Equal(request.Power, vehicle.Power);
                Assert.Equal(request.Fuel, vehicle.Fuel);
                Assert.Equal(request.LicensePlate, vehicle.LicensePlate);
                Assert.Equal(request.OwnerId, vehicle.OwnerId);
            }
        }

        [Fact]
        public async Task DeleteVehicle_ShouldDeleteVehicle()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                await repository.DeleteVehicle(1);
                var deletedVehicle = await context.Vehicles.FindAsync(1);
                Assert.Null(deletedVehicle);
            }
        }

        [Fact]
        public async Task GetAllVehicles_ShouldReturnAllVehicles()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var vehicles = await repository.GetAllVehicles();
                Assert.NotNull(vehicles);
            }
        }

        [Fact]
        public async Task GetVehicleById_ShouldReturnVehicle()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var vehicle = await repository.GetVehicleById(1);
                Assert.NotNull(vehicle);
            }
        }

        [Fact]
        public async Task UpdateVehicle_ShouldUpdateVehicle()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var updateRequest = new EditVehicle
                {
                    LicensePlate = "NEW123",
                    OwnerId = 3
                };

                await repository.UpdateVehicle(updateRequest, 1);
                var updatedVehicle = await context.Vehicles.FindAsync(1);

                Assert.NotNull(updatedVehicle);
                Assert.Equal(updateRequest.LicensePlate, updatedVehicle.LicensePlate);
                Assert.Equal(updateRequest.OwnerId, updatedVehicle.OwnerId);
            }
        }

        [Fact]
        public async Task FindAllByOwner_ShouldReturnVehicles()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var vehicles = await repository.FindAllByOwner(1);
                Assert.NotNull(vehicles);
            }
        }

        [Fact]
        public async Task FindByYear_ShouldReturnVehicles()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var vehicles = await repository.FindByYear(2020);
                Assert.NotNull(vehicles);
            }
        }

        [Fact]
        public async Task FindByHorsepower_ShouldReturnVehicles()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var vehicles = await repository.FindByHorsepower(180);
                Assert.NotNull(vehicles);
            }
        }

        [Fact]
        public async Task FindByFuelType_ShouldReturnVehicles()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var vehicles = await repository.FindByFuelType("Petrol");
                Assert.NotNull(vehicles);
            }
        }

        [Fact]
        public async Task FindByBrand_ShouldReturnVehicles()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var vehicles = await repository.FindByBrand("Toyota");
                Assert.NotNull(vehicles);
            }
        }

        [Fact]
        public async Task CountByBrand_ShouldReturnCount()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var count = await repository.CountByBrand("Toyota");
                Assert.Equal(1, count);
            }
        }

        [Fact]
        public async Task CountUnregistered_ShouldReturnCount()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var count = await repository.CountUnregistered();
                Assert.Equal(0, count);
            }
        }

        [Fact]
        public async Task CountRegistered_ShouldReturnCount()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var count = await repository.CountRegistered();
                Assert.Equal(2, count);
            }
        }

        [Fact]
        public async Task CountTransmission_ShouldReturnCount()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var count = await repository.CountTransmission("Automatic");
                Assert.Equal(1, count);
            }
        }

        [Fact]
        public async Task CheckRegistration_ShouldReturnTrueForRegistered()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new VehicleRepository(context);
                var isRegistered = await repository.CheckRegistration(1);
                Assert.True(isRegistered);
            }
        }
    }
}
