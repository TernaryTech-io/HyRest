using HyRest.Cache;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.ApiServices;

public partial class OnBaseWorkViewService : OnBaseService<IOnBaseWorkViewAPI>, IOnBaseWorkViewService
{
    private readonly ILogger<OnBaseWorkViewService> _logger;
    public override ILogger<IOnBaseWorkViewService> Logger => _logger;
    public OnBaseWorkViewService(IOnBaseAppCache cache, IHylandClientFactory hylandClientFactory, ILogger<OnBaseWorkViewService> logger)
        : base(cache, hylandClientFactory)
    {
        _logger = logger;
    }
}
