using System.Security;
using System.Text.Json.Serialization;

namespace HyRest.Identity;

public sealed class AuthenticationToken : IAuthenticationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = "";
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "";
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = "";
    private DateTime _experation { get; set; } = DateTime.UtcNow;
    public DateTime Expiration { get => _experation.AddSeconds(ExpiresIn); }
    public string AuthHeader { get => $"Bearer {AccessToken}"; }
    public bool IsExpired() => Expiration < DateTime.UtcNow;    
}

