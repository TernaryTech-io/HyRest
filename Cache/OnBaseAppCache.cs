//using Microsoft.Extensions.Caching.Distributed;
//using Microsoft.Extensions.Caching.Memory;
//using System.Text;
//using System.Text.Json;

//namespace HyRest.Cache;

///// <summary>
///// Hybrid & Configurable Cache for .NET 8
///// </summary>
///// <typeparam name="T"></typeparam>
//public class OnBaseAppCache<T> : IOnBaseAppCache<T>
//    where T : class, IOnBaseIdentifiable
//{
//    public OnBaseAppCache(IMemoryCache memoryCache, OnBaseCacheOptions options, string? prefix = null, IDistributedCache ? distributedCache = null)
//    {
//        _memoryCache = memoryCache;
//        _distributedCache = distributedCache;
//        _options = options;
//        _prefix = prefix;
//    }
//    #region private
//    private readonly IMemoryCache _memoryCache;
//    private readonly IDistributedCache? _distributedCache;
//    private readonly OnBaseCacheOptions _options;
//    private readonly string? _prefix;
//    #endregion
//    public async Task<T?> GetOrCreateAsync(string id, CancellationToken ct = default)
//    {
//        T? result = null;
//        if (_memoryCache.TryGetValue(CreateKey(id), out result)
//            && result != null)
//            return result;
//        if (result == null)
//            result = await TryGetFromDistributedAsync(id);
//        else if(result != null && _distributedCache != null)
//            await _distributedCache.RefreshAsync(CreateKey(id), ct);
//        return result;
//    }
//    private async Task<T?> TryGetFromDistributedAsync(string id, CancellationToken ct = default)
//    {
//        if(_distributedCache == null)
//            return default;
//        var bytes = await _distributedCache.GetAsync(CreateKey(id), ct);
//        if (bytes == null) return default;
//        var json = Encoding.UTF8.GetString(bytes);
//        return JsonSerializer.Deserialize<T>(json, _options.GetJsonSerializerOptions());
//    }
//    public async Task SetAsync(T item, CancellationToken ct = default)
//    {
//        await TrySetToDistributedAsync(item, ct);
//        _memoryCache.Set(CreateKey(item), item, _options.GetMemoryCacheOptions());
//    }
//    private async Task TrySetToDistributedAsync(T item, CancellationToken ct = default)
//    {
//        if (_distributedCache == null)
//            return;
//        if (item == null) return;
//        var json = JsonSerializer.Serialize(item, _options.GetJsonSerializerOptions());
//        var bytes = Encoding.UTF8.GetBytes(json);      
//        await _distributedCache.SetAsync(CreateKey(item), bytes, _options.GetDistributedOptions(), ct);
//    }
//    public async Task RemoveAsync(T item, CancellationToken ct = default)
//    {
//        _memoryCache.Remove(CreateKey(item));
//        await TryRemoveFromDistributedAsync(item,ct);
//    }
//    private async Task TryRemoveFromDistributedAsync(T item, CancellationToken ct = default)
//    {
//        if (_distributedCache == null)
//            return;
//        await _distributedCache.RemoveAsync(CreateKey(item),ct);
//    }
//    public async Task<bool> ContainsAsync(string id, CancellationToken ct = default)
//    {        
//        if (_distributedCache != null && await TryGetFromDistributedAsync(CreateKey(id), ct) != null)
//            return true;
//        return _memoryCache.TryGetValue(CreateKey(id), out T? output);            
//    }
//    private string CreateKey(string id)
//        => CacheKey.Create(id,typeof(T), _prefix).ToString();
//    private string CreateKey(T item)
//        => CacheKey.Create(item, _prefix).ToString();
//}