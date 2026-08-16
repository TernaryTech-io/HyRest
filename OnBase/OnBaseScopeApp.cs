using HyRest.Cache;
using HyRest.OnBase.ApiServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyRest;

public class OnBaseScopedApp : OnBaseApp, IDisposable, IAsyncDisposable
{
    public OnBaseScopedApp(ILogger<OnBaseScopedApp> logger, HylandClientFactory clientFactory, OnBaseSessionService sessionService,
        OnBaseAdministrationService administrationService, OnBaseCoreService coreService, OnBaseWorkViewService workViewService)
    : base(logger,clientFactory,sessionService,administrationService,coreService,workViewService)
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