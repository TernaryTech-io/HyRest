using HyRest.Cache;
using HyRest.Hyland.IdentityAdministration;
using HyRest.OnBase.ApiServices;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyRest;

public class HylandIdentityAppBuilder
{
    public IServiceCollection ServiceCollection { get; set; }
    internal IServiceProvider Services { get; set; }
    private IdentityAdminClientFactory _clientFactory { get; set; }
    private IAuthenticationCredentials? _authCredentials { get; set; }
    private HylandClientOptions _options { get; set; }
    public HylandIdentity Build()
    {
        RegisterAppServices(ServiceCollection, _options);
        Services = ServiceCollection.BuildServiceProvider();
        return Services.GetRequiredService<HylandIdentity>();
    }
    /// <summary>
    /// Entrypoint for Client Credentials
    /// </summary>
    /// <param name="credentials"></param>
    /// <param name="optionsAction"></param>
    public HylandIdentityAppBuilder(HylandClientOptions options, IdentityAdminCredentials? credentials = null, IServiceCollection? serviceCollection = null)
    {
        _authCredentials = credentials;
        ServiceCollection = serviceCollection ?? new ServiceCollection();
        _options = options;
        if (_authCredentials != null)
        {
            IdentityAdminClientFactory.RegisterCredentialAuthServices(ServiceCollection, _options, _authCredentials);
        }        
    }
    public static void RegisterAppServices(IServiceCollection sc, HylandClientOptions options)
    {
        sc.AddSingleton(options);
        sc.AddSingleton<IdentityAdminClientFactory>();
        sc.AddSingleton<IdentityAdminService>();
        sc.AddSingleton<HylandIdentity>();        
    }
}
