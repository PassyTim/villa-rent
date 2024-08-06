using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using VillaRent.Domain.IRepositories;
using VillaRent.Domain.Models;
using VillaRent.Persistence.Data;

namespace VillaRent.Persistence.Repositories;

public class CachedVillaNumberRepository : IVillaNumberRepository
{
    private readonly VillaNumberRepository _decorated;
    private readonly IDistributedCache _distributedCache;
    private readonly ApplicationDbContext _dbContext;
    
    public CachedVillaNumberRepository(VillaNumberRepository decorated, IDistributedCache distributedCache, ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _decorated = decorated;
        _distributedCache = distributedCache;
    }
    public async Task<List<VillaNumber>> GetAllAsync(Expression<Func<VillaNumber, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
    {
        string key = "villaNumber-all";

        string? cachedVillaNumbers = await _distributedCache.GetStringAsync(key);
        if (string.IsNullOrEmpty(cachedVillaNumbers))
        {
            List<VillaNumber> villaNumbers = await _decorated.GetAllAsync(filter, includeProperties, pageSize, pageNumber);

            await _distributedCache.SetStringAsync(key,
                JsonConvert.SerializeObject(villaNumbers),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            return villaNumbers;
        }

        return JsonConvert.DeserializeObject<List<VillaNumber>>(cachedVillaNumbers)!;
    }

    public async Task<VillaNumber?> GetAsync(
        Expression<Func<VillaNumber, bool>> filter = null, bool tracked = true, string? includeProperties = null)
    {
        int number = IdExtractor.Extract(filter);
        string key = $"villaNumber-{number}";

        string? cachedVillaNumber = await _distributedCache.GetStringAsync(key);
        
        VillaNumber? villaNumber;
        if (string.IsNullOrEmpty(cachedVillaNumber))
        {
            villaNumber = await _decorated.GetAsync(filter, tracked, includeProperties);
            if (villaNumber is null) return villaNumber;
            
            await _distributedCache.SetStringAsync(key,
                JsonConvert.SerializeObject(villaNumber),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            
            return villaNumber;
        }

        villaNumber = JsonConvert.DeserializeObject<VillaNumber>(cachedVillaNumber)!;
        _dbContext.Set<VillaNumber>().Attach(villaNumber);
        return villaNumber;
    }

    public Task CreateAsync(VillaNumber entity)
    {
        return _decorated.CreateAsync(entity);
    }

    public Task RemoveAsync(VillaNumber entity)
    {
        return _decorated.RemoveAsync(entity);
    }

    public Task SaveAsync()
    {
        return _decorated.SaveAsync();
    }

    public Task<VillaNumber> UpdateAsync(VillaNumber entity)
    {
        return _decorated.UpdateAsync(entity);
    }
}