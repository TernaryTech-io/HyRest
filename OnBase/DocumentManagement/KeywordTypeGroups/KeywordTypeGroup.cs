using HyRest.Utilities;
using System.Text.Json.Serialization;

namespace HyRest.OnBase.Core;

public class StandAloneKeywordTypes : KeywordTypeGroup
{
    internal StandAloneKeywordTypes(OnBaseCore core, List<KeywordType> keywordTypes) : base(core, keywordTypes)
    {
        
    }    
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
public class MultiInstanceKeywordTypeGroup : KeywordTypeGroup
{
    internal MultiInstanceKeywordTypeGroup(OnBaseCore core, KeywordTypeGroupModel item) : base(core, item)
    {

    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
public class SingleInstanceKeywordTypeGroup : KeywordTypeGroup
{
    internal SingleInstanceKeywordTypeGroup(OnBaseCore core, KeywordTypeGroupModel item) : base(core, item)
    {

    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}

public abstract class KeywordTypeGroup: OnBaseItemTypeService<OnBaseCore, KeywordTypeGroupModel>, IKeywordTypeGroup
{
    private List<KeywordType> _keywordTypes { get; set; } = [];
    private KeywordTypeGroupType _groupType { get; set; }
    internal KeywordTypeGroup(OnBaseCore core, KeywordTypeGroupModel item) : base(core, item)
    {
        if (item.StorageType == KeywordTypeGroupStorageType.MultiInstance)
            _groupType = KeywordTypeGroupType.MultiInstance;
        else
            _groupType = KeywordTypeGroupType.SingleInstance;
    }
    internal KeywordTypeGroup(OnBaseCore core, List<KeywordType> keywordTypes) : base(core, null)
    {        
        if (keywordTypes != null)
            _keywordTypes = keywordTypes;
        _groupType = KeywordTypeGroupType.StandAlone;
    }
    [HyRestConverter<JsonStringEnumConverter>]
    public KeywordTypeGroupType StorageType => _groupType;  
    public KeywordType? this[long id] => _keywordTypes.FirstOrDefault(i => i.Id == id);
    public KeywordType? this[string identifier]
        => _keywordTypes.FirstOrDefault(i => i.Id.ToString() == identifier || i.Name == identifier || i.SystemName == identifier);
    public int Count => KeywordTypes.Count;
    public IReadOnlyCollection<KeywordType> KeywordTypes
    {
        get
        {
            if (StorageType != KeywordTypeGroupType.StandAlone)
                PopulateKeywordTypes().Wait(Module.App.ClientOptions.RequestTimeOut);
            return _keywordTypes;
        }
    }
    internal void Add(KeywordType kt) => _keywordTypes.Add(kt);

    private async Task PopulateKeywordTypes()
    {
        var col = await Module.Service.GetKeywordTypesForKeywordTypeGroup(Item.Id);
        if (col != null)
            col.Items
            .Select(i => Module.KeywordTypes.Find(i.Id))
            .ToList()
            .ForEach(i =>
            {
                if (i != null)
                    _keywordTypes.Add(i);
            });
    }
    public static KeywordTypeGroup Create(OnBaseCore core, KeywordTypeGroupModel item)
    {
        if (item.StorageType == KeywordTypeGroupStorageType.MultiInstance)
            return new MultiInstanceKeywordTypeGroup(core, item);
        else
            return new SingleInstanceKeywordTypeGroup(core, item);
    }
}
public interface IKeywordTypeGroup
{
    long Id { get; }
    string Name { get; }
    string SystemName { get; }
    KeywordTypeGroupType StorageType { get; }
    KeywordType? this[long id] { get; }
    KeywordType? this[string identifier] { get; }
    int Count { get; }
    IReadOnlyCollection<KeywordType> KeywordTypes { get; }
}