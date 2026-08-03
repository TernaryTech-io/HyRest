namespace HyRest.CaseManagement;

public sealed partial class OnBaseWorkView : OnBaseModule<IOnBaseWorkViewAPI>, IOnBaseWorkView
{
    internal OnBaseWorkView(IOnBaseApp app) : base(app)
    {
        Applications = new Applications(this);
        Classes = new Classes(this);
        Attributes = new Attributes(this);
    }

    public Applications Applications { get; }
    public Classes Classes { get; }
    public Attributes Attributes { get; }

    internal static OnBaseWorkView Create(IOnBaseApp app)
        => new OnBaseWorkView(app);
}
