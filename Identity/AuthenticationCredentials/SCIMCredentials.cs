using System.ComponentModel;

namespace HyRest;
public class SCIMCredentials : AuthenticationCredentials
{
    [DefaultValue("client_credentials")]
    internal new string GrantType { get => base.GrantType ?? "client_credentials"; set => base.GrantType = value; }
    [DefaultValue("iam.user-catalog iam.user-catalog.read iam.user-catalog.write")]
    internal new string Scope { get => base.Scope ?? "openid"; set => base.Scope = value; }    
    public required new string? ClientId { get => base.ClientId ?? string.Empty; set => base.ClientId = value; }    
    public required new string? ClientSecret { get => base.ClientSecret ?? string.Empty; set => base.ClientSecret = value; }
    private new string? Tenant { get => null; set => base.Tenant = null; }
    private new string? Username { get => null; set => base.Username = null; }
    private new string? Password { get => null; set => base.Password = null; }
}
public class ReadOnlySCIMCredentials : AuthenticationCredentials
{
    [DefaultValue("client_credentials")]
    internal new string GrantType { get => base.GrantType ?? "client_credentials"; set => base.GrantType = value; }
    [DefaultValue("iam.user-catalog iam.user-catalog.read")]
    internal new string Scope { get => base.Scope ?? "openid"; set => base.Scope = value; }
    public required new string ClientId { get => base.ClientId ?? string.Empty; set => base.ClientId = value; }
    public required new string ClientSecret { get => base.ClientSecret ?? string.Empty; set => base.ClientSecret = value; }
    private new string? Tenant { get => null; set => base.Tenant = null; }
    private new string? Username { get => null; set => base.Username = null; }
    private new string? Password { get => null; set => base.Password = null; }
}