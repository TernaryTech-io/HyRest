using HyRest.Cache;
using HyRest.OnBase.ApiServices;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace HyRest.OnBase;

/// <summary>
/// OnBaseAppBuilder is used for registering services when dependency injection is not being used.
/// </summary>
public class OnBaseAppBuilder
{
    public IServiceCollection ServiceCollection { get; set; }
    public IServiceProvider Services { get; protected set; }
    private HylandClientFactory _clientFactory { get; set; }
    private IAuthenticationCredentials _authCredentials { get; set; }
    private HylandClientOptions _options { get; set; }
    public OnBaseApp Build()
    {
        RegisterAppServices<OnBaseApp>(ServiceCollection);
        Services = ServiceCollection.BuildServiceProvider();
        return Services.GetRequiredService<OnBaseApp>();
    }
    public OnBaseScopedApp BuildScoped()
    {
        RegisterAppServices<OnBaseScopedApp>(ServiceCollection);
        Services = ServiceCollection.BuildServiceProvider();
        return Services.GetRequiredService<OnBaseScopedApp>();
    }
    /// <summary>
    /// Entrypoint for Basic User Credentials.
    /// </summary>
    /// <param name="credentials"></param>
    /// <param name="optionsAction"></param>
    internal OnBaseAppBuilder(IAuthenticationCredentials credentials, Action<HylandClientOptions> optionsAction, IServiceCollection? serviceCollection = null)
    {
        _authCredentials = credentials;
        ServiceCollection = serviceCollection ?? new ServiceCollection();
        _options = new HylandClientOptions();
        optionsAction(_options);
        ServiceCollection.AddSingleton(_options);
        ServiceCollection.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(12),
                LocalCacheExpiration = TimeSpan.FromMinutes(60),
            };
        });
        HylandClientFactory.RegisterServices(ServiceCollection, _options, _authCredentials);        
    }
    public static void RegisterAppServices<T>(IServiceCollection sc)
        where T : class, IOnBaseApp
    {
        sc.AddSingleton<IOnBaseAppCache, OnBaseAppCache>();
        if (typeof(T) == typeof(OnBaseApp))
        {            
            sc.AddSingleton<OnBaseSessionService>();
            sc.AddSingleton<OnBaseSession>();
            sc.AddSingleton<OnBaseCoreService>();
            sc.AddSingleton<OnBaseCore>();
            sc.AddSingleton<OnBaseWorkViewService>();
            sc.AddSingleton<OnBaseWorkView>();
            sc.AddSingleton<OnBaseAdministrationService>();
            sc.AddSingleton<OnBaseAdministration>();
            sc.AddSingleton<OnBaseApp>();
        }
        else
        {
            sc.AddScoped<OnBaseSessionService>();
            sc.AddScoped<OnBaseSession>();
            sc.AddScoped<OnBaseCoreService>();
            sc.AddScoped<OnBaseCore>();
            sc.AddScoped<OnBaseWorkViewService>();
            sc.AddScoped<OnBaseWorkView>();
            sc.AddScoped<OnBaseAdministrationService>();
            sc.AddScoped<OnBaseAdministration>();
            sc.AddScoped<OnBaseScopedApp>();
        }
        
        
    }
    /// <summary>
    /// Entry point for creating an OnBase App builder using standard credentials.
    /// </summary>
    /// <param name="credentials"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static OnBaseAppBuilder Create(IAuthenticationCredentials credentials, Action<HylandClientOptions> optionsAction)
        => new OnBaseAppBuilder(credentials, optionsAction);
}
