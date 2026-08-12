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
    public async Task<T?> GetOrCreateAsync<T>(string id, Func<CancellationToken, ValueTask<T>>? factory = null, CancellationToken ct = default) where T : class, IOnBaseIdentifiable

        => await _cache.GetOrCreateAsync(
            key: CreateKey<T>(id), 
            factory: factory, 
            tags: [],
            cancellationToken: ct);
    public async Task RemoveAsync<T>(T item, CancellationToken ct = default) where T : class, IOnBaseIdentifiable
     => await _cache.RemoveAsync(CreateKey(item), ct);

    public async Task SetAsync<T>(T item, CancellationToken ct = default) where T : class, IOnBaseIdentifiable
     => await _cache.SetAsync(CreateKey(item), item, null, null, ct);

    private string CreateKey<T>(string id) where T : class, IOnBaseIdentifiable
        => CacheKey.Create(id, typeof(T), _prefex).ToString();
    private string CreateKey<T>(T item) where T : class, IOnBaseIdentifiable
        => CacheKey.Create(item, _prefex).ToString();    
}
