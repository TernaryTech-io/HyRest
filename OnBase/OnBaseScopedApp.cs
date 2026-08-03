using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyRest;

public class OnBaseScopedApp : OnBaseApp, IDisposable
{
    public OnBaseScopedApp(ILogger<OnBaseScopedApp> logger, IAuthenticationCredentials credentials, HylandClientOptions options)
        :base(logger,credentials,options)
    {        
        Init();
    }
    public OnBaseScopedApp(ILogger<OnBaseScopedApp> logger, IHylandClientFactory clientFactory, IOptions<HylandOpenIdOptionsBuilder> options)
        :base(logger,clientFactory,options)
    {        
        Init();
    }
    public void Dispose()
    {
        if (IsConnected)
            Session.DisconnectAsync().Wait();
    }
}