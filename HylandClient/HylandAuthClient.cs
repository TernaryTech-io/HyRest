using Duende.IdentityModel.Client;
using HyRest.Identity.Credentials;

namespace HyRest;
public abstract class HylandAuthClient : IHylandAuthClient
{
    protected internal IAuthenticationToken? _token { get; set; }
    protected internal IAuthenticationCredentials _credentials;
    protected internal HttpClient _httpClient { get; set; }
    protected internal DiscoveryDocumentResponse? _discoveryDocument { get; set; }
    protected internal UserInfo? _userInfo { get; set; }
    public HylandAuthClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public IAuthenticationToken? AuthToken => _token;
    public bool IsAuthenticated => _token is not null && !_token.IsExpired();
    public bool IsExpired => _token == null || _token.IsExpired();
    public abstract Task<IAuthenticationToken> AuthenticateAsync();
    public UserInfo? UserInfo
    {
        get
        {
            if (_userInfo == null)
                GetUserInfoAsync().Wait();
            return _userInfo;
        }
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
    protected internal async Task GetDiscoveryDocumentAsync()
        => _discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync();
    public abstract Task GetUserInfoAsync();
    public virtual IHylandAuthClient WithCredentials(IAuthenticationCredentials credentials)
    {
        _credentials = credentials;
        return this;
    }
}
