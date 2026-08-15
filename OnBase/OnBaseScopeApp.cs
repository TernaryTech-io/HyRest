using HyRest.Cache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyRest;

public class OnBaseScopedApp : OnBaseApp, IDisposable, IAsyncDisposable
{
    public OnBaseScopedApp(ILogger<OnBaseScopedApp> logger, IHylandClientFactory clientFactory,
    OnBaseSession session, OnBaseAdministration administration, OnBaseCore core, OnBaseWorkView workView)
    : base(logger,clientFactory,session,administration,core,workView)
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