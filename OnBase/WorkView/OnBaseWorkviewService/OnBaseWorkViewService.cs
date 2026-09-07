using HyRest.Cache;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.ApiServices;

public partial class OnBaseWorkViewService : OnBaseService<IOnBaseWorkViewAPI>, IOnBaseWorkViewService
{
    public OnBaseWorkViewService(OnBaseAppCache cache, OnBaseClientFactory hylandClientFactory, ILogger<OnBaseWorkViewService> logger)
        : base(cache, hylandClientFactory, logger)
    {
        
    }
}
