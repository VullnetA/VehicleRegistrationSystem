using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Vehicle_Registration_System.Data;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Enums;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Repositories.Interfaces;

namespace Vehicle_Registration_System.Repositories.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddVehicle(RegisterVehicle register)
        {
            Vehicle requestBody = new Vehicle();
            requestBody.Manufacturer = register.Manufacturer;
            requestBody.Model = register.Model;
            requestBody.Year = register.Year;
            requestBody.Category = register.Category;
            requestBody.Transmission = register.Transmission;
            requestBody.Power = register.Power;
            requestBody.Fuel = register.Fuel;
            requestBody.LicensePlate = register.LicensePlate;
            requestBody.DateRegistered = DateTime.UtcNow;
            requestBody.ExpirationDate = DateTime.UtcNow.AddYears(1);
            requestBody.OwnerId = register.OwnerId;

            _context.Vehicles.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehicles()
        {
            return await _context.Vehicles
                                 .Include(v => v.Owner)
                                 .ToListAsync();
        }


        public async Task<Vehicle> GetVehicleById(int id)
        {
            var vehicle = await _context.Vehicles
                                        .Include(v => v.Owner)
                                        .FirstOrDefaultAsync(v => v.Id == id);
            return vehicle ?? new Vehicle();
        }


        public async Task UpdateVehicle(EditVehicle edit, int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if(vehicle != null)
            {
                vehicle.LicensePlate = edit.LicensePlate;
                vehicle.OwnerId = edit.OwnerId;
                vehicle.DateRegistered = DateTime.UtcNow;
                vehicle.ExpirationDate = DateTime.UtcNow.AddYears(1);

                _context.Entry(vehicle).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vehicle>> FindAllByOwner(int ownerId)
        {
            return await _context.Vehicles
                                 .Include(v => v.Owner)
                                 .Where(v => v.OwnerId == ownerId)
                                 .ToListAsync();
        }


        public async Task<IEnumerable<Vehicle>> FindByYear(int year)
        {
            return await _context.Vehicles
                                 .Include(v => v.Owner)
                                 .Where(v => v.Year == year)
                                 .ToListAsync();
        }


        public async Task<IEnumerable<Vehicle>> FindByHorsepower(int power)
        {
            return await _context.Vehicles
                                 .Include(v => v.Owner)
                                 .Where(v => v.Power > power)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> FindByFuelType(string fuel)
        {
            if (!Enum.TryParse<Fuel>(fuel, out var fuelEnum))
            {
                return new List<Vehicle>();
            }

            return await _context.Vehicles
                                 .Include(v => v.Owner)
                                 .Where(v => v.Fuel == fuelEnum)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> FindByBrand(string manufacturer)
        {
            return await _context.Vehicles
                                 .Include(v => v.Owner)
                                 .Where(v => v.Manufacturer.Equals(manufacturer))
                                 .ToListAsync();
        }


        public async Task<long> CountByBrand(string manufacturer)
        {
            return await _context.Vehicles
                                 .CountAsync(v => v.Manufacturer.Equals(manufacturer));
        }

        public async Task<long> CountUnregistered()
        {
            return await _context.Vehicles
                                 .CountAsync(v => v.ExpirationDate < DateTime.UtcNow);
        }

        public async Task<long> CountRegistered()
        {
            return await _context.Vehicles
                                 .CountAsync(v => v.ExpirationDate > DateTime.UtcNow);
        }

        public async Task<long> CountTransmission(string transmission)
        {
            if (!Enum.TryParse<Transmission>(transmission, out var transmissionEnum))
            {
                return 0;
            }

            return await _context.Vehicles
                                 .CountAsync(v => v.Transmission == transmissionEnum);
        }


        public async Task<Vehicle> FindByLicensePlate(string licensePlate)
        {
            return await _context.Vehicles
                                 .Include(v => v.Owner)
                                 .FirstOrDefaultAsync(v => v.LicensePlate.Equals(licensePlate));
        }


        public async Task<bool> CheckRegistration(int id)
        {
            return await _context.Vehicles
                         .AnyAsync(v => v.Id == id && v.ExpirationDate > DateTime.UtcNow);
        }
    }
}