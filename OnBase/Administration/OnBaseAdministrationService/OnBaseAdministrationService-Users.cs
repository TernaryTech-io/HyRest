using HyRest.OnBase.ApiServices;

namespace HyRest.OnBase.Administration;

public partial class OnBaseAdministrationService : OnBaseService<IOnBaseAdministrationAPI>, IOnBaseAdministrationService
{
    private Task<UserCollectionModel?> _getUsers(CancellationToken token = default)
        => Run(Api.UsersGet(), token);
    private Task<UserCollectionModel?> _getUsers(IEnumerable<string>? ids = null, bool? serviceAccount = null, int? limit = null, 
        string? lastValue = null, bool? descendingOrder = true, CancellationToken token = default)
        => Run(Api.UsersGet(ids,serviceAccount,limit,lastValue,descendingOrder), token);
    private Task<UserModel?> _getUser(string id, CancellationToken token = default)
        => Run(Api.UsersGet2(id), token);
    public async Task<UserCollectionModel?> GetUsers(CancellationToken token = default)
    {
        var col = await _getUsers(token);
        if (col != null)
        {
            col.Items.ToList().ForEach(async i =>
            {
                await Cache.SetAsync(i, token);
            });
            return col;
        }
        else
            return null;
    }
    public async Task<UserModel?> GetUser(string identifier, CancellationToken token = default)
    {
        UserModel? item = null;
        if (Cache.TryGetValue(identifier, out item))
            return item;

        if (long.TryParse(identifier, out long id))
            item = await _getUser(identifier, token);
        else
        {
            var col = await _getUsers([identifier], null,null,null,null, token);
            if (col != null)
                item = col.Items.FirstOrDefault();
        }
        if (item != null)
            await Cache.SetAsync(item);
        return item;
    }
}