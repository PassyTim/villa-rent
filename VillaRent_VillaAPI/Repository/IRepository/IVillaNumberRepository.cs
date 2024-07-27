using VillaRent_VillaAPI.Models;

namespace VillaRent_VillaAPI.Repository.IRepository;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateAsync(VillaNumber entity);
}