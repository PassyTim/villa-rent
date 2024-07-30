using VillaRent.Domain.Models;

namespace VillaRent.Domain.IRepositories;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    Task<VillaNumber> UpdateAsync(VillaNumber entity);
}