using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.Administration;

public class OnBaseAdministration : OnBaseModule<OnBaseAdministrationService>, IOnBaseAdministration
{
    internal OnBaseAdministration(OnBaseApp app, OnBaseAdministrationService service) : base(app, service)
    {
        Users = new Users(this);
    }
    public Users Users { get; }
}

