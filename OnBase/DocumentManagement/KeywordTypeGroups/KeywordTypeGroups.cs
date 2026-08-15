using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public class KeywordTypeGroups : OnBaseItemTypeCollectionService< OnBaseCore, KeywordTypeGroup>
{    
    internal KeywordTypeGroups(OnBaseCore core) : base(core)
    {
        
    }

    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetKeywordTypeGroups(token);
        col?.Items
                .ToList()
                .ForEach(i =>
                {
                    if (i.StorageType == KeywordTypeGroupStorageType.MultiInstance)
                        Add(new MultiInstanceKeywordTypeGroup(Module, i));
                    else if (i.StorageType == KeywordTypeGroupStorageType.SingleInstance)
                        Add(new SingleInstanceKeywordTypeGroup(Module, i));
                });
    }
    protected override async Task<KeywordTypeGroup?> GetOne(string id, CancellationToken token = default)
    {
        var i = await Module.Service.GetKeywordTypeGroup(id, token);
        if (i != null)
        {
            if (i.StorageType == KeywordTypeGroupStorageType.MultiInstance)
                Add(new MultiInstanceKeywordTypeGroup(Module, i));
            else if (i.StorageType == KeywordTypeGroupStorageType.SingleInstance)
                Add(new SingleInstanceKeywordTypeGroup(Module, i));
        }
        return null;
    }    
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}