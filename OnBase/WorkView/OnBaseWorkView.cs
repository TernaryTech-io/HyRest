using HyRest.OnBase.ApiServices;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.WorkView;

public sealed partial class OnBaseWorkView : OnBaseModule<OnBaseWorkViewService>, IOnBaseWorkView
{
    public new ILogger<IOnBaseWorkView> Logger => (ILogger<IOnBaseWorkView>)base.Logger;
    public OnBaseWorkView(IOnBaseApp app, OnBaseWorkViewService service, ILogger<OnBaseWorkView> logger) : base(app, service, logger)
    {
        Applications = new Applications(this);
        Classes = new Classes(this);
        Attributes = new Attributes(this);
    }

    public Applications Applications { get; }
    public Classes Classes { get; }
    public Attributes Attributes { get; }

    //internal static OnBaseWorkView Create(IOnBaseApp app)
    //    => new OnBaseWorkView(app);
}
