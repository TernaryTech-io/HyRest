using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyRest;

public class OnBaseScopedApp : OnBaseApp, IDisposable, IAsyncDisposable
{
    public OnBaseScopedApp(ILogger<OnBaseScopedApp> logger, IAuthenticationCredentials credentials, HylandClientOptions options)
        :base(logger,credentials,options)
    {        
        
    }
    public OnBaseScopedApp(ILogger<OnBaseScopedApp> logger, IHylandClientFactory clientFactory, IOptions<HylandOpenIdClientOptionsBuilder> options)
        :base(logger,clientFactory,options)
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
    public static OnBaseScopedApp CreateScopedApp(ILogger<OnBaseScopedApp> logger, IAuthenticationCredentials credentials, HylandClientOptions options)
        => new OnBaseScopedApp(logger, credentials, options);
       
}