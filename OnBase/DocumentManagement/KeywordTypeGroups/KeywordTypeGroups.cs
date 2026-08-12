

using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class KeywordTypeGroups : OnBaseItemTypeCollectionService< OnBaseCore, KeywordTypeGroup>
{    
    internal KeywordTypeGroups(OnBaseCore core) : base(core)
    {
        
    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Run(Module.Api.GetKeywordTypeGroupCollection(null, null, Options.DefaultLanguage), token);
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
        base.GetCollection(token);
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}