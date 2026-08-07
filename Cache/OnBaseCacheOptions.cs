using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace HyRest.Cache;

public sealed class OnBaseCacheOptions
{    
    /// <summary>
    /// Optional, Construct the MemoryCacheEntryOptions or use the properties to have one constructed
    /// </summary>
    public MemoryCacheEntryOptions? MemoryCacheOptions { get; set; }
    /// <summary>
    /// Defaults to 30 minutes
    /// </summary>
    public TimeSpan? DefaultMemorySlidingExpiration { get; set; }
    /// <summary>
    /// Defaults to 2 Hours
    /// </summary>
    public TimeSpan? DefaultMemoryAbsoluteExpirationRelativeToNow { get; set; }
    /// <summary>
    /// Set an absolute expiration, default 12 hours.
    /// </summary>
    public DateTimeOffset? DefaultMemoryAbsoluteExpiration { get; set; }
    /// <summary>
    /// Returns the configured fields or defaults.
    /// </summary>
    /// <returns></returns>
    public MemoryCacheEntryOptions GetMemoryCacheOptions()
    {
        return MemoryCacheOptions ?? new MemoryCacheEntryOptions
        {
            SlidingExpiration = DefaultMemorySlidingExpiration ?? TimeSpan.FromMinutes(30),
            AbsoluteExpiration = DefaultMemoryAbsoluteExpiration ?? DateTimeOffset.UtcNow.AddHours(12),
            AbsoluteExpirationRelativeToNow = DefaultMemoryAbsoluteExpirationRelativeToNow ?? TimeSpan.FromHours(2)
        };
    }
    /// <summary>
    /// Optional, Construct the DistributedCacheEntryOptions or use the properties to have one constructed
    /// </summary>
    public DistributedCacheEntryOptions? DistributedCacheOptions { get; set; }
    /// <summary>
    /// Defaults to 2 hours
    /// </summary>
    public TimeSpan? DefaultDistributedSlidingExpiration { get; set; }
    /// <summary>
    /// Defaults to 8 Hours
    /// </summary>
    public TimeSpan? DefaultDistributedAbsoluteExpirationRelativeToNow { get; set; }
    /// <summary>
    /// Set an absolute expiration, default 24 hours.
    /// </summary>
    public DateTimeOffset? DefaultDistributedAbsoluteExpiration { get; set; }
    /// <summary>
    /// Returns the configured fields or defaults
    /// </summary>
    /// <returns>DistributedCacheEntryOptions</returns>
    public DistributedCacheEntryOptions GetDistributedOptions()
    {
        return DistributedCacheOptions ?? new DistributedCacheEntryOptions
        {
            SlidingExpiration = DefaultDistributedSlidingExpiration ?? TimeSpan.FromHours(2),
            AbsoluteExpirationRelativeToNow = DefaultDistributedAbsoluteExpirationRelativeToNow ?? TimeSpan.FromHours(8),
            AbsoluteExpiration = DefaultDistributedAbsoluteExpiration ?? DateTimeOffset.UtcNow.AddHours(24)
        };
    }
    public JsonSerializerOptions? DefaultDistributedJsonSerializerOptions { get; set; }
    public JsonSerializerOptions GetJsonSerializerOptions()
        => DefaultDistributedJsonSerializerOptions ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
}

public enum OnBaseCachePrefixOption
{
    None,
    UserId,
    Tenant,
    Specified
}