using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VillaRent.Domain.IRepositories;
using VillaRent.Persistence.Data;

namespace VillaRent.Persistence.Repositories;

public class RepositoryBase<T>(ApplicationDbContext dbContext) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();
    private const char Separator = ',';

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
        int pageSize = 0, int pageNumber = 1)
    {
        IQueryable<T> query = _dbSet;

        if (filter is not null) query = query.Where(filter);

        if (pageSize > 0)
        {
            if (pageSize > 100) pageSize = 100;

            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
        
        if (includeProperties is not null)
        {
            foreach (var includeProp in includeProperties.Split(Separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        
        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        if (!tracked) query = query.AsNoTracking();
        
        if (filter is not null) query = query.Where(filter);
        if (includeProperties is not null)
        {
            foreach (var includeProp in includeProperties.Split(Separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}