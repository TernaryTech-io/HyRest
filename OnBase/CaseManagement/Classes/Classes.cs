namespace HyRest.CaseManagement;

public class Classes : OnBaseItemTypeCollectionService<OnBaseWorkView, Class>
{
    public Classes(OnBaseWorkView module) : base(module)
    {
        
    }

    protected override async Task GetCollection(CancellationToken token = default)
    {
        Parallel.ForEach(Module.Applications.ToList(), (a, t) =>
        {
            _items.AddRange(a.Classes);
        });
        base.GetCollection(token);
    }
    protected override async Task<Class?> GetOne(long id, CancellationToken token = default)
    {
        var item = await Module.App.Cache.GetOrCreateAsync<Class>(id, null, token);
        if (item != null)
            return item;
        var model = await Module.Run(Module.Api.ClassesGet2(id.ToString(), Module.App.ClientOptions.DefaultLanguage));
        if (model != null)
        {
            item = new Class(Module, model);
            Module.App.Cache.SetAsync(item, token);
        }
        return null;
    }
}
