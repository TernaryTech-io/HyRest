using Microsoft.Extensions.Caching.Hybrid;

namespace HyRest.Cache;

public class OnBaseAppCache : IOnBaseAppCache
    
{
    private HybridCache _cache;
    private string? _prefex;
    public OnBaseAppCache(HybridCache cache, string? prefix = null)
    {
        _cache = cache;
    }
    public async Task<T?> GetOrCreateAsync<T>(string id, Func<CancellationToken, ValueTask<T>> factory, CancellationToken ct = default) 
        where T : class, IOnBaseCacheable
        => await _cache.GetOrCreateAsync(
            key: CreateKey<T>(id), 
            factory: factory, 
            tags: [],
            cancellationToken: ct);
    public async Task RemoveAsync<T>(T item, CancellationToken ct = default) where T : class, IOnBaseCacheable
     => await _cache.RemoveAsync(CreateKey(item), ct);

    public async Task SetAsync<T>(T item, CancellationToken ct = default) 
        where T : class, IOnBaseCacheable
    {
        var idKey = CreateKey(item);
        await _cache.SetAsync(idKey, item, null, null, ct);
        if (item.Name != null)
        {
            var nameKey = CreateKey<T>(item.Name);
            await _cache.SetAsync(nameKey, item, null, null, ct);
        }
        if(item.SystemName != null)
        {
            var sysKey = CreateKey<T>(item.SystemName);
            await _cache.SetAsync(sysKey, item, null, null, ct);
        }        
    }
    public async Task<(bool,T?)> TryGetValueAsync<T>(string key, CancellationToken ct = default)
        where T : class, IOnBaseCacheable
    {
        var result = await _cache.GetOrCreateAsync<object,object>(
            key,
            null!,
            DoNothing,
            ReadOnlyOptions,
            [],
            ct) as T;

        return (result is not null, (T)result!);
    }
    public async Task<bool> ExistsAsync<T>(string key, CancellationToken ct = default)
        where T : class, IOnBaseCacheable
    {
        var (exists,_) = await TryGetValueAsync<T>(key, ct);
        return exists;
    }
    private string CreateKey<T>(string id) where T : class, IOnBaseCacheable
        => CacheKey.Create(id, typeof(T), _prefex).ToString();
    private string CreateKey<T>(T item) where T : class, IOnBaseCacheable
        => CacheKey.Create(item, _prefex).ToString();
    /// <summary>
    /// Provides override options so that null or default values aren't written to the cache for ExistsAsync & TryGetValueAsync
    /// </summary>
    private readonly HybridCacheEntryOptions ReadOnlyOptions = new()
    {
        Flags = HybridCacheEntryFlags.DisableUnderlyingData | HybridCacheEntryFlags.DisableLocalCacheWrite | HybridCacheEntryFlags.DisableDistributedCacheWrite
    };
    private async ValueTask<object> DoNothing(object _, CancellationToken __)
    {
        return await ValueTask.FromResult<object>(null!);
    }
}
