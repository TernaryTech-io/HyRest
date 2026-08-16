using System.Text.Json.Serialization;
using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public class SingleInstanceKeywordGroup : KeywordGroup
{
    internal SingleInstanceKeywordGroup(OnBaseCore core, KeywordGroupModel group) : base(core, group) { }
    [HyRestConverter<JsonStringEnumConverter>]
    public override KeywordTypeGroupType GroupType => KeywordTypeGroupType.SingleInstance;
    public override string Name => KeywordTypeGroup?.Name ?? string.Empty;
    public override string SystemName => KeywordTypeGroup?.SystemName ?? string.Empty;
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}

public class SingleInstanceGroupCollection
{
    private readonly List<SingleInstanceKeywordGroup> _groups;
    internal SingleInstanceGroupCollection(List<SingleInstanceKeywordGroup> keywordGroups)
    {        
        _groups = keywordGroups;
    }
    public SingleInstanceKeywordGroup? this[string name] => _groups.FirstOrDefault(k => k.Name == name || k.SystemName == name);
    public SingleInstanceKeywordGroup? this[long id] => _groups.FirstOrDefault(k => k.Id == id);
    public SingleInstanceKeywordGroup? GetRecordWithValue(string keywordName, object value)
        => _groups.FirstOrDefault(g => g[keywordName]?[value] != null);
    public SingleInstanceKeywordGroup? GetRecordWithValue(long keywordId, object value)
        => _groups.FirstOrDefault(g => g[keywordId]?[value] != null);
    public List<SingleInstanceKeywordGroup> ToList() => _groups.ToList();
    public IReadOnlyCollection<SingleInstanceKeywordGroup> GroupRecords => _groups;
    internal List<KeywordGroupModel> GetModel()
     => _groups.Select(g => g.GetModel()).ToList();
    public string? ToJson()
        => JsonUtility.Serialize(this);
}