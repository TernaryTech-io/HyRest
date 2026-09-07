using System.Text.Json.Serialization;

namespace HyRest.Hyland.IdentityAdministration;

public class Tenant
{
    private readonly TenantModel _model;
    public Tenant(TenantModel model) 
    {
        _model = model;
    }
    public string Id { get => _model.Id; set => _model.Id = value; }
    public string Name { get => _model.Name; }
    public string ScimEndpoint { get => _model.ScimEndpoint; }
    public bool EnableLocalLogin { get => _model.EnableLocalLogin; }
    public IReadOnlyList<Client> Clients { get; }
    public IReadOnlyList<IAuthProvider> Providers { get => _model.Providers.ToList(); }
    public IReadOnlyList<string> AdministrativeUsers { get => _model.AdministrativeUsers.ToList();  }
    public IReadOnlyList<string> AdministrativeGroups { get => _model.AdministrativeGroups.ToList(); }
    public IReadOnlyList<ApiResource> ApiResources { get => _model.ApiResources.ToList(); }
    public IReadOnlyList<string> ManuallyManagedGroups { get => _model.ManuallyManagedGroups.ToList(); }
    internal TenantModel ToModel() => _model;
}
