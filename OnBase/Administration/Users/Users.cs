using System.Text;

namespace HyRest.Administration;

public class Users : OnBaseItemCollectionService<IOnBaseAdministrationAPI,OnBaseAdministration, User>
{
    internal Users(OnBaseAdministration module) : base(module)
    {

    }

    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.UsersGet());
        if (col != null)
        {
            col.Items
                .Select(i => new User(Module, i))
                .ToList()
                .ForEach(i => _items.Add(i));
        }
    }
}

public class User : OnBaseItemService<IOnBaseAdministrationAPI, OnBaseAdministration, UserModel>
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