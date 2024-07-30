using AutoMapper;
using VillaRent.API.Contracts.DTO;
using VillaRent.Application.ServiceModels;
using VillaRent.Domain.Models;

namespace VillaRent.API.Configurations;

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
        
        CreateMap<ResponseUser, ApplicationUser>().ReverseMap();
    }
}