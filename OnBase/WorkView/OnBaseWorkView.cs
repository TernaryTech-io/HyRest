using HyRest.OnBase.ApiServices;

namespace HyRest.OnBase.WorkView;

public sealed partial class OnBaseWorkView : OnBaseModule<OnBaseWorkViewService>, IOnBaseWorkView
{
    internal OnBaseWorkView(OnBaseApp app, OnBaseWorkViewService service) : base(app, service)
    {
        Applications = new Applications(this);
        Classes = new Classes(this);
        Attributes = new Attributes(this);
        Filters = new Filters(this);
    }
    public Applications Applications { get; }
    public Classes Classes { get; }
    public Attributes Attributes { get; }
    public Filters Filters { get; set; }
}
