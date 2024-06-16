using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetAllVehicles();
        Task<VehicleDto> GetVehicleById(int id);
        Task AddVehicle(RegisterVehicle register);
        Task UpdateVehicle(EditVehicle edit, int id);
        Task DeleteVehicle(int id);
        Task<IEnumerable<VehicleDto>> FindAllByOwner(int ownerId);
        Task<IEnumerable<VehicleDto>> FindByYear(int year);
        Task<IEnumerable<VehicleDto>> FindByHorsepower(int power);
        Task<IEnumerable<VehicleDto>> FindByFuelType(string fuel);
        Task<IEnumerable<VehicleDto>> FindByBrand(string manufacturer);
        Task<long> CountByBrand(string manufacturer);
        Task<long> CountUnregistered();
        Task<long> CountRegistered();
        Task<long> CountTransmission(string transmission);
        Task<VehicleDto> FindByLicensePlate(string licensePlate);
        Task<bool> CheckRegistration(int id);
    }
}
