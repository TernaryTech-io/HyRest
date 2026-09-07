namespace HyRest.Hyland.IdentityAdministration;

public class ModifyTenantProperties : NewTenantProperties
{
    private readonly CreateModifyTenant _tenant;
    public readonly string TenantId;
    public ModifyTenantProperties(Tenant tenant) : base(tenant.ToModel())
    {
        TenantId = tenant.Id;
        _tenant = tenant.ToModel();
    }
}

public class NewTenantProperties
{
    private readonly CreateModifyTenant _tenant;
    public NewTenantProperties(CreateModifyTenant? tenant = null)
    {        
        _tenant = tenant ?? new CreateModifyTenant();
    }
    public string Name { get => _tenant.Name; set => _tenant.Name = value; }
    public string ScimEndpoint { get => _tenant.ScimEndpoint; set => _tenant.ScimEndpoint = value; }
    public bool EnableLocalLogin { get => _tenant.EnableLocalLogin; set => _tenant.EnableLocalLogin = value; }
    public ConnectionSettings Connection { get => _tenant.Connection; set => _tenant.Connection = value; }
    public ICollection<string> AdministrativeUsers { get => _tenant.AdministrativeUsers; set => _tenant.AdministrativeUsers = value; }
    public ICollection<string> AdministrativeGroups { get => _tenant.AdministrativeGroups; set => _tenant.AdministrativeGroups = value; }
    public ICollection<ApiResource> ApiResources { get => _tenant.ApiResources; set => _tenant.ApiResources = value;  }
    // TO DO: Add Providers
    internal CreateModifyTenant ToModel() => _tenant;
}