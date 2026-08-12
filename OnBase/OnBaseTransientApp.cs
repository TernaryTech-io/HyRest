using HyRest.Cache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyRest;

public class OnBaseScopedApp : OnBaseApp, IDisposable, IAsyncDisposable
{
    public OnBaseScopedApp(ILogger<OnBaseApp> logger, IHylandClientFactory clientFactory, HylandClientOptions options, OnBaseAppCache cache)
        : base(logger,clientFactory,options,cache)
    {        
        
    }
    public OnBaseScopedApp(ILogger<OnBaseScopedApp> logger, IHylandClientFactory clientFactory, IOptions<HylandOpenIdClientOptionsBuilder> options, OnBaseAppCache cache)
        :base(logger,clientFactory,options, cache)
    {        
        
    }
    public void Dispose()
    {
        if (IsConnected)
            Session.Disconnect();
    }
    public async ValueTask DisposeAsync()
    {
        if (IsConnected)
            await Session.DisconnectAsync();
        return;
    }

       
}