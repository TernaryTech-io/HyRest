using HyRest.Hyland.IdentityAdministration;

namespace HyRest;

/// <summary>
/// Basic Authentication requiring a username, password and Client Id & Secret.
/// </summary>
public class BasicUserCredentials : AuthenticationCredentials
{
    public new GrantType GrantType => GrantType.ClientCredentials;
    public override List<Scope> Scopes => [Scope.Evolution];
    public required override string? Username { get; set; }
    public required override string? Password { get; set; }
}