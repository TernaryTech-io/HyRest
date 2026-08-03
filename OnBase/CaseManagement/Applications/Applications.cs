
namespace HyRest.CaseManagement;

public class Applications : OnBaseItemTypeCollectionService<IOnBaseWorkViewAPI, OnBaseWorkView, Application>
{
    public Applications(OnBaseWorkView module) : base(module)
    {

    }

    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.Applications(Module.App.ClientOptions.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new Application(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
    }
}
