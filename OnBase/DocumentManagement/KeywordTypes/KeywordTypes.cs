using HyRest.Utilities;


namespace HyRest.DocumentManagement;

public class KeywordTypes : OnBaseItemTypeCollectionService<IOnBaseDocumentAPI, OnBaseCore, KeywordType>
{
    internal KeywordTypes(OnBaseCore core) : base(core)
    {
       
    }
    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.GetKeywordTypeCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new KeywordType(Module, i))
                .ToList()
                .ForEach(i => Add(i)); 
        }        
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
