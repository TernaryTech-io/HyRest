using HyRest.Cache;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace HyRest;

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
        Services = ServiceCollection.BuildServiceProvider();
        return Services.GetRequiredService<OnBaseApp>();
    }
    public OnBaseScopedApp BuildScoped()
    {
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
        RegisterKnownServices();
    }
    private void RegisterKnownServices()
    {
        ServiceCollection.AddSingleton(_options);
        HylandClientFactory.RegisterServices(ServiceCollection, _options, _authCredentials);
        ServiceCollection.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromHours(12),
                LocalCacheExpiration = TimeSpan.FromMinutes(60),
            };
        });
        ServiceCollection.AddSingleton<IOnBaseAppCache, OnBaseAppCache>();
        ServiceCollection.AddTransient<OnBaseApp>();
        ServiceCollection.AddScoped<OnBaseScopedApp>();
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
