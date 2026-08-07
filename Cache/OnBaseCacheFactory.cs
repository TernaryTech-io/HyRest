using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace HyRest.Cache;

public class OnBaseCacheFactory
{
    private readonly HybridCache _cache;
    public OnBaseCacheFactory(HybridCache cache)
    {
        _cache = cache;
    }
    public OnBaseAppCache<T> CreateCache<T>(string? prefix = null) where T : class, IOnBaseIdentifiable
    {         
        return new OnBaseAppCache<T>(_cache, prefix);
    }
    public static void RegisterServices(IServiceCollection services, HybridCacheEntryOptions? defaultOptions = null)
    {
        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = defaultOptions ?? new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(12),
                LocalCacheExpiration = TimeSpan.FromMinutes(60),
            };
        });
        services.AddSingleton<OnBaseCacheFactory>();
    }
}
