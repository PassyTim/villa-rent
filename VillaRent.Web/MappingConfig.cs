using AutoMapper;
using VillaRent.Web.Models.DTO;

namespace VillaRent.Web;

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