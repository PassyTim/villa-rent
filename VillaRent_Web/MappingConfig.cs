using AutoMapper;
using VillaRent_Web.Models.DTO;

namespace VillaRent_Web;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
        CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();

        CreateMap<VillaNumberDto, VillaNumberCreateDto>().ReverseMap();
        CreateMap<VillaNumberDto, VillaNumberUpdateDto>().ReverseMap();
    }
}