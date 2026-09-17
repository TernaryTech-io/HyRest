using HyRest.Cache;
using HyRest.OnBase.ApiServices;
using Microsoft.Extensions.DependencyInjection;

namespace HyRest.OnBase;

/// <summary>
/// OnBaseAppBuilder is used for registering services when dependency injection is not being used.
/// </summary>
public class OnBaseAppBuilder
{
    public IServiceCollection ServiceCollection { get; set; }
    internal IServiceProvider Services { get; set; }
    private OnBaseClientFactory _clientFactory { get; set; }
    private IAuthenticationCredentials? _authCredentials { get; set; }
    private HylandClientOptions _options { get; set; }
    public OnBaseApp Build()
    {
        RegisterAppServices<OnBaseApp>(ServiceCollection, _options);
        Services = ServiceCollection.BuildServiceProvider();
        return Services.GetRequiredService<OnBaseApp>();
    }
    public OnBaseScopedApp BuildScoped()
    {
        RegisterAppServices<OnBaseScopedApp>(ServiceCollection, _options);
        Services = ServiceCollection.BuildServiceProvider();
        return Services.GetRequiredService<OnBaseScopedApp>();
    }
    /// <summary>
    /// Entrypoint for Basic User Credentials.
    /// </summary>
    /// <param name="credentials"></param>
    /// <param name="optionsAction"></param>
    public OnBaseAppBuilder(HylandClientOptions options, IAuthenticationCredentials? credentials = null, IServiceCollection? serviceCollection = null)
    {
        _authCredentials = credentials;
        ServiceCollection = serviceCollection ?? new ServiceCollection();
        _options = options;        
        if(_authCredentials != null)
        {            
            OnBaseClientFactory.RegisterBasicAuthServices(ServiceCollection, _options, _authCredentials);
        }
        ServiceCollection.AddSingleton<OnBaseClientFactory>();
    }
    public OnBaseAppBuilder WithCredentials(IAuthenticationCredentials credentials)
    {
        _authCredentials = credentials;
        OnBaseClientFactory.RegisterBasicAuthServices(ServiceCollection, _options, _authCredentials);
        return this;
    }
    public static void RegisterAppServices<T>(IServiceCollection sc, HylandClientOptions options)
        where T : class, IOnBaseApp
    {
        sc.AddSingleton(options);
        //sc.AddSingleton<MemoryCache>(sp =>
        //{
        //    var options = new MemoryCacheOptions()
        //    {
        //        ExpirationScanFrequency = TimeSpan.FromSeconds(60),
        //        SizeLimit = 2048
        //    };
        //    return new MemoryCache(options);
        //});
        //sc.AddHybridCache(options =>
        //{
        //    options.DefaultEntryOptions = new HybridCacheEntryOptions
        //    {
        //        Expiration = TimeSpan.FromMinutes(30),
        //        LocalCacheExpiration = TimeSpan.FromMinutes(15),
        //    };
        //});
        
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
    public static OnBaseAppBuilder Create(HylandClientOptions options, IAuthenticationCredentials? credentials = null)
        => new OnBaseAppBuilder(options, credentials);
}
