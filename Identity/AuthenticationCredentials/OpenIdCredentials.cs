using System.ComponentModel;

namespace HyRest;

/// <summary>
/// Credential set for authenticating to the Identity Server Admin API
/// </summary>
public class OpenIdCredentials : AuthenticationCredentials
{
    private List<string> _scope { get; set; } = ["openid", "evolution", "profile", "profile.onbase"];
    [DefaultValue("authorization_code")]
    public new string GrantType { get => base.GrantType ?? "authorization_code"; set => base.GrantType = value; }
    public override string Scope { get => string.Join(" ", _scope); }
    public override string ClientId { get; set; }
    public override string ClientSecret { get; set; }
    #region notused
    private new string? Tenant { get => null; set => base.Tenant = null; }
    private new string? Username { get => null; set => base.Username = null; }
    private new string? Password { get => null; set => base.Password = null; }
    #endregion
    public void AddScope(string scope) => _scope.Add(scope);
    public void ClearScope() => _scope.Clear();
    public IReadOnlyList<string> ScopeCollection => _scope;
    public string CallbackPath { get; set; }
    public string SignedOutCallbackPath { get; set; }
    public string SignedOutRedirectUri { get; set; } = "/";
}