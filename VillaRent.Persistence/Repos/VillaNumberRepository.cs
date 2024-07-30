using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;
using VillaRent.Persistence.Data;

namespace VillaRent.Persistence.Repos;

public class VillaNumberRepository(ApplicationDbContext dbContext)
    : RepositoryBase<VillaNumber>(dbContext), IVillaNumberRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _dbContext.VillaNumbers.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
}