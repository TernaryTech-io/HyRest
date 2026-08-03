using System.ComponentModel;

namespace HyRest.Identity.Credentials;

/// <summary>
/// Basic Authentication requiring a username, password and Client Id & Secret.
/// </summary>
public class BasicUserCredentials : AuthenticationCredentials
{
    [DefaultValue("password")]
    public new string GrantType { get => base.GrantType ?? "password"; set => base.GrantType = value; }
    [DefaultValue("evolution")]
    public new string Scope { get => base.Scope ?? "evolution"; set => base.Scope = value; }
    [DefaultValue("OnBase")]
    public new string Tenant { get => base.Tenant ?? "OnBase"; set => base.Tenant = value; }
    public required new string ClientId { get => base.ClientId ?? string.Empty; set => base.ClientId = value; }
    public required new string ClientSecret { get => base.ClientSecret ?? string.Empty; set => base.ClientSecret = value; }
    public required new string Username { get => base.Username ?? string.Empty; set => base.Username = value; }
    public required new string Password { get => base.Password ?? string.Empty; set => base.Password = value; }
}