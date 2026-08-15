namespace HyRest.OnBase.Administration;

public class User : OnBaseItemTypeService<OnBaseAdministration, UserModel>
{
    internal User(OnBaseAdministration module, UserModel user) : base(module, user)
    {

    }
    public override string? Name
    {
        get
        {
            if (Item.Name == null)
                GetDetailedModel().Wait(Module.App.RequestTimeOut);
            return Item.Name;
        }
    }
    public string? RealName
    {
        get
        {
            if (Item.RealName == null)
                GetDetailedModel().Wait(Module.App.RequestTimeOut);
            return Item.RealName;
        }
    }
    public string? EmailAddress
    {
        get
        {
            if (Item.EmailAddress == null)
                GetDetailedModel().Wait(Module.App.RequestTimeOut);
            return Item.EmailAddress;
        }
    }
    public bool Active => Item.Deactivated ? true : false;

    private async Task GetDetailedModel()
    {
        var userModel = await Module.Service.GetUser(Item.Id);
        ReplaceModel(userModel);
    }
}