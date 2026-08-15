using HyRest.Cache;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.ApiServices;

public class OnBaseSessionService : OnBaseService<IOnBaseSessionAPI>, IOnBaseSessionService
{
    private readonly ILogger<OnBaseSessionService> _logger;
    public override ILogger<IOnBaseSessionService> Logger => _logger;
    public OnBaseSessionService(IOnBaseAppCache cache, IHylandClientFactory hylandClientFactory, ILogger<OnBaseSessionService> logger)
        : base(cache, hylandClientFactory)
    {
        _logger = logger;
    }

    public Task InitiateSession(CancellationToken token)
        => Run(Api.InitiateSessionAsync(), token);
    public Task HeartBeat(CancellationToken token)
        => Run(Api.HeartbeatAsync(), token);
    public Task Disconnect(CancellationToken token)
        => Run(Api.DisconnectAsync(), token);
}
