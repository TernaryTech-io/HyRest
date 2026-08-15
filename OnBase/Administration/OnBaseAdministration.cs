using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.Administration;

public class OnBaseAdministration : OnBaseModule<OnBaseAdministrationService>, IOnBaseAdministration
{
    public ILogger<IOnBaseAdministration> Logger => (ILogger<IOnBaseAdministration>)base.Logger;
    public OnBaseAdministration(IOnBaseApp app, OnBaseAdministrationService service, ILogger<OnBaseAdministration> logger) : base(app, service,logger)
    {
        Users = new Users(this);
    }
    public Users Users { get; }
    //public static OnBaseAdministration Create(IOnBaseApp app)
    //    => new OnBaseAdministration(app);
}

