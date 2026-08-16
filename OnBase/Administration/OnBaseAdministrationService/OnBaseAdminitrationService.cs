using HyRest.Cache;
using HyRest.OnBase.ApiServices;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.Administration;

public partial class OnBaseAdministrationService : OnBaseService<IOnBaseAdministrationAPI>, IOnBaseAdministrationService
{
    public OnBaseAdministrationService(OnBaseAppCache cache, HylandClientFactory hylandClientFactory, ILogger<OnBaseAdministrationService> logger) 
        : base(cache, hylandClientFactory, logger)
    {
    }
}
