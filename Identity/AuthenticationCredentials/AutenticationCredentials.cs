using HyRest.Hyland.IdentityAdministration;
using System.Text.Json.Serialization;

namespace HyRest;

public class AuthenticationCredentials : IAuthenticationCredentials
{
    [JsonPropertyName("grant_type")]
    public virtual GrantType GrantType { get; set; }

    [JsonPropertyName("scope")]
    public virtual List<Scope> Scopes { get; set; } = [];

    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }

    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("username")]
    public virtual string? Username { get; set; }

    [JsonPropertyName("password")]
    public virtual string? Password { get;  set; }

    [JsonPropertyName("tenant")]
    public virtual string? Tenant { get; set; }
    public FormUrlEncodedContent ToBody()
    {
        var dict = new Dictionary<string, string>();
        dict["grant_type"] = GrantType.Value;
        if (Scopes.Count > 0)
            dict["scope"] = string.Join(" ", Scopes.Select(s => s.Value));
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
        return new SCIMCredentials()
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }
    public static ReadOnlySCIMCredentials CreateReadOnlySCIMCredentials(string clientId, string clientSecret)
    {
        return new ReadOnlySCIMCredentials()
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }
    public static IdentityAdminCredentials CreateIdentityAdminCredentials(string clientId, string clientSecret)
    {
        return new IdentityAdminCredentials()
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
        return new BasicUserCredentials()
        {
            Username = username,
            Password = password,
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }
}

