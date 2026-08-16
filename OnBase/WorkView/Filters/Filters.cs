namespace HyRest.OnBase.WorkView;

public class Filters : OnBaseItemTypeCollectionService<OnBaseWorkView, Filter>
{
    public Filters(OnBaseWorkView module) : base(module)
    {

    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        foreach (var cls in Module.Classes.ToList())
        {
            var col = await Module.Service.GetFilters(cls.Id.ToString(), token);
            col?.Items
                    .Select(i => new Filter(Module, i))
                    .ToList()
                    .ForEach(i => Add(i));
        }
    }
    protected override async Task<Filter?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetFilter(id, token);
        if (model != null)
            return new Filter(Module, model);
        return null;
    }
}