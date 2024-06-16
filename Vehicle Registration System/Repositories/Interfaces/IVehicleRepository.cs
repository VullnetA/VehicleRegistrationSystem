using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllVehicles();
        Task<Vehicle> GetVehicleById(int id);
        Task AddVehicle(RegisterVehicle register);
        Task UpdateVehicle(EditVehicle edit, int id);
        Task DeleteVehicle(int id);
        Task<IEnumerable<Vehicle>> FindAllByOwner(int ownerId);
        Task<IEnumerable<Vehicle>> FindByYear(int year);
        Task<IEnumerable<Vehicle>> FindByHorsepower(int power);
        Task<IEnumerable<Vehicle>> FindByFuelType(string fuel);
        Task<IEnumerable<Vehicle>> FindByBrand(string manufacturer);
        Task<long> CountByBrand(string manufacturer);
        Task<long> CountUnregistered();
        Task<long> CountRegistered();
        Task<long> CountTransmission(string transmission);
        Task<Vehicle> FindByLicensePlate(string licensePlate);
        Task<bool> CheckRegistration(int id);
    }
}
