using HyRest.Hyland.IdentityAdministration;
using _Scope = HyRest.Hyland.IdentityAdministration.Scope;
using System.ComponentModel;

namespace HyRest;

/// <summary>
/// Credential set for authenticating to the Identity Server Admin API
/// </summary>
public class IdentityAdminCredentials : AuthenticationCredentials
{
    [DefaultValue("client_credentials")]
    public override GrantType GrantType => GrantType.ClientCredentials;
    [DefaultValue("idpadmin")]
    public override List<_Scope> Scopes => [ _Scope.IdpAdmin ];  
    public required override string? ClientId { get; set; }
    public required override string? ClientSecret { get; set; }
}