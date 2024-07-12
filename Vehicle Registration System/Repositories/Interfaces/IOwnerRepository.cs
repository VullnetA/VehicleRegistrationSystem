using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.Repositories.Interfaces
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllOwners();
        Task<Owner> GetOwnerById(int id);
        Task<IEnumerable<Owner>> GetOwnersByName(string name);
        Task AddOwner(InputOwner input);
        Task UpdateOwner(EditOwner edit, int id);
        Task DeleteOwner(int id);
        Task<IEnumerable<Owner>> FindOwnerByVehicle(string manufacturer, string model);
        Task<long> GetLicensesByCity(string placeOfBirth);
        Task<IEnumerable<Owner>> FindByCity(string placeOfBirth); 
    }
}
