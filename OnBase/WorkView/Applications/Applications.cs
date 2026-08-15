
namespace HyRest.OnBase.WorkView;

public class Applications : OnBaseItemTypeCollectionService<OnBaseWorkView, Application>
{
    public Applications(OnBaseWorkView module) : base(module)
    {

    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetApplications(token);
        col?.Items
                .Select(i => new Application(Module, i))
                .ToList()
                .ForEach(i => Add(i));
    }
    protected override async Task<Application?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetApplication(id, token);
        if (model != null)
            return new Application(Module, model);
        return null;
    }
}
