namespace HyRest;

public class HylandApiClient : IHylandApiClient
{
    private readonly HttpClient _httpClient;
    internal HylandApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public HttpClient HttpClient => _httpClient;
}

