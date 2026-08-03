using HyRest.Utilities;

namespace HyRest.DocumentManagement;
public sealed class AutoFillKeywordSets : OnBaseItemTypeCollectionService<IOnBaseDocumentAPI, OnBaseCore, AutoFillKeywordSet>
{
    internal AutoFillKeywordSets(OnBaseCore core) : base(core){}
    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.GetAutofillKeywordSetCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new AutoFillKeywordSet(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
