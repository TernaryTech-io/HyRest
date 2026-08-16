namespace HyRest.OnBase.WorkView;

public class Classes : OnBaseItemTypeCollectionService<OnBaseWorkView, Class>
{
    public Classes(OnBaseWorkView module) : base(module)
    {
        
    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        foreach(var app in Module.Applications.ToList())
        {
            var col = await Module.Service.GetClasses(app.Id.ToString(), token);
            col?.Items
                    .Select(i => new Class(Module, i))
                    .ToList()
                    .ForEach(i => Add(i));
        }        
    }
    protected override async Task<Class?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetClass(id, token);
        if (model != null)
            return new Class(Module, model);
        return null;
    }
}
