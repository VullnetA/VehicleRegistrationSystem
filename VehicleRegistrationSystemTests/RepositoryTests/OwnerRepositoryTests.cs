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
    public class OwnerRepositoryTests
    {
        private DbContextOptions<AppDbContext> _options;

        public OwnerRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private async Task SeedDatabase(AppDbContext context)
        {
            context.Owners.AddRange(new List<Owner>
            {
                new Owner { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1980, 1, 1), PlaceOfBirth = "CityA", Email = "john.doe@example.com", Phone = "1234567890", Gender = Gender.Male, Address = "123 Main St", LicenseIssueDate = new DateTime(2000, 1, 1) },
                new Owner { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1990, 1, 1), PlaceOfBirth = "CityB", Email = "jane.smith@example.com", Phone = "0987654321", Gender = Gender.Female, Address = "456 Elm St", LicenseIssueDate = new DateTime(2010, 1, 1) }
            });

            context.Vehicles.AddRange(new List<Vehicle>
            {
                new Vehicle { Id = 1, Manufacturer = "Toyota", Model = "Camry", Year = 2020, Category = Category.Sedan, Transmission = Transmission.Automatic, Power = 200, Fuel = Fuel.Petrol, LicensePlate = "XYZ123", DateRegistered = DateTime.UtcNow, ExpirationDate = DateTime.UtcNow.AddYears(1), OwnerId = 1 },
                new Vehicle { Id = 2, Manufacturer = "Honda", Model = "Civic", Year = 2019, Category = Category.Sedan, Transmission = Transmission.Manual, Power = 180, Fuel = Fuel.Petrol, LicensePlate = "ABC789", DateRegistered = DateTime.UtcNow, ExpirationDate = DateTime.UtcNow.AddYears(1), OwnerId = 2 }
            });

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddOwner_ShouldAddOwner()
        {
            using (var context = new AppDbContext(_options))
            {
                var repository = new OwnerRepository(context);
                var request = new InputOwner
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    DateOfBirth = new DateTime(1995, 5, 10),
                    PlaceOfBirth = "CityC",
                    Email = "alice.johnson@example.com",
                    Phone = "1122334455",
                    Gender = Gender.Female,
                    Address = "789 Oak St",
                    LicenseIssueDate = new DateTime(2015, 5, 10)
                };

                await repository.AddOwner(request);

                var owner = await context.Owners.FirstOrDefaultAsync(o => o.Email == request.Email);
                Assert.NotNull(owner);
                Assert.Equal(request.FirstName, owner.FirstName);
                Assert.Equal(request.LastName, owner.LastName);
                Assert.Equal(request.DateOfBirth, owner.DateOfBirth);
                Assert.Equal(request.PlaceOfBirth, owner.PlaceOfBirth);
                Assert.Equal(request.Email, owner.Email);
                Assert.Equal(request.Phone, owner.Phone);
                Assert.Equal(request.Gender, owner.Gender);
                Assert.Equal(request.Address, owner.Address);
                Assert.Equal(request.LicenseIssueDate, owner.LicenseIssueDate);
            }
        }

        [Fact]
        public async Task DeleteOwner_ShouldDeleteOwner()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new OwnerRepository(context);
                await repository.DeleteOwner(1);
                var deletedOwner = await context.Owners.FindAsync(1);
                Assert.Null(deletedOwner);
            }
        }

        [Fact]
        public async Task FindByCity_ShouldReturnOwners()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new OwnerRepository(context);
                var owners = await repository.FindByCity("CityA");
                Assert.NotNull(owners);
                Assert.Single(owners);
            }
        }

        [Fact]
        public async Task FindOwnerByVehicle_ShouldReturnOwners()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new OwnerRepository(context);
                var owners = await repository.FindOwnerByVehicle("Toyota", "Camry");
                Assert.NotNull(owners);
                Assert.Single(owners);
            }
        }

        [Fact]
        public async Task GetAllOwners_ShouldReturnAllOwners()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new OwnerRepository(context);
                var owners = await repository.GetAllOwners();
                Assert.NotNull(owners);
                Assert.Equal(2, owners.Count());
            }
        }

        [Fact]
        public async Task GetOwnerById_ShouldReturnOwner()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new OwnerRepository(context);
                var owner = await repository.GetOwnerById(1);
                Assert.NotNull(owner);
                Assert.Equal(1, owner.Id);
            }
        }

        [Fact]
        public async Task GetLicensesByCity_ShouldReturnCount()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new OwnerRepository(context);
                var count = await repository.GetLicensesByCity("CityA");
                Assert.Equal(1, count);
            }
        }

        [Fact]
        public async Task UpdateOwner_ShouldUpdateOwner()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new OwnerRepository(context);
                var updateRequest = new EditOwner
                {
                    Phone = "9998887777",
                    Address = "New Address"
                };

                await repository.UpdateOwner(updateRequest, 1);
                var updatedOwner = await context.Owners.FindAsync(1);

                Assert.NotNull(updatedOwner);
                Assert.Equal(updateRequest.Phone, updatedOwner.Phone);
                Assert.Equal(updateRequest.Address, updatedOwner.Address);
            }
        }
    }
}
