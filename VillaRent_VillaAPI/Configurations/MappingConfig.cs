using AutoMapper;
using VillaRent_VillaAPI.Models;
using VillaRent_VillaAPI.Models.DTO;

namespace VillaRent_VillaAPI.Configurations;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDto>().ReverseMap();

        CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        CreateMap<Villa, VillaCreateDto>().ReverseMap();

        CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberCreateDto>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDto>().ReverseMap();

        CreateMap<ApplicationUser, UserDto>().ReverseMap();
    }
}