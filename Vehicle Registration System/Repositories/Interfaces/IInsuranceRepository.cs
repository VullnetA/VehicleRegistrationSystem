using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.Repositories.Interfaces
{
    public interface IInsuranceRepository
    {
        Task<IEnumerable<Insurance>> GetAllInsurances();
        Task<Insurance> FindInsuranceById(int id);
        Task<Insurance> FindInsuranceByVehicleId(int id);
        Task AddInsurance(MakeInsurance make, float fee);
        Task UpdateInsurance(EditInsurance edit, int id);
        Task DeleteInsurance(int id);
        Task<long> CountInsurance();
        Task<IEnumerable<Vehicle>> FindExpiredInsurance();
    }
}
