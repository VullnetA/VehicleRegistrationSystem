using AutoMapper;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Enums;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Repositories.Interfaces;
using Vehicle_Registration_System.Services.Interfaces;

namespace Vehicle_Registration_System.Services.Implementations
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IMapper _mapper;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public InsuranceService(IMapper mapper, IInsuranceRepository insuranceRepository, IVehicleRepository vehicleRepository)
        {
            _mapper = mapper;
            _insuranceRepository = insuranceRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task AddInsurance(MakeInsurance make)
        {
            var vehicle = await _vehicleRepository.GetVehicleById(make.VehicleId);
            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            }

            float fee = CalculateInsuranceFee(vehicle);

            await _insuranceRepository.AddInsurance(make, fee);
        }

        public async Task<long> CountInsurance()
        {
            return await _insuranceRepository.CountInsurance();
        }

        public async Task DeleteInsurance(int id)
        {
            await _insuranceRepository.DeleteInsurance(id);
        }

        public async Task<IEnumerable<VehicleDto>> FindExpiredInsurance()
        {
            var expiredInsurances = await _insuranceRepository.FindExpiredInsurance();
            if (expiredInsurances == null) return Enumerable.Empty<VehicleDto>();

            return expiredInsurances.Select(element =>
            {
                var vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
        }

        public async Task<InsuranceDto> FindInsuranceById(int id)
        {
            var insurance = await _insuranceRepository.FindInsuranceById(id);
            if (insurance == null) return null;

            var insuranceDto = new InsuranceDto();
            return _mapper.Map(insurance, insuranceDto);
        }

        public async Task<InsuranceDto> FindInsuranceByVehicleId(int id)
        {
            var insurance = await _insuranceRepository.FindInsuranceByVehicleId(id);
            if (insurance == null) return null;

            var insuranceDto = new InsuranceDto();
            return _mapper.Map(insurance, insuranceDto);
        }

        public async Task<IEnumerable<InsuranceDto>> GetAllInsurances()
        {
            var insurances = await _insuranceRepository.GetAllInsurances();
            if (insurances == null) return Enumerable.Empty<InsuranceDto>();

            return insurances.Select(element =>
            {
                var insuranceDto = new InsuranceDto();
                return _mapper.Map(element, insuranceDto);
            });
        }

        public async Task UpdateInsurance(EditInsurance edit, int id)
        {
            await _insuranceRepository.UpdateInsurance(edit, id);
        }

        public float CalculateInsuranceFee(Vehicle vehicle)
        {
            // Define the base fee for each vehicle category
            var categoryFees = new Dictionary<Category, float>
            {
                { Category.Motorcycle, 1000 },
                { Category.Hatchback, 2000 },
                { Category.Coupe, 2500 },
                { Category.Sedan, 3000 },
                { Category.Van, 4000 },
                { Category.SUV, 4500 },
                { Category.Truck, 6000 },
                { Category.Bus, 6500 }
            };

            var powerFees = new List<(int min, int max, float fee)>
            {
                (0, 100, 500),
                (101, 200, 700),
                (201, 300, 800),
                (301, 400, 900),
                (401, int.MaxValue, 1000)
            };

            categoryFees.TryGetValue(vehicle.Category, out var categoryFee);

            var powerFee = powerFees.First(pf => vehicle.Power >= pf.min && vehicle.Power <= pf.max).fee;

            return categoryFee + powerFee;
        }
    }
}
