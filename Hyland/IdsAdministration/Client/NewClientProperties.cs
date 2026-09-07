using HyRest.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace HyRest.Hyland.IdentityAdministration;

public class ModifyClientProperties : NewClientProperties
{
    public readonly string ClientId;
    public ModifyClientProperties(IdentityAdmin admin, Client client) : base(admin, client)
    {
        ClientId = client.ClientId;
    }
}

public class NewClientProperties
{
    private readonly CreateModifyClient _model;
    private readonly IdentityAdmin _admin;
    public NewClientProperties(IdentityAdmin admin, CreateModifyClient? createModifyClient = null)
    {
        if (createModifyClient == null)
            createModifyClient = new CreateModifyClient();
        _model = createModifyClient;
        _admin = admin;
    }
    public bool Enabled { get => _model.Enabled; set => _model.Enabled = value; }
    [StringLength(256, MinimumLength = 1)]
    [RegularExpression(@"^[A-Za-z0-9_ -]+$")]
    public string ClientName { get => _model.ClientName; set => _model.ClientName = value; }

    [StringLength(256)]
    public string Description { get => _model.Description; set => _model.Description = value; }

    public string ProtocolType { get => _model.ProtocolType; set => _model.ProtocolType = value; }

    public bool IncludeXFrameOptions { get => _model.IncludeXFrameOptions; set => _model.IncludeXFrameOptions = value; }
    public ICollection<string> RedirectUris => _model.RedirectUris;
    [Required]
    public ICollection<string> AllowedFrameAncestors => _model.AllowedFrameAncestors;
    [Required]
    public TokenSettings TokenSettings { get => _model.TokenSettings; set => _model.TokenSettings = value; }
    [Required]
    public LogoutSettings LogoutSettings { get => _model.LogoutSettings; set => _model.LogoutSettings = value; }
    [Required]
    public AuthenticationRestrictionSettings AuthenticationRestrictionSettings { get => _model.AuthenticationRestrictionSettings; set => _model.AuthenticationRestrictionSettings = value; }
    [Required]
    public PkceSettings PkceSettings { get => _model.PkceSettings; set => _model.PkceSettings = value; }
    [Required]
    public DeviceFlowSettings DeviceFlowSettings { get => _model.DeviceFlowSettings; set => _model.DeviceFlowSettings = value; }
    [Required]
    public SecretSettingsModel SecretSettings { get => _model.SecretSettings; set => _model.SecretSettings = value; }
    [Required]
    public SecuritySettings SecuritySettings { get => _model.SecuritySettings; set => _model.SecuritySettings = value; }

    public void AddRedirectUri(string redirectUri)
    {
        _model.RedirectUris.Add(redirectUri);
    }
    public void AddPostLogoutRedirectUri(string postLogoutRedirectUri)
    {
        _model.RedirectUris.Add(postLogoutRedirectUri);
    }

    /// <summary>
    /// Add a grant type to the allowed grant types.
    /// </summary>
    /// <param name="grant" cref="GrantType">Select from a predefined grant type.</param>
    public void AddGrant(GrantType grant)
    {
        _model.AuthenticationRestrictionSettings.AllowedGrantTypes.Add(grant.Value);
    }
    /// <summary>
    /// Add a scope to the authentication settings.
    /// </summary>
    /// <param name="scope" cref="Scope">Select from a predefined scope.</param>
    public void AddScope(Scope scope)
    {
        _model.AuthenticationRestrictionSettings.AllowedScopes.Add(scope.Value);
    }
    /// <summary>
    /// Create a new secret. Will automatically change the RequireClientSecret flag to true.
    /// </summary>    
    /// <param name="expiration" cref="DateTimeOffset">Set an Expiration, max 180 days.</param>
    /// <param name="description">Optional description, one will be generated if left blank.</param>
    /// <param name="value">Optional value. If left blank a string will be generated and returned by this method.</param>
    /// <param name="type" cref="SecretType">Default is SharedSercret</param>
    /// <param name="hash">Optional, If true, the method will hash the value. Default is true.</param>
    /// <returns>Returns the provided value for the secret, or the auto-generated value if none was provided.</returns>
    public async Task<string> CreateNewSecret(
        DateTimeOffset expiration,
        string? description = null,
        string? value = null,
        SecretType type = SecretType.SharedSecret,
        bool hash = true)
    {
        if (expiration > DateTime.Now.AddDays(180))
            throw new Exception("Expiration must be 180 days or less.");
        bool autoGen = value == null;
        if (autoGen)
            value = PasswordGenerator.GenerateRandomPassword(28, true,true,true,false);
        if (value == null)
            throw new Exception("Failed to generate secret value");
        string originalValue = value;
        if (hash)
            value = Sha512(value);
        var secret = new ClientSecret()
        {
            Description = description ?? $"Secret that was {(autoGen ? "auto-generated" : "provided")}.",
            Expiration = expiration,
            Value = value,
            Hashed = hash,
            IsAutoGenerated = autoGen,
        };
        _model.SecretSettings.RequireClientSecret = true;
        var newSecret = await _admin.Service.CreateClientSecretAsync(_admin.Tenant.Id, secret);        
        _model.SecretSettings.ClientSecrets.Add(newSecret);

        return originalValue;
    }
    private string Sha512(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA512.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
    internal CreateModifyClient ToModel()
        => _model;
}
