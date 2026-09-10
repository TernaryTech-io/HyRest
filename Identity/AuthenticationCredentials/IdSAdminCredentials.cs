using HyRest.Hyland.IdentityAdministration;
using System.ComponentModel;

namespace HyRest;

/// <summary>
/// Credential set for authenticating to the Identity Server Admin API
/// </summary>
public class IdentityAdminCredentials : AuthenticationCredentials
{
    public override GrantType GrantType { get; set; } = GrantType.ClientCredentials;
    public override List<Scope> Scopes { get; set; } = [ Scope.IdpAdmin ];  
}