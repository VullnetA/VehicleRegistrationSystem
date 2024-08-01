using AutoMapper;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Repositories.Interfaces;
using Vehicle_Registration_System.Services.Interfaces;

namespace Vehicle_Registration_System.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly IMapper _mapper;
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IMapper mapper, IVehicleRepository vehicleRepository)
        {
            _mapper = mapper;
            _vehicleRepository = vehicleRepository;
        }

        public async Task AddVehicle(RegisterVehicle register)
        {
            await _vehicleRepository.AddVehicle(register);
        }

        public async Task<bool> CheckRegistration(int id)
        {
            return await _vehicleRepository.CheckRegistration(id);
        }

        public async Task<long> CountByBrand(string manufacturer)
        {
            return await _vehicleRepository.CountByBrand(manufacturer);
        }

        public async Task<long> CountByCategory(string category)
        {
            return await _vehicleRepository.CountByCategory(category);
        }

        public async Task<long> CountByFuelType(string fuel)
        {
            return await _vehicleRepository.CountByFuelType(fuel);
        }

        public async Task<long> CountByYear(int year)
        {
            return await _vehicleRepository.CountByYear(year);
        }

        public async Task<long> CountRegistered()
        {
            return await _vehicleRepository.CountRegistered();
        }

        public async Task<long> CountTransmission(string transmission)
        {
            return await _vehicleRepository.CountTransmission(transmission);
        }

        public async Task<long> CountUnregistered()
        {
            return await _vehicleRepository.CountUnregistered();
        }

        public async Task DeleteVehicle(int id)
        {
            await _vehicleRepository.DeleteVehicle(id);
        }

        public async Task<IEnumerable<VehicleDto>> FindAllByOwner(int ownerId)
        {
            var vehicles = await _vehicleRepository.FindAllByOwner(ownerId);
            if (vehicles == null) return Enumerable.Empty<VehicleDto>();

            return vehicles.Select(element =>
            {
                var vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
        }

        public async Task<IEnumerable<VehicleDto>> FindByBrand(string manufacturer)
        {
            var vehicles = await _vehicleRepository.FindByBrand(manufacturer);
            if (vehicles == null) return Enumerable.Empty<VehicleDto>();

            return vehicles.Select(element =>
            {
                var vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
        }

        public async Task<IEnumerable<VehicleDto>> FindByFuelType(string fuel)
        {
            var vehicles = await _vehicleRepository.FindByFuelType(fuel);
            if (vehicles == null) return Enumerable.Empty<VehicleDto>();

            return vehicles.Select(element =>
            {
                var vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
        }

        public async Task<IEnumerable<VehicleDto>> FindByHorsepower(int power)
        {
            var vehicles = await _vehicleRepository.FindByHorsepower(power);
            if (vehicles == null) return Enumerable.Empty<VehicleDto>();

            return vehicles.Select(element =>
            {
                var vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
        }

        public async Task<VehicleDto> FindByLicensePlate(string licensePlate)
        {
            var vehicle = await _vehicleRepository.FindByLicensePlate(licensePlate);
            if (vehicle == null) return null;

            var vehicleDto = new VehicleDto();
            return _mapper.Map(vehicle, vehicleDto);
        }

        public async Task<IEnumerable<VehicleDto>> FindByYear(int year)
        {
            var vehicles = await _vehicleRepository.FindByYear(year);
            if (vehicles == null) return Enumerable.Empty<VehicleDto>();

            return vehicles.Select(element =>
            {
                var vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
        }

        public async Task<IEnumerable<VehicleDto>> GetAllVehicles()
        {
            var vehicles = await _vehicleRepository.GetAllVehicles();
            if (vehicles == null) return Enumerable.Empty<VehicleDto>();

            return vehicles.Select(element =>
            {
                var vehicleDto = new VehicleDto();
                return _mapper.Map<VehicleDto>(element);
            });
        }

        public async Task<VehicleDto> GetVehicleById(int id)
        {
            var vehicle = await _vehicleRepository.GetVehicleById(id);
            if (vehicle == null) return null;

            var vehicleDto = new VehicleDto();
            return _mapper.Map(vehicle, vehicleDto);
        }

        public async Task UpdateVehicle(EditVehicle edit, int id)
        {
            await _vehicleRepository.UpdateVehicle(edit, id);
        }
    }
}
