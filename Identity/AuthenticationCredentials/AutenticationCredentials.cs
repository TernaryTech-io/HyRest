using System.Text.Json.Serialization;

namespace HyRest.Identity.Credentials;

public class AuthenticationCredentials : IAuthenticationCredentials
{
    [JsonPropertyName("grant_type")]
    public virtual string? GrantType { get; set; }

    [JsonPropertyName("scope")]
    public virtual string? Scope {get; set;}

    [JsonPropertyName("client_id")]
    public virtual string? ClientId { get; set; }

    [JsonPropertyName("client_secret")]
    public virtual string? ClientSecret { get; set; }

    [JsonPropertyName("username")]
    public virtual string? Username { get; set; }

    [JsonPropertyName("password")]
    public virtual string? Password { get;  set; }

    [JsonPropertyName("tenant")]
    public virtual string? Tenant { get; set; }
    public FormUrlEncodedContent ToBody()
    {
        var dict = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(GrantType))
            dict["grant_type"] = GrantType;
        if (!string.IsNullOrEmpty(Scope))
            dict["scope"] = Scope;
        if (!string.IsNullOrEmpty(ClientId))
            dict["client_id"] = ClientId;
        if (!string.IsNullOrEmpty(ClientSecret))
            dict["client_secret"] = ClientSecret;
        if (!string.IsNullOrEmpty(Tenant))
            dict["tenant"] = Tenant;
        if (!string.IsNullOrEmpty(Username))
            dict["username"] = Username;
        if (!string.IsNullOrEmpty(Password))
            dict["password"] = Password;
        return new FormUrlEncodedContent(dict);
    }
    public static SCIMCredentials CreateSCIMCredentials(string clientId, string clientSecret)
    {
        return new SCIMCredentials
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }
    public static ReadOnlySCIMCredentials CreateReadOnlySCIMCredentials(string clientId, string clientSecret)
    {
        return new ReadOnlySCIMCredentials
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }
    public static IdSAdminCredentials CreateIdsAdminCredentials(string clientId, string clientSecret)
    {
        return new IdSAdminCredentials
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }
    /// <summary>
    /// Create an instance of authentication credentials for user access to the Rest API
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <returns></returns>
    public static BasicUserCredentials CreateUserCredentials(string username, string password, string clientId, string clientSecret)
    {
        return new BasicUserCredentials
        {
            Username = username,
            Password = password,
            ClientId = clientId,
            ClientSecret = clientSecret,
            GrantType = "password",
            Scope = "evolution"
        };
    }
}

