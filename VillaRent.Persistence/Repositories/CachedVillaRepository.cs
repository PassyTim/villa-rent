using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;
using VillaRent.Persistence.Data;

namespace VillaRent.Persistence.Repositories;

public class CachedVillaRepository : IVillaRepository
{
    private readonly IVillaRepository _decorated;
    private readonly IDistributedCache _distributedCache;
    private readonly ApplicationDbContext _dbContext;

    public CachedVillaRepository(IVillaRepository decorated, IDistributedCache distributedCache, ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _distributedCache = distributedCache;
        _decorated = decorated;
    }
    public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
    {
        string key = "villa-all";
        
        string? cachedVillas = await _distributedCache.GetStringAsync(key);

        List<Villa> villas;
        if (string.IsNullOrEmpty(cachedVillas))
        {
            villas = await _decorated.GetAllAsync(filter, includeProperties, pageSize, pageNumber);
            if (villas is null)
            {
                return villas;
            }

            await _distributedCache.SetStringAsync(key, 
                JsonConvert.SerializeObject(villas), 
                new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return villas;
        }

        villas = JsonConvert.DeserializeObject<List<Villa>>(cachedVillas)!;
        return villas;
    }

    public async Task<Villa?> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true, string? includeProperties = null)
    {
        int id = IdExtractor.Extract(filter);
        var key = $"villa-{id}";
        
        string? cachedVilla = await _distributedCache.GetStringAsync(key);

        Villa? villa; 
        if (string.IsNullOrEmpty(cachedVilla))
        {
            villa = await _decorated.GetAsync(filter, tracked, includeProperties);
            if (villa is null)
            {
                return villa;
            }

            await _distributedCache.SetStringAsync(key, 
                JsonConvert.SerializeObject(villa), 
                new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            });
            return villa;
        }
        
        villa = JsonConvert.DeserializeObject<Villa>(cachedVilla)!;
        _dbContext.Set<Villa>().Attach(villa);
        return villa;
    }

    public Task CreateAsync(Villa entity)
    {
        return _decorated.CreateAsync(entity);
    }

    public Task RemoveAsync(Villa entity)
    {
        return _decorated.RemoveAsync(entity);
    }

    public Task SaveAsync()
    {
        return _decorated.SaveAsync();
    }

    public Task<Villa> UpdateAsync(Villa entity)
    {
        return _decorated.UpdateAsync(entity);
    }
}