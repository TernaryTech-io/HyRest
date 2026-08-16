namespace HyRest;

public class HylandBasicAuthClient : HylandAuthClient
{
    private IHylandIdentityServiceAuthenticationAPI _api;
    /// <summary>
    /// Hyland Http Client for basic authentication
    /// </summary>
    /// <param name="httpClient"></param>
    public HylandBasicAuthClient(HttpClient httpClient) : base(httpClient)
    {        
        _api = IHylandRestAPI.Get<IHylandIdentityServiceAuthenticationAPI>(_httpClient, HylandClientFactory.Settings);
    }
    public override HylandBasicAuthClient WithCredentials(IAuthenticationCredentials credentials)
    {
        if (credentials is BasicUserCredentials basic)
            _credentials = basic;
        else throw new Exception("Basic User Credentials are required for this Authentication Client");
        return this;
    }    
    /// <summary>
    /// Authenticate to the Hyland
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override async Task<IAuthenticationToken> AuthenticateAsync()
    {
        var response = await _api.GetAuthToken(_credentials.ToBody());
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Authentication failed: {response.StatusCode}");
        _token = response.Content;
        return _token;
    }
    public override async Task GetUserInfoAsync()
    {
        _userInfo = new UserInfo
        {
            UserName = _credentials.Username
        };
    }
    public static HylandBasicAuthClient Create(HttpClient httpClient, IAuthenticationCredentials credentials)
        => new HylandBasicAuthClient(httpClient).WithCredentials(credentials);
}
