using HyRest.Identity.Credentials;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace HyRest;

public sealed class HylandClientFactory : IHylandClientFactory
{
    private IServiceProvider _serviceProvider;   
    private IHttpClientFactory _factory;
    private IHylandAuthClient _authClient;
    private SessionCookieClientHandler _cookieClientHandler;
    internal HylandClientFactory(IServiceProvider serviceProvider, IAuthenticationCredentials credentials)
    {
        _serviceProvider = serviceProvider;
        _factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        _cookieClientHandler = _serviceProvider.GetRequiredService<SessionCookieClientHandler>();
        _authClient = _serviceProvider.GetRequiredService<HylandBasicAuthClient>()
            .WithCredentials(credentials);
        _authClient.AuthenticateAsync().Wait();
    }
    public static IServiceCollection RegisterServices(IServiceCollection services, IHylandClientOptions options, IAuthenticationCredentials credentials)
    {        
        services.AddSingleton(credentials);
        services.AddTransient(sp =>
        {
            return options;
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
        services.AddHttpClient<HylandApiClient>(client =>
        {
            client.BaseAddress = new Uri(options.ApiBaseUrl);
        })
        .ConfigurePrimaryHttpMessageHandler(sp => sp.GetRequiredService<SessionCookieClientHandler>())
        .AddHttpMessageHandler<BearerTokenHandler>();
        return services;
    }
    
    public CookieContainer? CookieContainer => _cookieClientHandler.CookieContainer;
    public UserInfo? UserInfo => _authClient.UserInfo;
    /// <summary>
    /// Constructor for Depandancy Injection
    /// </summary>
    /// <param name="serviceProvider"></param>
    public HylandClientFactory(IServiceProvider serviceProvider, OpenIdCredentials credentials, IHttpContextAccessor contextAccessor)
    {
        //In this scenario, the Duende Access Token Management should handle Authentication
        _serviceProvider = serviceProvider;
        _factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        _cookieClientHandler = _serviceProvider.GetRequiredService<SessionCookieClientHandler>();
        _authClient = _serviceProvider.GetRequiredService<HylandOpenIdAuthClient>()
            .WithCredentials(credentials)
            .WithContextAccessor(contextAccessor);
        
    }
    public TApi CreateClient<TApi>() where TApi : IHylandRestAPI
    {
        var client = _factory.CreateClient(nameof(HylandApiClient));
        return IHylandRestAPI.Get<TApi>(client);
    }
}
