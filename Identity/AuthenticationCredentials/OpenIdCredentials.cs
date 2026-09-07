using HyRest.Hyland.IdentityAdministration;
using System.ComponentModel;

namespace HyRest;

/// <summary>
/// Credential set for authenticating to the Identity Server Admin API
/// </summary>
public class OpenIdCredentials : AuthenticationCredentials
{
    public new GrantType GrantType => GrantType.AuthorizationCode;
    public override List<Scope> Scopes => [ Scope.OpenId, Scope.Evolution, Scope.Profile, Scope.ProfileOnbase ];
    public override string ClientId { get; set; }
    public override string ClientSecret { get; set; }
    public void AddScope(Scope scope) => Scopes.Add(scope);
    public void ClearScope() => Scopes.Clear();
    public string CallbackPath { get; set; }
    public string SignedOutCallbackPath { get; set; }
    public string SignedOutRedirectUri { get; set; } = "/";
}