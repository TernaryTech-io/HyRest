using Refit;

namespace HyRest.Administration;

public class OnBaseAdministration : OnBaseModule, IOnBaseAdministration
{
    internal OnBaseAdministration(IOnBaseApp app) : base(app)
    {
        Users = new Users(this);
    }
    public Users Users { get; }
    public static OnBaseAdministration Create(IOnBaseApp app)
        => new OnBaseAdministration(app);
}

