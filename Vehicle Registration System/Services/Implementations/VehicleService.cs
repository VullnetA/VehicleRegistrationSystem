using AutoMapper;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Enums;
using Vehicle_Registration_System.Models;
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
            var response = vehicles?.Select(element =>
            {
                VehicleDto vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
            return response;
        }

        public async Task<IEnumerable<VehicleDto>> FindByBrand(string manufacturer)
        {
            var vehicles = await _vehicleRepository.FindByBrand(manufacturer);
            var response = vehicles?.Select(element =>
            {
                VehicleDto vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
            return response;
        }

        public async Task<IEnumerable<VehicleDto>> FindByFuelType(string fuel)
        {
            var vehicles = await _vehicleRepository.FindByFuelType(fuel);
            var response = vehicles?.Select(element =>
            {
                VehicleDto vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
            return response;
        }

        public async Task<IEnumerable<VehicleDto>> FindByHorsepower(int power)
        {
            var vehicles = await _vehicleRepository.FindByHorsepower(power);
            var response = vehicles?.Select(element =>
            {
                VehicleDto vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
            return response;
        }

        public async Task<VehicleDto> FindByLicensePlate(string licensePlate)
        {
            var vehicle = await _vehicleRepository.FindByLicensePlate(licensePlate);

            VehicleDto vehicleDto = new VehicleDto();

            var response = _mapper.Map(vehicle, vehicleDto);
            return response;
        }

        public async Task<IEnumerable<VehicleDto>> FindByYear(int year)
        {
            var vehicles = await _vehicleRepository.FindByYear(year);
            var response = vehicles?.Select(element =>
            {
                VehicleDto vehicleDto = new VehicleDto();
                return _mapper.Map(element, vehicleDto);
            });
            return response;
        }

        public async Task<IEnumerable<VehicleDto>> GetAllVehicles()
        {
            var vehicles = await _vehicleRepository.GetAllVehicles();
            var response = vehicles?.Select(element =>
            {
                VehicleDto vehicleDto = _mapper.Map<VehicleDto>(element);
                return vehicleDto;
            });
            return response;
        }


        public async Task<VehicleDto> GetVehicleById(int id)
        {
            var vehicle = await _vehicleRepository.GetVehicleById(id);

            VehicleDto vehicleDto = new VehicleDto();

            var response = _mapper.Map(vehicle, vehicleDto);
            return response;
        }

        public async Task UpdateVehicle(EditVehicle edit, int id)
        {
            await _vehicleRepository.UpdateVehicle(edit, id);
        }
    }
}
