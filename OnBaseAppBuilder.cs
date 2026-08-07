using HyRest.Cache;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyRest;

public class OnBaseAppBuilder
{
    public IServiceCollection Services { get; set; } = new ServiceCollection();
    private HylandClientFactory _clientFactory { get; set; }
    private OnBaseCacheFactory _cacheFactory { get; set; }
    internal OnBaseAppBuilder(IAuthenticationCredentials credentials, Action<HylandClientOptions> optionsAction)
    {
        var options = new HylandClientOptions();
        optionsAction(options);
        _clientFactory = HylandClientFactory.Create(options, credentials);
        _clientFactory.RegisterServices(Services);
        OnBaseCacheFactory.RegisterServices(Services);
    }
}
