using VillaRent_VillaAPI.Data;
using VillaRent_VillaAPI.Models;
using VillaRent_VillaAPI.Repository.IRepository;

namespace VillaRent_VillaAPI.Repository;

public class VillaNumberRepository(ApplicationDbContext dbContext) : Repository<VillaNumber>(dbContext), IVillaNumberRepository
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