using HyRest.Cache;
using System.Text;
using Ternary.DataConversions.Extensions;

namespace HyRest.Administration;

public class Users : OnBaseItemCollectionService<OnBaseAdministration, User>
{   
    internal Users(OnBaseAdministration module) : base(module)
    {

    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Run(Module.Api.UsersGet(), token);            
        if (col != null)
        {
            col.Items
                .Select(i => new User(Module, i))
                .ToList()
                .ForEach(i =>
                {
                    _items.Add(i);
                });
        }
    }
    protected override async Task<User?> GetOne(long id, CancellationToken token = default)
    {
        var item = await Module.App.Cache.GetOrCreateAsync<User>(id, null, token);
        if (item != null)
            return item;
        var model = await Module.Run(Module.Api.UsersGet2(id.ToString()), token);
        if (model != null)
        {
            var user = new User(Module, model);
            Module.App.Cache.SetAsync(user, token);
            return user;
        }
        return null;
    }
}

