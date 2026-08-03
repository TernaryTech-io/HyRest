using System.Text.Json.Serialization;
using Ternary.DataConversions.Extensions;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class MultiInstanceKeywordGroup : KeywordGroup
{
    internal MultiInstanceKeywordGroup(OnBaseCore core, KeywordGroupModel group) : base(core, group)
    { }
    public override string Name => KeywordTypeGroup?.Name ?? string.Empty;
    public override string SystemName => KeywordTypeGroup?.SystemName ?? string.Empty;
    public string? InstanceId => Item.InstanceId;
    public long GroupId => Item.GroupId != null ? Item.GroupId.ConvertTo<long>() : -1;
    [HyRestConverter<JsonStringEnumConverter>]
    public override KeywordTypeGroupType GroupType => KeywordTypeGroupType.MultiInstance;
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
public class MultiInstanceGroupCollection
{
    private readonly List<MultiInstanceKeywordGroup> _groups;
    internal MultiInstanceGroupCollection(List<MultiInstanceKeywordGroup> keywordGroups)
    {
        if (keywordGroups.DistinctBy(g => g.Id).Count() > 1)
            throw new Exception("All KeywordGroups added to this object should be of the same type.");
        _groups = keywordGroups;
    }
    public long Id => _groups.First().Id;
    public string? Name => _groups.First().Name;
    public string? SystemName => _groups.First().SystemName;
    public MultiInstanceKeywordGroup? this[int index] => _groups[index];
    public MultiInstanceKeywordGroup? this[string instanceId] => _groups.FirstOrDefault(k => k.InstanceId == instanceId);
    public MultiInstanceKeywordGroup? GetRecordWithValue(string keywordName, object value)
        => _groups.FirstOrDefault(g => g[keywordName]?[value] != null);
    public MultiInstanceKeywordGroup? GetRecordWithValue(long keywordId, object value)
        => _groups.FirstOrDefault(g => g[keywordId]?[value] != null);
    public List<MultiInstanceKeywordGroup> ToList() => _groups.ToList();
    public MultiInstanceKeywordGroup[] ToArray() => _groups.ToArray();
    public IReadOnlyCollection<MultiInstanceKeywordGroup> GroupRecords => _groups;
    [JsonIgnore]
    public int Count => _groups.Count;
    internal List<KeywordGroupModel> GetModel()
     => _groups.Select(g => g.GetModel()).ToList();
    public string? ToJson()
        => JsonUtility.Serialize(this);
}

public class SortedMultiInstanceCollections
{
    private List<MultiInstanceGroupCollection> _sortedList { get; set; } = [];
    internal SortedMultiInstanceCollections(List<MultiInstanceKeywordGroup> mikgs)
    {
        var distGroups = mikgs.DistinctBy(m => m.Id).Select(i => i.Id).ToList();
        distGroups.ForEach(i =>
        {
            var list = mikgs.Where(g => g.Id == i).ToList();
            _sortedList.Add(new MultiInstanceGroupCollection(list));
        });
    }
    public MultiInstanceGroupCollection? this[long id] => _sortedList.FirstOrDefault(i => i.Id == id);
    public MultiInstanceGroupCollection? this[string name] => _sortedList.FirstOrDefault(i => i.Name == name || i.SystemName == name);
    public MultiInstanceGroupCollection? Find(string identifier) 
        => _sortedList.FirstOrDefault(i => i.Name == identifier || i.SystemName == identifier || i.Id.ToString() == identifier);
    public IReadOnlyCollection<MultiInstanceGroupCollection> GroupCollection => _sortedList;
    [JsonIgnore]
    public int Count => _sortedList.Count;
    public string? ToJson()
        => JsonUtility.Serialize(this);
}