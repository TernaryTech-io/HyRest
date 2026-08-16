using HyRest.Cache;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.ApiServices;

public class OnBaseSessionService : OnBaseService<IOnBaseSessionAPI>, IOnBaseSessionService
{
    public OnBaseSessionService(OnBaseAppCache cache, HylandClientFactory hylandClientFactory, ILogger<OnBaseSessionService> logger)
        : base(cache, hylandClientFactory, logger)
    {
        
    }

    public Task InitiateSession(CancellationToken token)
        => Run(Api.InitiateSessionAsync(), token);
    public Task HeartBeat(CancellationToken token)
        => Run(Api.HeartbeatAsync(), token);
    public Task Disconnect(CancellationToken token)
        => Run(Api.DisconnectAsync(), token);
}
