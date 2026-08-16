namespace HyRest.OnBase.ApiServices;

public partial class OnBaseWorkViewService : OnBaseService<IOnBaseWorkViewAPI>, IOnBaseWorkViewService
{
    private Task<FilterCollectionModel?> _getFilters(string classId, CancellationToken token = default)
        => Run(Api.FiltersGet(classId, Options.DefaultLanguage), token);
    private Task<FilterModel?> _getFilter(string id, CancellationToken token = default)
        => Run(Api.FiltersGet2(id, Options.DefaultLanguage), token);
    public async Task<FilterCollectionModel?> GetFilters(string classId, CancellationToken token = default)
    {
        var col = await _getFilters(classId, token);
        if (col != null)
        {
            col.Items.ToList().ForEach(async i =>
            {
                await Cache.SetAsync(i, token, CachePrefix);
            });
            return col;
        }
        else
            return null;
    }
    public async Task<FilterModel?> GetFilter(string id, CancellationToken token = default)
    {
        var model = await _getFilter(id, token);
        if (model != null)
        {
            await Cache.SetAsync(model, token, CachePrefix);
            return model;
        }
        return null;
    }
}