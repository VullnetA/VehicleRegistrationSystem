using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<OwnerDto>> GetAllOwners();
        Task<OwnerDto> GetOwnerById(int id);
        Task<IEnumerable<OwnerDto>> GetOwnersByName(string name);
        Task AddOwner(InputOwner input);
        Task UpdateOwner(EditOwner edit, int id);
        Task DeleteOwner(int id);
        Task<IEnumerable<OwnerDto>> FindOwnerByVehicle(string manufacturer, string model);
        Task<long> GetLicensesByCity(string placeOfBirth);
        Task<IEnumerable<OwnerDto>> FindByCity(string placeOfBirth);
    }
}
