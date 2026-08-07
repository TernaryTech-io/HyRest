using Microsoft.Extensions.Caching.Hybrid;

namespace HyRest.Cache;

public class OnBaseAppCache<T> : IOnBaseAppCache<T>
    where T : class, IOnBaseIdentifiable
{
    private HybridCache _cache;
    private string? _prefex;
    public OnBaseAppCache(HybridCache cache, string? prefix = null)
    {
        _cache = cache;
    }
    public async Task<T?> GetOrCreateAsync(string id, Func<CancellationToken, ValueTask<T>> factory, CancellationToken ct = default)
        => await _cache.GetOrCreateAsync(
            key: CreateKey(id), 
            factory: factory, 
            tags: [],
            cancellationToken: ct);
    public async Task RemoveAsync(T item, CancellationToken ct = default)
     => await _cache.RemoveAsync(CreateKey(item), ct);

    public async Task SetAsync(T item, CancellationToken ct = default)
     => await _cache.SetAsync(CreateKey(item), item, null, null, ct);

    private string CreateKey(string id)
        => CacheKey.Create(id, typeof(T), _prefex).ToString();
    private string CreateKey(T item)
        => CacheKey.Create(item, _prefex).ToString();    
}
