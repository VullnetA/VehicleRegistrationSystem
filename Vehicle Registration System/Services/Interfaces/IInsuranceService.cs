using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.Services.Interfaces
{
    public interface IInsuranceService
    {
        Task<IEnumerable<InsuranceDto>> GetAllInsurances();
        Task<InsuranceDto> FindInsuranceById(int id);
        Task AddInsurance(MakeInsurance make);
        Task UpdateInsurance(EditInsurance edit, int id);
        Task DeleteInsurance(int id);
        Task<long> CountInsurance();
        Task<IEnumerable<VehicleDto>> FindExpiredInsurance();
    }
}
