
namespace HyRest.CaseManagement;

public class Applications : OnBaseItemTypeCollectionService<OnBaseWorkView, Application>
{
    public Applications(OnBaseWorkView module) : base(module)
    {

    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Run(Module.Api.Applications(Module.App.ClientOptions.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new Application(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
        base.GetCollection(token);
    }    
}
