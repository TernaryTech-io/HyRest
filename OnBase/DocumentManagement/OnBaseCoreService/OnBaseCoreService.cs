using HyRest.Cache;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    private readonly ILogger<OnBaseCoreService> _logger;
    public override ILogger<IOnBaseCoreService> Logger => _logger;
    public OnBaseCoreService(IOnBaseAppCache cache, IHylandClientFactory hylandClientFactory, ILogger<OnBaseCoreService> logger)
        : base(cache, hylandClientFactory)
    {
        _logger = logger;
    }
}