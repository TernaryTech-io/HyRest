namespace HyRest.CaseManagement;

public class FilterTypes : OnBaseItemTypeCollectionService<IOnBaseWorkViewAPI, OnBaseWorkView, FilterType>
{
    public FilterTypes(OnBaseWorkView module) : base(module)
    {

    }
    protected override async Task GetCollection()
    {
        foreach (var cls in Module.Classes.ToList())
        {
            var filterTypes = await Module.Run(Api.FiltersGet(cls.Id.ToString(), Module.App.ClientOptions.DefaultLanguage));
            _items.AddRange(filterTypes.Items.Select(f => new FilterType(Module, f)));
        }        
    }
}