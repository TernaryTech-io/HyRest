using HyRest.Utilities;


namespace HyRest.OnBase.Core;

public class KeywordTypes : OnBaseItemTypeCollectionService<OnBaseCore, KeywordType>
{
    internal KeywordTypes(OnBaseCore core) : base(core)
    {
       
    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetKeywordTypes(token);
        col?.Items
                .Select(i => new KeywordType(Module, i))
                .ToList()
                .ForEach(i => Add(i));
    }
    protected override async Task<KeywordType?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetKeywordType(id, token);
        if (model != null)
            return new KeywordType(Module, model);
        return null;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
