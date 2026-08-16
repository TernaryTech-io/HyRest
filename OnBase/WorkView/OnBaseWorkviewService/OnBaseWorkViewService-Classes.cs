namespace HyRest.OnBase.ApiServices;

public partial class OnBaseWorkViewService : OnBaseService<IOnBaseWorkViewAPI>, IOnBaseWorkViewService
{
    private Task<ClassCollectionModel?> _getClasses(string appId, CancellationToken token = default)
        => Run(Api.ClassesGet(appId, Options.DefaultLanguage), token);
    private Task<ClassModel?> _getClass(string id, CancellationToken token = default)
        => Run(Api.ClassesGet2(id, Options.DefaultLanguage), token);
    public async Task<ClassCollectionModel?> GetClasses(string appId, CancellationToken token = default)
    {
        var col = await _getClasses(appId, token);
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
    public async Task<ClassModel?> GetClass(string id, CancellationToken token = default)
    {
        var model = await _getClass(id, token);
        if (model != null)
        {
            await Cache.SetAsync(model, token, CachePrefix);
            return model;
        }
        return null;
    }
    public Task<ClassAccessRights?> GetAccessRights(string id, CancellationToken token = default)
        => Run(Api.AccessRights(id), token);
}