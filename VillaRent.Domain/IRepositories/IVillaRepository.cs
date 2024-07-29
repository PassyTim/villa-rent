using VillaRent.Domain.Models;

namespace VillaRent.Domain.IRepositories;

public interface IVillaRepository : IRepository<Villa>
{
    Task<Villa> UpdateAsync(Villa entity);
}