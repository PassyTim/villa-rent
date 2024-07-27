using VillaRent_VillaAPI.Models;

namespace VillaRent_VillaAPI.Repository.IRepository;

public interface IVillaRepository : IRepository<Villa>
{
    Task<Villa> UpdateAsync(Villa entity);
}