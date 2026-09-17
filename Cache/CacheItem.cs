using HyRest.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyRest.Cache;

public class CacheItem
{
    public string Key { get; set; }
    public string?[]? Tags { get; set; }
    public DateTime Created { get; set; }
    public TimeSpan LifeTime { get; set; }
    public DateTime Expiration { get; set; }
    public Type ObjectType { get; set; }
    public string Json { get; set; }
    public static CacheItem Create(CacheKey key, object item, TimeSpan? lifetime = null, params string?[]? tags)
    {
        TimeSpan timeSpan = lifetime.HasValue ? lifetime.Value : TimeSpan.FromMinutes(15);
        return new CacheItem
        {
            Key = key.ToString(),
            Created = DateTime.UtcNow,
            LifeTime = timeSpan,
            Expiration = DateTime.UtcNow.AddMinutes(timeSpan.Minutes),
            ObjectType = item.GetType(),
            Tags = tags,
            Json = JsonUtility.Serialize(item, item.GetType())
        };
    }
    public T? Deserialize<T>()
    {
        if (ObjectType != typeof(T))
            throw new Exception($"Cannot deserialize cache item to the type of {typeof(T).Name}");
        return JsonUtility.Deserialize<T>(Json);
    }
}