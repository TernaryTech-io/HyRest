using HyRest.Cache;
using HyRest.OnBase.ApiServices;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
    public OnBaseAppBuilder(IAuthenticationCredentials credentials, HylandClientOptions options, IServiceCollection? serviceCollection = null)
    {
        _authCredentials = credentials;
        ServiceCollection = serviceCollection ?? new ServiceCollection();
        _options = options;
        ServiceCollection.AddSingleton(_options);
        ServiceCollection.AddLogging(options =>
        {
            if (Environment.UserInteractive)
                options.AddConsole();
            options.SetMinimumLevel(_options.LogLevel);
        });
        ServiceCollection.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(12),
                LocalCacheExpiration = TimeSpan.FromMinutes(60),
            };
        });
        ServiceCollection.AddSingleton<HylandClientFactory>();
        HylandClientFactory.RegisterBasicAuthServices(ServiceCollection, _options, _authCredentials);        
    }
    public static void RegisterAppServices<T>(IServiceCollection sc)
        where T : class, IOnBaseApp
    {        
        sc.AddSingleton<OnBaseAppCache>();
        sc.AddSingleton<OnBaseSessionService>();
        sc.AddSingleton<OnBaseCoreService>();
        sc.AddSingleton<OnBaseWorkViewService>();
        sc.AddSingleton<OnBaseAdministrationService>();

        if (typeof(T) == typeof(OnBaseApp))
            sc.AddSingleton<OnBaseApp>();
        else
            sc.AddScoped<OnBaseScopedApp>();


    }
    /// <summary>
    /// Entry point for creating an OnBase App builder using standard credentials.
    /// </summary>
    /// <param name="credentials"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static OnBaseAppBuilder Create(IAuthenticationCredentials credentials, HylandClientOptions options)
        => new OnBaseAppBuilder(credentials, options);
}
