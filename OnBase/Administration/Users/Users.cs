using HyRest.Cache;
using System.Text;

namespace HyRest.Administration;

public class Users : OnBaseItemCollectionService<OnBaseAdministration, User>
{
    internal Users(OnBaseAdministration module, OnBaseAppCache<User> cache) : base(module, cache)
    {

    }

    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Run<IOnBaseAdministrationAPI, UserCollectionModel>((api,ct) => api.UsersGet());
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
    protected override async Task<User?> GetOne(string identifier, CancellationToken token = default)
    {
        if (long.TryParse(identifier, out long id))
        {
            var model = await Module.Run<IOnBaseAdministrationAPI, UserModel>((api, ct) => api.UsersGet2(identifier));
            if (model != null)
                return new User(Module, model);
        }
    }
}

public class User : OnBaseItemService<OnBaseAdministration, UserModel>
{
    private bool _hydrated;
    internal User(OnBaseAdministration module, UserModel user) : base (module, user)
    {

    }
    public string? Name
    {
        get
        {
            if (Item.Name == null)
                PopulateDetails().Wait();
            return Item.Name;
        }
    }
    public string? RealName
    {
        get
        {
            if (Item.RealName == null)
                PopulateDetails().Wait();
            return Item.RealName;
        }
    }
    public string? EmailAddress
    {
        get
        {
            if (Item.EmailAddress == null)
                PopulateDetails().Wait();
            return Item.EmailAddress;
        }
    }
    public bool Active => Item.Deactivated ? true : false;

    internal async Task PopulateDetails()
    {
        var userModel = await Module.Run(Module.Api<IOnBaseAdministrationAPI>().UsersGet2(Item.Id, null));
        base.ReplaceModel(userModel);
    }
}