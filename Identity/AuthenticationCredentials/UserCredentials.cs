using HyRest.Hyland.IdentityAdministration;
using System.ComponentModel;

namespace HyRest;

/// <summary>
/// Basic Authentication requiring a username, password and Client Id & Secret.
/// </summary>
public class BasicUserCredentials : AuthenticationCredentials
{    
    public override GrantType GrantType { get; set; } = GrantType.Password;
    public override List<Scope> Scopes { get; set; } = [Scope.Evolution];
    public required override string? Username { get; set; }
    public required override string? Password { get; set; }
}