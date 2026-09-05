using Microsoft.Extensions.Logging;

namespace HyRest.Hyland.IdentityAdministration;

public class HylandIdentity : HylandIdentityBase
{
    public HylandIdentity(ILogger<HylandIdentity> logger, IdentityAdminClientFactory clientFactory, IdentityAdminService identityService) 
        : base(logger, clientFactory)
    {
        ClientFactory.AuthClient.AuthenticateAsync().Wait();
        Administration = new IdentityAdmin(this, identityService);
    }
    public IdentityAdmin Administration { get; protected set; }
    public override IdentityAdminClientFactory ClientFactory => (IdentityAdminClientFactory)base.ClientFactory;
    public override HylandClientOptions ClientOptions => (HylandClientOptions)base.ClientOptions;
    public override bool IsConnected => ClientFactory.AuthClient.IsAuthenticated;
}
