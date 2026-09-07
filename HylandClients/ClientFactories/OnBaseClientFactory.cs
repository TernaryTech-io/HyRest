
using HyRest.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Net;

namespace HyRest;

public sealed class OnBaseClientFactory : IOnBaseClientFactory
{
    private IServiceProvider _serviceProvider;   
    private IHttpClientFactory _factory;
    private readonly IHylandAuthClient _authClient;
    private readonly IOnBaseApiClient _apiClient;
    private HylandClientOptions _options;
    private SessionCookieClientHandler _cookieClientHandler;
    public OnBaseClientFactory(IServiceProvider serviceProvider, IAuthenticationCredentials credentials)
    {
        _serviceProvider = serviceProvider;
        _factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        _cookieClientHandler = _serviceProvider.GetRequiredService<SessionCookieClientHandler>();
        _authClient = _serviceProvider.GetRequiredService<HylandBasicAuthClient>()
            .WithCredentials(credentials);
        _authClient.AuthenticateAsync().Wait(TimeSpan.FromSeconds(180));
        _apiClient = _serviceProvider.GetRequiredService<OnBaseApiClient>()
            .WithCookieContainer(_cookieClientHandler.CookieContainer);
        _options = _serviceProvider.GetRequiredService<HylandClientOptions>();
    }
    public static IServiceCollection RegisterBasicAuthServices(IServiceCollection services, HylandClientOptions options, IAuthenticationCredentials credentials)
    {        
        services.AddSingleton(credentials);
        services.AddTransient(sp =>
        {
            return (HylandClientOptions)options;
        });
        services.AddHttpClient<HylandBasicAuthClient>(client =>
        {
            client.BaseAddress = new Uri(options.IdsBaseUrl);
        });
        services.AddTransient(sp =>
        {
            return new SessionCookieClientHandler(options);
        });
        services.AddTransient<BearerTokenHandler>();
        services.AddHttpClient<OnBaseApiClient>(client =>
        {
            client.BaseAddress = new Uri(options.ApiBaseUrl);
        })
        .ConfigurePrimaryHttpMessageHandler(sp => sp.GetRequiredService<SessionCookieClientHandler>())
        .AddHttpMessageHandler<BearerTokenHandler>();
        return services;
    }    

    public IOnBaseApiClient ApiClient => _apiClient;
    public IHylandAuthClient AuthClient => _authClient;
    public IHylandClientOptions ClientOptions => _options;
    /// <summary>
    /// Constructor for Depandancy Injection
    /// </summary>
    /// <param name="serviceProvider"></param>
    public OnBaseClientFactory(IServiceProvider serviceProvider, OpenIdCredentials credentials, IHttpContextAccessor contextAccessor)
    {
        //In this scenario, the Duende Access Token Management should handle Authentication
        _serviceProvider = serviceProvider;
        _factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        _cookieClientHandler = _serviceProvider.GetRequiredService<SessionCookieClientHandler>();
        _authClient = _serviceProvider.GetRequiredService<HylandOpenIdAuthClient>()
            .WithCredentials(credentials)
            .WithContextAccessor(contextAccessor);
        _apiClient = _serviceProvider.GetRequiredService<OnBaseApiClient>()
            .WithCookieContainer(_cookieClientHandler.CookieContainer);
        _options = _serviceProvider.GetRequiredService<HylandClientOptions>();
    }
    public TApi CreateClient<TApi>() where TApi : IHylandRestAPI
    {
        var client = _factory.CreateClient(nameof(OnBaseApiClient));
        return IHylandRestAPI.Get<TApi>(client, Settings);
    }
    public static RefitSettings Settings =>
        new RefitSettings
        {
            CaptureRequestContent = true,
            ContentSerializer = new SystemTextJsonContentSerializer(JsonUtility.Options)            
        };
}
