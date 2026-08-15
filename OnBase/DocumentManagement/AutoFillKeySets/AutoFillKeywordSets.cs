using HyRest.Utilities;

namespace HyRest.OnBase.Core;
public sealed class AutoFillKeywordSets : OnBaseItemTypeCollectionService<OnBaseCore, AutoFillKeywordSet>
{
    internal AutoFillKeywordSets(OnBaseCore core) : base(core){}
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetAutoFillKeywordSets(token);
        col?.Items
                .Select(i => new AutoFillKeywordSet(Module, i))
                .ToList()
                .ForEach(i => Add(i));
    }
    protected override async Task<AutoFillKeywordSet?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetAutoFillKeywordSet(id, token);
        if(model != null)
            return new AutoFillKeywordSet(Module, model);
        return null;               
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
