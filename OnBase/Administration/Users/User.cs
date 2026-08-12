namespace HyRest.Administration;

public class User : OnBaseItemService<OnBaseAdministration, UserModel>
{
    internal User(OnBaseAdministration module, UserModel user) : base(module, user)
    {

    }
    public override string? Name
    {
        get
        {
            if (Item.Name == null)
                GetDetailedModel().Wait(Module.App.ClientOptions.RequestTimeOut);
            return Item.Name;
        }
    }
    public string? RealName
    {
        get
        {
            if (Item.RealName == null)
                GetDetailedModel().Wait(Module.App.ClientOptions.RequestTimeOut);
            return Item.RealName;
        }
    }
    public string? EmailAddress
    {
        get
        {
            if (Item.EmailAddress == null)
                GetDetailedModel().Wait(Module.App.ClientOptions.RequestTimeOut);
            return Item.EmailAddress;
        }
    }
    public bool Active => Item.Deactivated ? true : false;

    private async Task GetDetailedModel()
    {
        var userModel = await Module.Run<IOnBaseAdministrationAPI, UserModel>((api, ct) => api.UsersGet2(Item.Id.ToString()));
        base.ReplaceModel(userModel);
    }
}