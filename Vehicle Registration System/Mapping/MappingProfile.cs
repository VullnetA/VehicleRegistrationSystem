using AutoMapper;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner));
            CreateMap<VehicleDto, Vehicle>();

            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();

            CreateMap<Insurance, InsuranceDto>()
                .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle));

            CreateMap<InsuranceDto, Insurance>();
        }
    }
}