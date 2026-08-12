using HyRest.Utilities;


namespace HyRest.DocumentManagement;

public class KeywordTypes : OnBaseItemTypeCollectionService<OnBaseCore, KeywordType>
{
    internal KeywordTypes(OnBaseCore core) : base(core)
    {
       
    }
    protected override async Task GetCollection(CancellationToken token)
    {
        var col = await Module.Run(Module.Api.GetKeywordTypeCollection(null, null, Options.DefaultLanguage),token);
        if (col != null)
        {
            col.Items
                .Select(i => new KeywordType(Module, i))
                .ToList()
                .ForEach(i => Add(i)); 
        }
        base.GetCollection(token);
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
