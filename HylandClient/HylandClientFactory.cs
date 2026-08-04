using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace HyRest;

public class HylandClientFactory : IHylandClientFactory
{
    private readonly IServiceProvider _serviceProvider;   
    private readonly IHylandClientOptions _options;
    private readonly IHttpClientFactory _factory;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly HylandAuthClient? _authClient;
    private readonly SessionCookieClientHandler _cookieClientHandler;
    /// <summary>
    /// Constructor for basic authentication
    /// </summary>
    /// <param name="options"></param>
    /// <param name="credentials"></param>
    public HylandClientFactory(IHylandClientOptions options, IAuthenticationCredentials credentials)
    {        
        //We must create the service provider, and provide the credentials, and a client for authentication.
        //THe BearTokenHandler will be initialized with the HylandAuthClient and provide the bearer token header.
        _options = options;
        var services = new ServiceCollection();
        services.AddSingleton(credentials);
        services.AddTransient(sp =>
        {
            return options;
        });
        services.AddHttpClient<HylandAuthClient>(client =>
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
        _serviceProvider = services.BuildServiceProvider();
        _factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        _cookieClientHandler = _serviceProvider.GetRequiredService<SessionCookieClientHandler>();
        _authClient = _serviceProvider.GetRequiredService<HylandAuthClient>()
            .WithCredentials(credentials);
        _authClient.AuthenticateAsync().Wait();
    }
    public CookieContainer? CookieContainer => _cookieClientHandler.CookieContainer;
    //public HttpContextAccessor HttpContextAccessor => (HttpContextAccessor)_contextAccessor;
    /// <summary>
    /// Constructor for Depandancy Injection
    /// </summary>
    /// <param name="serviceProvider"></param>
    public HylandClientFactory(IServiceProvider serviceProvider)
    {
        //In this scenario, the Duende Access Token Management should handle Authentication
        _serviceProvider = serviceProvider;
        _factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        _cookieClientHandler = _serviceProvider.GetRequiredService<SessionCookieClientHandler>();
        //_contextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
    }
    //public void GetHttpUser()
    //{
    //    var context = HttpContextAccessor.HttpContext;
    //    var userClaim = context.User;
    //    //if(userClaim != null)
    //    //{
    //    //    var username = userClaim.Claims.FirstOrDefault()
    //    //}
    //}
    public TApi CreateClient<TApi>() where TApi : IHylandRestAPI
    {
        var client = _factory.CreateClient(nameof(HylandApiClient));
        return IHylandRestAPI.Get<TApi>(client);
    }
}
