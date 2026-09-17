using Microsoft.Extensions.Logging;

namespace HyRest.Cache;

public class OnBaseAppCache : IOnBaseAppCache    
{
    private readonly ILogger _logger;
    private readonly CacheProcessor _cache;
    public OnBaseAppCache(ILogger<OnBaseAppCache> logger, string? prefix = null)
    {
        _cache = new CacheProcessor(logger, new());
        _logger = logger;
    }
    public async Task<T?> GetOrCreateAsync<T>(string id, Func<CancellationToken, ValueTask<T>> factory, CancellationToken ct = default, string? prefix = null) 
        where T : class, IOnBaseCacheable
    {
        var key = CreateKey<T>(id, prefix).ToString();
        var item = await _cache.TryGetResult(id);
        if (item != null)
            return item.Deserialize<T>();
        return null;
    }
    public async Task RemoveAsync<T>(T item, CancellationToken ct = default, string? prefix = null) where T : class, IOnBaseCacheable
     => await _cache.Remove(CreateKey(item, prefix));

    public async Task SetAsync<T>(T item, CancellationToken ct = default, string? prefix = null) 
        where T : class, IOnBaseCacheable
    {
        var idKey = CreateCacheKey(item, prefix);
        var cacheItem = CacheItem.Create(idKey, item);
        _logger.LogTrace($"Setting cache with key {idKey}");
        _cache.Add(cacheItem);
        if (item.Name != null)
        {
            var nameKey = CreateKey<T>(item.Name, prefix);
            var cacheKey = CacheKey.Create(nameKey, typeof(T), prefix);
            var ciName = CacheItem.Create(cacheKey, item);
            _cache.Add(ciName);
        }       
    }
    public async Task<(bool,T?)> TryGetValueAsync<T>(string key, CancellationToken ct = default, string? prefix = null)
        where T : class, IOnBaseCacheable
    {        
        var idkey = CreateKey<T>(key, prefix);
        var result = await _cache.TryGetResult(idkey);            

        return (result is not null, result?.Deserialize<T>());
    }
    public async Task<bool> ExistsAsync<T>(string key, CancellationToken ct = default, string? prefix = null)
        where T : class, IOnBaseCacheable
    {
        var (exists,_) = await TryGetValueAsync<T>(key, ct, prefix);
        return exists;
    }
    private string CreateKey<T>(string id, string? prefix = null) where T : class, IOnBaseCacheable
        => CacheKey.Create(id, typeof(T), prefix).ToString();
    private string CreateKey<T>(T item, string? prefix = null) where T : class, IOnBaseCacheable
        => CacheKey.Create(item, prefix).ToString();
    private CacheKey CreateCacheKey<T>(T item, string? prefix = null) where T : class, IOnBaseCacheable
        => CacheKey.Create(item, prefix);
    ///// <summary>
    ///// Provides override options so that null or default values aren't written to the cache for ExistsAsync & TryGetValueAsync
    ///// </summary>
    //private readonly HybridCacheEntryOptions ReadOnlyOptions = new()
    //{
    //    Flags = HybridCacheEntryFlags.DisableUnderlyingData | HybridCacheEntryFlags.DisableLocalCacheWrite | HybridCacheEntryFlags.DisableDistributedCacheWrite
    //};
    //private async ValueTask<T> DoNothing<T>(T _, CancellationToken __)
    //    where T : class, IOnBaseCacheable
    //{
    //    return await ValueTask.FromResult<T>(null!);
    //}
}
