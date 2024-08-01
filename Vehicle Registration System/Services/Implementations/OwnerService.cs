using AutoMapper;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;
using Vehicle_Registration_System.Repositories.Interfaces;
using Vehicle_Registration_System.Services.Interfaces;

namespace Vehicle_Registration_System.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        public readonly IMapper _mapper;
        public readonly IOwnerRepository _ownerRepository;

        public OwnerService(IMapper mapper, IOwnerRepository ownerRepository)
        {
            _mapper = mapper;
            _ownerRepository = ownerRepository;
        }

        public async Task<Owner> AddOwner(InputOwner input)
        {
            return await _ownerRepository.AddOwner(input);
        }

        public async Task DeleteOwner(int id)
        {
            await _ownerRepository.DeleteOwner(id);
        }

        public async Task<IEnumerable<OwnerDto>> FindByCity(string placeOfBirth)
        {
            var owners = await _ownerRepository.FindByCity(placeOfBirth);
            if (owners == null) return Enumerable.Empty<OwnerDto>();

            return owners.Select(element =>
            {
                var ownerDto = new OwnerDto();
                return _mapper.Map(element, ownerDto);
            });
        }

        public async Task<IEnumerable<OwnerDto>> FindOwnerByVehicle(string manufacturer, string model)
        {
            var owners = await _ownerRepository.FindOwnerByVehicle(manufacturer, model);
            if (owners == null) return Enumerable.Empty<OwnerDto>();

            return owners.Select(element =>
            {
                var ownerDto = new OwnerDto();
                return _mapper.Map(element, ownerDto);
            });
        }

        public async Task<IEnumerable<OwnerDto>> GetAllOwners()
        {
            var owners = await _ownerRepository.GetAllOwners();
            if (owners == null) return Enumerable.Empty<OwnerDto>();

            return owners.Select(element =>
            {
                var ownerDto = new OwnerDto();
                return _mapper.Map(element, ownerDto);
            });
        }

        public async Task<long> GetLicensesByCity(string placeOfBirth)
        {
            return await _ownerRepository.GetLicensesByCity(placeOfBirth);
        }

        public async Task<OwnerDto> GetOwnerById(int id)
        {
            var owner = await _ownerRepository.GetOwnerById(id);
            if (owner == null) return null;

            var ownerDto = new OwnerDto();
            return _mapper.Map(owner, ownerDto);
        }

        public async Task<IEnumerable<OwnerDto>> GetOwnersByName(string name)
        {
            var owners = await _ownerRepository.GetOwnersByName(name);
            if (owners == null) return Enumerable.Empty<OwnerDto>();

            return owners.Select(element =>
            {
                var ownerDto = new OwnerDto();
                return _mapper.Map(element, ownerDto);
            });
        }

        public async Task UpdateOwner(EditOwner edit, int id)
        {
            await _ownerRepository.UpdateOwner(edit, id);
        }
    }
}
