using HyRest.Utilities;

namespace HyRest.DocumentManagement;
public sealed class CustomQueries : OnBaseItemTypeCollectionService<IOnBaseDocumentAPI, OnBaseCore, CustomQuery>
{

    internal CustomQueries(OnBaseCore core) : base(core) { }
    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.GetCustomQueryCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new CustomQuery(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
