using Microsoft.EntityFrameworkCore;
using Vehicle_Registration_System.Data;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Enums;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Repositories.Interfaces;

namespace Vehicle_Registration_System.Repositories.Implementations
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly AppDbContext _context;

        public InsuranceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddInsurance(MakeInsurance make, float fee)
        {
            Insurance requestBody = new Insurance();

            requestBody.InsuranceCompany = make.InsuranceCompany;
            requestBody.InsuranceFee = fee;
            requestBody.VehicleId = make.VehicleId;
            requestBody.DateRegistered = DateTime.UtcNow;
            requestBody.ExpirationDate = DateTime.UtcNow.AddYears(1);

            _context.Insurances.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task<long> CountInsurance()
        {
            return await _context.Insurances.CountAsync();
        }

        public async Task DeleteInsurance(int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);
            if (insurance != null)
            {
                _context.Insurances.Remove(insurance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vehicle>> FindExpiredInsurance()
        {
            return await _context.Vehicles
                                 .Include(v => v.Owner)
                                 .Include(v => v.Insurance)
                                 .Where(v => v.Insurance.ExpirationDate < DateTime.UtcNow)
                                 .ToListAsync();
        }

        public async Task<Insurance> FindInsuranceById(int id)
        {
            return await _context.Insurances
                                 .Include(i => i.Vehicle)
                                 .ThenInclude(v => v.Owner)
                                 .FirstOrDefaultAsync(i => i.Id == id) ?? new Insurance();
        }

        public async Task<Insurance> FindInsuranceByVehicleId(int vehicleId)
        {
            return await _context.Insurances
                                 .Include(i => i.Vehicle)
                                 .ThenInclude(v => v.Owner)
                                 .FirstOrDefaultAsync(i => i.Vehicle.Id == vehicleId) ?? new Insurance();
        }

        public async Task<IEnumerable<Insurance>> GetAllInsurances()
        {
            return await _context.Insurances
                                 .Include(i => i.Vehicle)
                                 .ThenInclude(v => v.Owner)
                                 .ToListAsync();
        }


        public async Task UpdateInsurance(EditInsurance edit, int id)
        {
            var insurance = await _context.Insurances.FindAsync(id);

            if(insurance != null)
            {
                insurance.InsuranceCompany = edit.InsuranceCompany;
                insurance.InsuranceFee = edit.InsuranceFee;
                insurance.DateRegistered = DateTime.UtcNow;
                insurance.ExpirationDate = DateTime.UtcNow.AddYears(1);

                _context.Entry(insurance).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
