namespace HyRest;

public class HylandApiClient : IHylandApiClient
{
    private readonly HttpClient _httpClient;
    public HylandApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public HttpClient HttpClient => _httpClient;
}

