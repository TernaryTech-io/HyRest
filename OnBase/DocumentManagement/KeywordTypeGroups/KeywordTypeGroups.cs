

using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class KeywordTypeGroups : OnBaseItemTypeCollectionService<IOnBaseDocumentAPI, OnBaseCore, KeywordTypeGroup>
{    
    internal KeywordTypeGroups(OnBaseCore core) : base(core)
    {
        
    }
    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.GetKeywordTypeGroupCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .ToList()
                .ForEach(i =>
                {
                    if (i.StorageType == KeywordTypeGroupStorageType.MultiInstance)
                        Add(new MultiInstanceKeywordTypeGroup(Module, i));
                    else if (i.StorageType == KeywordTypeGroupStorageType.SingleInstance)
                        Add(new SingleInstanceKeywordTypeGroup(Module, i));
                });
        }
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}