using AutoMapper;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Repositories.Interfaces;
using Vehicle_Registration_System.Services.Interfaces;

namespace Vehicle_Registration_System.Services.Implementations
{
    public class OwnerService : IOwnerService
    {
        public readonly IMapper _mapper;
        public readonly IOwnerRepository _ownerRepository;

        public OwnerService (IMapper mapper, IOwnerRepository ownerRepository)
        {
            _mapper = mapper;
            _ownerRepository = ownerRepository;
        }

        public async Task AddOwner(InputOwner input)
        {
            await _ownerRepository.AddOwner(input);
        }

        public async Task DeleteOwner(int id)
        {
            await _ownerRepository.DeleteOwner(id);
        }

        public async Task<IEnumerable<OwnerDto>> FindByCity(string placeOfBirth)
        {
            var owners = await _ownerRepository.FindByCity(placeOfBirth);
            var response = owners?.Select(element =>
            {
                OwnerDto ownerDto = new OwnerDto();

                return _mapper.Map(element, ownerDto);
            });
            return response;
        }

        public async Task<IEnumerable<OwnerDto>> FindOwnerByVehicle(string manufacturer, string model)
        {
            var owners = await _ownerRepository.FindOwnerByVehicle(manufacturer, model);
            var response = owners?.Select(element =>
            {
                OwnerDto ownerDto = new OwnerDto();

                return _mapper.Map(element, ownerDto);
            });
            return response;
        }

        public async Task<IEnumerable<OwnerDto>> GetAllOwners()
        {
            var owners = await _ownerRepository.GetAllOwners();
            var response = owners?.Select(element =>
            {
                OwnerDto ownerDto = new OwnerDto();

                return _mapper.Map(element, ownerDto);
            });
            return response;
        }

        public async Task<long> GetLicensesByCity(string placeOfBirth)
        {
            return await _ownerRepository.GetLicensesByCity(placeOfBirth);
        }

        public async Task<OwnerDto> GetOwnerById(int id)
        {
            var owner = await _ownerRepository.GetOwnerById(id);

            OwnerDto ownerDto = new OwnerDto();

            var response = _mapper.Map(owner, ownerDto);
            return response;
        }

        public async Task UpdateOwner(EditOwner edit, int id)
        {
            await _ownerRepository.UpdateOwner(edit, id);
        }
    }
}
