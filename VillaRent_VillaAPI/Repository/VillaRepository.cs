using VillaRent_VillaAPI.Data;
using VillaRent_VillaAPI.Models;
using VillaRent_VillaAPI.Repository.IRepository;

namespace VillaRent_VillaAPI.Repository;

public class VillaRepository(ApplicationDbContext dbContext) : Repository<Villa>(dbContext), IVillaRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Villa> UpdateAsync(Villa entity)
    {
        entity.UpdatedDate =DateTime.Now;
        _dbContext.Villas.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

}