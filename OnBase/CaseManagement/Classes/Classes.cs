namespace HyRest.CaseManagement;

public class Classes : OnBaseItemTypeCollectionService<IOnBaseWorkViewAPI, OnBaseWorkView, Class>
{
    public Classes(OnBaseWorkView module) : base(module)
    {

    }

    protected override Task GetCollection()
        => Task.Run(() => Parallel.ForEach(Module.Applications.ToList(), (a, t) =>
        {
            _items.AddRange(a.Classes);
        }));
}
