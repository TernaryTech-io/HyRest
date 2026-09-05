namespace HyRest;

public class IdentityAdminApiClient : IHylandApiClient
{
    private readonly IHylandIdentityServiceAdministrationAPI _api;
    private readonly HttpClient _httpClient;
    public IdentityAdminApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _api = IHylandRestAPI.Get<IHylandIdentityServiceAdministrationAPI>(_httpClient, OnBaseClientFactory.Settings);
    }
    public HttpClient HttpClient => _httpClient;
}
