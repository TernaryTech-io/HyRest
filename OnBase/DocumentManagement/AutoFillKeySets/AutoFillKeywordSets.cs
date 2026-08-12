using HyRest.Utilities;

namespace HyRest.DocumentManagement;
public sealed class AutoFillKeywordSets : OnBaseItemTypeCollectionService<OnBaseCore, AutoFillKeywordSet>
{
    internal AutoFillKeywordSets(OnBaseCore core) : base(core){}
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Run(Module.Api.GetAutofillKeywordSetCollection(null, null, Options.DefaultLanguage), token);
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
