using System.Text.Json;

namespace HyRest.Hyland.IdentityAdministration;

public class IdentityAdmin : IdentityAdminBase<IdentityAdminService>, IIdentityAdminModule
{
    private Tenant? _tenant { get; set; }
    public IdentityAdmin(HylandIdentity app, IdentityAdminService service) : base(app, service)
    {
        GetFirstTenant().Wait();
    }
    public HylandIdentity App { get => (HylandIdentity)base.App; }
    internal Tenant? Tenant
    {
        get
        {
            if (_tenant == null)
                GetFirstTenant().Wait();
            return _tenant;
        }
    }
    public IReadOnlyList<Client> Clients => _tenant?.Clients ?? [];
    public IReadOnlyList<IAuthProvider> Providers => _tenant?.Providers ?? [];
    public IReadOnlyList<string> AdministrativeUsers => _tenant?.AdministrativeUsers ?? []; 
    public IReadOnlyList<string> AdministrativeGroups  => _tenant?.AdministrativeGroups ?? []; 
    public IReadOnlyList<ApiResource> ApiResources => _tenant?.ApiResources ?? []; 
    public IReadOnlyList<string> ManuallyManagedGroups => _tenant?.ManuallyManagedGroups ?? []; 
    public ModifyClientProperties CreateModifyClientProperties(Client client)
        => new ModifyClientProperties(this, client);
    public NewClientProperties CreateNewClientProperties(CreateModifyClient? model = null)
        => new NewClientProperties(this, model);
    public async Task<Client> AddNewClient(NewClientProperties props)
    {
        if (_tenant == null)
            throw new Exception("No Tenant is loaded.");
        var model = props.ToModel();
        var json = JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true});
        Console.WriteLine(json);
        var client = Service.CreateClientAsync(_tenant.Id, model);
        client.Wait(TimeSpan.FromSeconds(App.ClientOptions.RequestTimeOut));
        if (client.IsFaulted || client.IsCanceled)
            throw client.Exception ?? new Exception("Failed to create client");
        return client.Result;
    }
    public async Task<Tenant> AddOrModifyTenant(CreateModifyTenant tenant)
    {
        string? id = Tenant?.Id;
        TenantModel? tenantModel = null;
        if (id == null)
            tenantModel = await Service.CreateTenantAsync(tenant);
        else
            tenantModel = await Service.UpdateTenantAsync(id, tenant);
        if (tenantModel == null)
            throw new Exception("Failed to update or create tenant.");
        return new Tenant(tenantModel);
    }

    private async Task GetFirstTenant()
    {
        var list = await Service.ListTenantsAsync();
        var first = list.FirstOrDefault();
        if (first == null)
            return;
        var tenant = await Service.GetTenantAsync(first.Id);
        _tenant = new Tenant(tenant);
    }
}
