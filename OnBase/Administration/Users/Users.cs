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
    internal User(OnBaseAdministration module, UserModel user) : base (module, PopulateDetails(module,user))
    {

    }
    public string? Name => Item.Name;
    public string? RealName => Item.RealName;
    public string? EmailAddress => Item.EmailAddress;
    public bool Active => Item.Deactivated ? true : false;

    internal static UserModel PopulateDetails(OnBaseAdministration module, UserModel user)
    {
        var task = module.Run(module.Api<IOnBaseAdministrationAPI>().UsersGet2(user.Id, null));
        task.Wait();
        if (task.IsCompletedSuccessfully)
            return task.Result;
        else
            return user;
    }
}