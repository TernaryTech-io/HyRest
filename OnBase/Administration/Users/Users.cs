namespace HyRest.OnBase.Administration;

public class Users : OnBaseItemTypeCollectionService<OnBaseAdministration, User>
{   
    internal Users(OnBaseAdministration module) : base(module)
    {

    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetUsers(token);
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
    protected override async Task<User?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetUser(id);
        if (model != null)
            return new User(Module, model);
        return null;
    }
}

