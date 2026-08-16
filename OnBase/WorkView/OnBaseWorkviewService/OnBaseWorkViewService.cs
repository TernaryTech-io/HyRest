using HyRest.Cache;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.ApiServices;

public partial class OnBaseWorkViewService : OnBaseService<IOnBaseWorkViewAPI>, IOnBaseWorkViewService
{
    public OnBaseWorkViewService(OnBaseAppCache cache, HylandClientFactory hylandClientFactory, ILogger<OnBaseWorkViewService> logger)
        : base(cache, hylandClientFactory, logger)
    {
        
    }
}
