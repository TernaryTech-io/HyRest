namespace HyRest.CaseManagement;

public class FilterTypes : OnBaseItemTypeCollectionService<OnBaseWorkView, FilterType>
{
    public FilterTypes(OnBaseWorkView module) : base(module)
    {

    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        Module.Classes.ToList()
            .ForEach(async c =>
            {
                var filterTypes = await Module.Run(Module.Api.FiltersGet(c.Id.ToString(), Module.App.ClientOptions.DefaultLanguage), token);
                _items.AddRange(filterTypes.Items.Select(f => new FilterType(Module, f)));
            });
        base.GetCollection(token);
    }
    protected override async Task<FilterType?> GetOne(long id, CancellationToken token = default)
    {
        var item = Module.App.Cache.GetOrCreate<FilterType>(id, null, token);
        if (item != null)
            return item;
        var one = await Module.Run(Module.Api.FiltersGet2(id.ToString(), Module.App.ClientOptions.DefaultLanguage));
        if (one != null)
        {
            item = new FilterType(Module, one);            
            Module.App.Cache.SetAsync(item, token);
            return item;
        }
        return null;
    }
}