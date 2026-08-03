namespace HyRest;
public class HylandAuthClient : IHylandAuthClient
{
    private IHylandIdentityServiceAuthenticationAPI _api;
    private IAuthenticationToken? _token { get; set; }
    private IAuthenticationCredentials _credentials;
    private HttpClient _httpClient { get; set; }
    /// <summary>
    /// Hyland Http Client for basic authentication
    /// </summary>
    /// <param name="httpClient"></param>
    public HylandAuthClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _api = IHylandRestAPI.Get<IHylandIdentityServiceAuthenticationAPI>(_httpClient);
    }
    public IAuthenticationToken? AuthToken => _token;
    public bool IsAuthenticated => _token is not null && !_token.IsExpired();
    public bool IsExpired => _token == null || _token.IsExpired();
    public HylandAuthClient WithCredentials(IAuthenticationCredentials credentials)
    {
        _credentials = credentials;
        return this;
    }
    public async Task<string> GetAccessTokenAsync()
    {
        if (!IsAuthenticated)
            await AuthenticateAsync();
        if (_token?.AccessToken != null)
            return _token.AccessToken;
        throw new Exception("Failed to retrieve access token. User is not authenticated.");
    }
    public string GetAccessToken()
    {
        GetAccessTokenAsync().Wait();
        return _token?.AccessToken
            ?? throw new Exception("Failed to retrieve access token. User is not authenticated.");
    }
    /// <summary>
    /// Authenticate to the Hyland
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<IAuthenticationToken> AuthenticateAsync()
    {
        var response = await _api.GetAuthToken(_credentials.ToBody());
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Authentication failed: {response.StatusCode}");
        _token = response.Content;
        return _token;
    }
    public static HylandAuthClient Create(HttpClient httpClient, IAuthenticationCredentials credentials)
        => new HylandAuthClient(httpClient).WithCredentials(credentials);
}


