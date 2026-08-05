using Duende.IdentityModel.Client;
using Duende.AccessTokenManagement.OpenIdConnect;
using HyRest.Identity.Credentials;
using Microsoft.AspNetCore.Http;

namespace HyRest;

public class HylandOpenIdAuthClient : HylandAuthClient
{
    private IHttpContextAccessor _contextAccessor;
    public override HylandOpenIdAuthClient WithCredentials(IAuthenticationCredentials credentials)
    {
        if (credentials is OpenIdCredentials openid)
            _credentials = openid;
        else throw new Exception("Basic User Credentials are required for this Authentication Client");
        return this;
    }
    public HylandOpenIdAuthClient WithContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
        return this;
    }
    public override async Task<IAuthenticationToken> AuthenticateAsync()
    {
        if (_discoveryDocument == null)
            await GetDiscoveryDocumentAsync();
        var credentials = (OpenIdCredentials)_credentials;
        var token = await _contextAccessor.HttpContext.GetUserAccessTokenAsync();
        if (!token.Succeeded)
            throw new Exception("Could not retrieve token");
        _token = new AuthenticationToken
        {
            AccessToken = token.Token.AccessToken,
            ExpiresIn = (token.Token.Expiration - DateTimeOffset.Now).Seconds,
            Scope = credentials.Scope,
            TokenType = token.Token.AccessTokenType.HasValue ? token.Token.AccessTokenType.Value : string.Empty
        };
        return _token;
    }

    public override async Task GetUserInfoAsync()
    {
        if (_discoveryDocument == null)
            await GetDiscoveryDocumentAsync();
        if (_token == null)
            await AuthenticateAsync();
        var user = await _httpClient.GetUserInfoAsync(new UserInfoRequest
        {
            Address = _discoveryDocument.UserInfoEndpoint,
            Token = _token.AccessToken,
        });
        _userInfo = new UserInfo
        {
            UserId = user.Claims.FirstOrDefault(c => c.Type == "sub").Value,
            UserName = user.Claims.FirstOrDefault(c => c.Type == "username").Value,
            RealName = user.Claims.FirstOrDefault(c => c.Type == "name").Value,
            Email = user.Claims.FirstOrDefault(c => c.Type == "email").Value
        };
    }
    public HylandOpenIdAuthClient(HttpClient httpClient) : base(httpClient)
    {
        
    }
}