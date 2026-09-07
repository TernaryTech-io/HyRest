namespace HyRest;

public class HylandCredentialAuthClient : HylandAuthClient
{
    private IHylandIdentityServiceAuthenticationAPI _api;
    public HylandCredentialAuthClient(HttpClient httpClient) : base(httpClient)
    {
        _api = IHylandRestAPI.Get<IHylandIdentityServiceAuthenticationAPI>(_httpClient, OnBaseClientFactory.Settings);
    }
    /// <summary>
    /// Authenticate to the Hyland
    /// </summary>
    /// <returns cref="IAuthenticationToken">IAuthenticationToken</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override async Task<IAuthenticationToken> AuthenticateAsync()
    {
        var body = _credentials.ToBody();
        var response = await _api.GetAuthToken(body);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Authentication failed: {response.StatusCode}");
        _token = response.Content;
        return _token;
    }
    public override Task GetUserInfoAsync()
    {
        throw new NotImplementedException();
    }
}
