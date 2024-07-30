using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;
using VillaRent.Persistence.Data;

namespace VillaRent.Persistence.Repositories;

public class VillaRepository(ApplicationDbContext dbContext) : RepositoryBase<Villa>(dbContext), IVillaRepository
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