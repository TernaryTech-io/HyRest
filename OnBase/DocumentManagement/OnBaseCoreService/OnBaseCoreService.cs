using HyRest.Cache;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    public OnBaseCoreService(OnBaseAppCache cache, OnBaseClientFactory hylandClientFactory, ILogger<OnBaseCoreService> logger)
        : base(cache, hylandClientFactory, logger)
    {
        
    }
}