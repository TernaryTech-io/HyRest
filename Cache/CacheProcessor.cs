using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyRest.Cache;

public class CacheProcessor 
{
    private readonly ILogger _logger;
    public CacheProcessor(ILogger logger, CancellationTokenSource? source = null)
    {
        _logger = logger;
        if (source != null)
            _source = source;
        ClearExpired();
    }
    private CancellationTokenSource _source { get; set; } = new();
    public ConcurrentDictionary<string, CacheItem> Cache { get; set; } = [];
    public CacheProcessor WithCancellationTokenSource(CancellationTokenSource tokenSource)
    {
        _source = tokenSource;
        return this;
    }
    public void Add(CacheItem item)
    {
        Cache.AddOrUpdate(item.Key, item, (key,ci) =>
        {
            ci.Expiration = DateTime.UtcNow.AddMinutes(item.LifeTime.Minutes);
            Cache[key] = ci;
            return ci;
        });        
    }
    public void Cancel()
    {
        _source.Cancel();
    }
    public async Task Remove(string id)
    {
        Cache.TryRemove(id, out CacheItem? item);
    }
    public async Task<CacheItem?> TryGetResult(string id)
    {
        try
        {
            if (Cache.ContainsKey(id))
            {
                CacheItem? item = Cache[id];
                if (item != null)
                {
                    item.Expiration = DateTime.UtcNow.AddMinutes(item.LifeTime.Minutes);
                    Cache[id] = item;
                    return item;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogTrace($"<ConcurrentProcessor> Failed to process item ({id}) with error: {ex.Message}. {ex.StackTrace}");
        }
        return null;
    }
    public async Task ClearExpired()
    {
        while (true)
        {
            if (_source.Token.IsCancellationRequested)
                break;
            if (!Cache.IsEmpty)
            {
                try
                {
                    var items = Cache.Values
                        .Where(c => c.Expiration > DateTime.UtcNow)
                        .ToList();
                    items.ForEach(i =>
                    {
                        Cache.TryRemove(i.Key, out var item);
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogTrace($"CacheProcessor Running Queue failed with error: {ex.Message}. {ex.StackTrace}");
                }
            }
            else
            {
                await Task.Delay(30000);
            }
        }
        Cache.Clear();
    }
}

