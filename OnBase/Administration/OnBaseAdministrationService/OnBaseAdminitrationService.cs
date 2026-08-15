using HyRest.Cache;
using HyRest.OnBase.ApiServices;

namespace HyRest.OnBase.Administration;

public partial class OnBaseAdministrationService : OnBaseService<IOnBaseAdministrationAPI>, IOnBaseAdministrationService
{
    public OnBaseAdministrationService(IOnBaseAppCache cache, IHylandClientFactory hylandClientFactory) : base(cache, hylandClientFactory)
    {
    }
}
