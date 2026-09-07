using HyRest.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Refit;


namespace HyRest;

public sealed class IdentityAdminClientFactory : IHylandClientFactory
{
    public IdentityAdminClientFactory(IServiceProvider serviceProvider, IAuthenticationCredentials credentials)
    {
        _serviceProvider = serviceProvider;
        _factory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        _authClient = _serviceProvider.GetRequiredService<HylandCredentialAuthClient>()
            .WithCredentials(credentials);
        _authClient.AuthenticateAsync().Wait(TimeSpan.FromSeconds(180));
        _apiClient = _serviceProvider.GetRequiredService<IdentityAdminApiClient>();
        _options = _serviceProvider.GetRequiredService<HylandClientOptions>();
    }
    public static IServiceCollection RegisterCredentialAuthServices(IServiceCollection services, HylandClientOptions options, IAuthenticationCredentials credentials)
    {
        services.AddSingleton(credentials);
        services.AddTransient(sp =>
        {
            return options;
        });
        services.AddHttpClient<HylandCredentialAuthClient>(client =>
        {
            client.BaseAddress = new Uri(options.IdsBaseUrl);
        });
        services.AddTransient<BearerTokenHandler>();
        services.AddHttpClient<IdentityAdminApiClient>(client =>
        {
            client.BaseAddress = new Uri(options.IdsBaseUrl);
        })
        .AddHttpMessageHandler<BearerTokenHandler>(); 
        return services;
    }
    private IServiceProvider _serviceProvider;
    private IHttpClientFactory _factory;
    private readonly IHylandAuthClient _authClient;
    private readonly IHylandApiClient _apiClient;
    private HylandClientOptions _options;

    public IHylandApiClient ApiClient => _apiClient;

    public IHylandAuthClient AuthClient => _authClient;

    public IHylandClientOptions ClientOptions => _options;

    public TApi CreateClient<TApi>() where TApi : IHylandRestAPI
    {
        var client = _factory.CreateClient(nameof(IdentityAdminApiClient));
        return IHylandRestAPI.Get<TApi>(client, Settings);
    }
    public static RefitSettings Settings =>
        new RefitSettings
        {
            CaptureRequestContent = true,
            ContentSerializer = new SystemTextJsonContentSerializer(JsonUtility.Options)
        };
}
