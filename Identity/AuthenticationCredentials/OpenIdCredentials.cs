using HyRest.Hyland.IdentityAdministration;
using System.ComponentModel;

namespace HyRest;

/// <summary>
/// Credential set for authenticating to the Identity Server Admin API
/// </summary>
public class OpenIdCredentials : AuthenticationCredentials
{
    public override GrantType GrantType { get; set; } = GrantType.AuthorizationCode;
    public override List<Scope> Scopes { get; set; } = [ Scope.OpenId, Scope.Evolution, Scope.Profile, Scope.ProfileOnbase ];
    public string CallbackPath { get; set; }
    public string SignedOutCallbackPath { get; set; }
    public string SignedOutRedirectUri { get; set; } = "/";
}