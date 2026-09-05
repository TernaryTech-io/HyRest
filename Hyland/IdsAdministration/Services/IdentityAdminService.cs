using Microsoft.Extensions.Logging;

namespace HyRest.Hyland.IdentityAdministration;

public class IdentityAdminService : HylandService<IHylandIdentityServiceAdministrationAPI>, IIdentityAdminService
{
    private const string DefaultVersion = "1";

    public IdentityAdminService(IdentityAdminClientFactory hylandClientFactory, ILogger<IdentityAdminService> logger)
        : base(hylandClientFactory, logger)
    {

    }
    public Task<ICollection<string>?> ListAdminUsersAsync(string tenantId, string version = DefaultVersion)
        => Run(Api.AdminsGet(tenantId, version));

    public Task AddAdminUserAsync(string tenantId, AdministrativeUserRequestModel body, string version = DefaultVersion)
        => Run(Api.AdminsPost(tenantId, version, body));

    public Task RemoveAdminUserAsync(string tenantId, string username, string version = DefaultVersion)
        => Run(Api.AdminsDelete(tenantId, username, version));

    public Task<ICollection<ApiResource>> ListApiResourcesAsync(string tenantId, string version = DefaultVersion)
        => Run(Api.ApiresourcesGet(tenantId, version));

    public Task<ApiResource?> CreateApiResourceAsync(string tenantId, CreateModifyApiResource body, string version = DefaultVersion)
        => Run(Api.ApiresourcesPost(tenantId, version, body));

    public Task GetApiResourceAsync(string tenantId, string apiResourceId, string version = DefaultVersion)
        => Run(Api.ApiresourcesGet2(tenantId, apiResourceId, version));

    public Task<ApiResource> UpdateApiResourceAsync(string tenantId, string apiResourceId, CreateModifyApiResource body, string version = DefaultVersion)
        => Run(Api.ApiresourcesPut(tenantId, apiResourceId, version, body));

    public Task DeleteApiResourceAsync(string tenantId, string apiResourceId, string version = DefaultVersion)
        => Run(Api.ApiresourcesDelete(tenantId, apiResourceId, version));

    public Task<ICollection<IdNamePair>?> ListClientsAsync(string tenantId, string version = DefaultVersion)
        => Run(Api.ClientsGet(tenantId, version));

    public Task<Client?> CreateClientAsync(string tenantId, CreateModifyClient body, string version = DefaultVersion)
        => Run(Api.ClientsPost(tenantId, version, body));

    public Task GetClientAsync(string tenantId, string clientId, string version = DefaultVersion)
        => Run(Api.ClientsGet2(tenantId, clientId, version));

    public Task<Client?> UpdateClientAsync(string tenantId, string clientId, CreateModifyClient body, string version = DefaultVersion)
        => Run(Api.ClientsPut(tenantId, clientId, version, body));

    public Task DeleteClientAsync(string tenantId, string clientId, string version = DefaultVersion)
        => Run(Api.ClientsDelete(tenantId, clientId, version));

    public Task<ClientSecret?> CreateClientSecretAsync(string tenantId, ClientSecret body, string version = DefaultVersion)
        => Run(Api.Secret(version, tenantId, body));
    public Task<ICollection<IdNamePair>?> ListProvidersAsync(string tenantId, string version = DefaultVersion)
        => Run(Api.ProvidersGet(tenantId, version));

    public Task<object?> CreateProviderAsync(string tenantId, object body, string version = DefaultVersion)
        => Run(Api.ProvidersPost(tenantId, version, body));

    public Task<object?> GetProviderAsync(string tenantId, string providerId, string version = DefaultVersion)
        => Run(Api.ProvidersGet2(tenantId, providerId, version));

    public Task<object?> UpdateProviderAsync(string tenantId, string providerId, object body, string version = DefaultVersion)
        => Run(Api.ProvidersPut(tenantId, providerId, version, body));

    public Task DeleteProviderAsync(string tenantId, string providerId, string version = DefaultVersion)
        => Run(Api.ProvidersDelete(tenantId, providerId, version));
    public Task<ICollection<IdNamePair>?> ListTenantsAsync(string version = DefaultVersion)
        => Run(Api.TenantsGet(version));

    public Task<TenantModel?> GetTenantAsync(string tenantId, string version = DefaultVersion)
        => Run(Api.TenantsGet2(tenantId, version));

    public Task<TenantModel?> CreateTenantAsync(CreateModifyTenant body, string version = DefaultVersion)
        => Run(Api.TenantsPost(version, body));

    public Task<TenantModel?> UpdateTenantAsync(string tenantId, CreateModifyTenant body, string version = DefaultVersion)
        => Run(Api.TenantsPut(tenantId, version, body));

    public Task DeleteTenantAsync(string tenantId, string version = DefaultVersion)
        => Run(Api.TenantsDelete(tenantId, version));
}