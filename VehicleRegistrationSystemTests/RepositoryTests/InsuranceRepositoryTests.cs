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
    public class InsuranceRepositoryTests
    {
        private DbContextOptions<AppDbContext> _options;

        public InsuranceRepositoryTests()
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

            context.Insurances.AddRange(new List<Insurance>
            {
                new Insurance { Id = 1, InsuranceCompany = InsuranceCompany.UNIQA, InsuranceFee = 500, VehicleId = 1, DateRegistered = DateTime.UtcNow, ExpirationDate = DateTime.UtcNow.AddYears(1) },
                new Insurance { Id = 2, InsuranceCompany = InsuranceCompany.UNIQA, InsuranceFee = 600, VehicleId = 2, DateRegistered = DateTime.UtcNow, ExpirationDate = DateTime.UtcNow.AddYears(1) }
            });

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddInsurance_ShouldAddInsurance()
        {
            using (var context = new AppDbContext(_options))
            {
                var repository = new InsuranceRepository(context);
                var request = new MakeInsurance
                {
                    InsuranceCompany = InsuranceCompany.UNIQA,
                    VehicleId = 1
                };

                float fee = 700;

                await repository.AddInsurance(request, fee);

                var insurance = await context.Insurances.FirstOrDefaultAsync(i => i.InsuranceCompany == request.InsuranceCompany && i.InsuranceFee == fee);
                Assert.NotNull(insurance);
                Assert.Equal(request.InsuranceCompany, insurance.InsuranceCompany);
                Assert.Equal(fee, insurance.InsuranceFee);
                Assert.Equal(request.VehicleId, insurance.VehicleId);
            }
        }

        [Fact]
        public async Task CountInsurance_ShouldReturnCount()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new InsuranceRepository(context);
                var count = await repository.CountInsurance();
                Assert.Equal(2, count);
            }
        }

        [Fact]
        public async Task DeleteInsurance_ShouldDeleteInsurance()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new InsuranceRepository(context);
                await repository.DeleteInsurance(1);
                var deletedInsurance = await context.Insurances.FindAsync(1);
                Assert.Null(deletedInsurance);
            }
        }

        [Fact]
        public async Task FindExpiredInsurance_ShouldReturnExpiredInsurances()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new InsuranceRepository(context);
                context.Insurances.First(i => i.Id == 1).ExpirationDate = DateTime.UtcNow.AddDays(-1);
                await context.SaveChangesAsync();

                var expiredInsurances = await repository.FindExpiredInsurance();
                Assert.NotNull(expiredInsurances);
            }
        }

        [Fact]
        public async Task FindInsuranceById_ShouldReturnInsurance()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new InsuranceRepository(context);
                var insurance = await repository.FindInsuranceById(1);
                Assert.NotNull(insurance);
            }
        }

        [Fact]
        public async Task GetAllInsurances_ShouldReturnAllInsurances()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new InsuranceRepository(context);
                var insurances = await repository.GetAllInsurances();
                Assert.NotNull(insurances);
            }
        }

        [Fact]
        public async Task UpdateInsurance_ShouldUpdateInsurance()
        {
            using (var context = new AppDbContext(_options))
            {
                await SeedDatabase(context);
                var repository = new InsuranceRepository(context);
                var updateRequest = new EditInsurance
                {
                    InsuranceCompany = InsuranceCompany.UNIQA,
                    InsuranceFee = 800
                };

                await repository.UpdateInsurance(updateRequest, 1);
                var updatedInsurance = await context.Insurances.FindAsync(1);

                Assert.NotNull(updatedInsurance);
                Assert.Equal(updateRequest.InsuranceCompany, updatedInsurance.InsuranceCompany);
                Assert.Equal(updateRequest.InsuranceFee, updatedInsurance.InsuranceFee);
            }
        }
    }
}
