using System.Collections;
using System.Text.Json.Serialization;

namespace HyRest.OnBase.Core;


public abstract class EditableKeywordGroup : KeywordGroup, IModifiableKeywordRecord
{
    private object _lock = new object();
    private bool _keywordGroup => GroupType != KeywordTypeGroupType.StandAlone;
    internal EditableKeywordGroup(OnBaseCore core, KeywordGroupModel group) : base(core, group)
    {

    }
    /// <summary>
    /// Adds the Keyword to the collection, or adds it's values if it already exists.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Add(EditableKeyword item)
    {
        lock(_lock)
        {
            var existing = ValiditityCheck(item);
            if (existing != null)
            {
                item.Values.ToList().ForEach(v => existing.Values.Add(v.GetModel()));
            }
            else
                throw new Exception($"The keyword ({item.Id}) is not present in the keyword configuration");
        }
    }
    public void AddRange(IEnumerable<EditableKeyword> items) => items.ToList().ForEach(k => Add(k));
    /// <summary>
    /// Removes the existing keyword and replaces it with the provided keyword. If keyword does not exist it will perform an Add.
    /// </summary>
    /// <param name="item"></param>
    public void Update(EditableKeyword item)
    {
        lock(_lock)
        {
            var existing = ValiditityCheck(item);
            if (existing != null)
            {
                existing.Values.Clear();
                item.Values.ToList().ForEach(v => existing.Values.Add(v.GetModel()));
            }
            else
                throw new Exception($"The keyword ({item.Id}) is not present in the keyword configuration");
        }
    }
    public void UpdateRange(IEnumerable<EditableKeyword> items)
    {
        lock(_lock)
        {
            items.ToList().ForEach(k => Update(k));
        }
    }
    public void Clear()
    {
        lock(_lock)
        {
            foreach (var kw in Item.Keywords)
            {
                kw.Values.Clear();
            }
        }
    }
    public EditableKeyword CreateEditableKeyword(string identifier)
    {
        var keyType = Module.KeywordTypes.Find(identifier);
        if (keyType == null)
            throw new Exception($"Could not find keyword type {identifier}.");
        return CreateEditableKeyword(keyType.Id);
    }
    public EditableKeyword CreateEditableKeyword(long typeId)
    {
        var key = Item.Keywords.FirstOrDefault(k => k.Id == typeId.ToString());
        if (key == null)
            throw new Exception($"Could not find a stand alone keyword with Id {typeId}");
        return new EditableKeyword(Module, key, _keywordGroup);
    }
}
public abstract class KeywordGroup : OnBaseItemService<OnBaseCore, KeywordGroupModel>, IKeywordGroup
{
    private object _lock = new object();
    private KeywordTypeGroup? _keyTypeGroup { get; set; }
    internal KeywordGroup(OnBaseCore core, KeywordGroupModel group) : base(core, group)
    {        
        GetKeywordTypeGroup();
        if (KeywordTypeGroup == null)
            GroupType = KeywordTypeGroupType.StandAlone;
        else if (KeywordTypeGroup.StorageType == KeywordTypeGroupType.SingleInstance)
            GroupType = KeywordTypeGroupType.SingleInstance;
        else if (KeywordTypeGroup.StorageType == KeywordTypeGroupType.MultiInstance)
            GroupType = KeywordTypeGroupType.MultiInstance;
    }   
    internal KeywordGroup(OnBaseCore core, EditableSingleInstanceRecord group, bool readOnly = true) 
        : base(core, group.Item)
    {
        _keyTypeGroup = group.KeywordTypeGroup;
        GroupType = group.GroupType;
    }
    internal KeywordGroup(OnBaseCore core, EditableMultiInstanceRecord group)
        : base(core, group.Item)
    {
        _keyTypeGroup = group.KeywordTypeGroup;
        GroupType = group.GroupType;
    }   
    public virtual KeywordTypeGroupType GroupType { get; }
    public override string? TypeId => Item.Id;
    [JsonIgnore]
    public virtual KeywordTypeGroup? KeywordTypeGroup
    {
        get
        {
            if (_keyTypeGroup == null)
                GetKeywordTypeGroup();
            return _keyTypeGroup;
        }
    }
    public IReadOnlyCollection<Keyword> Keywords => GetKeywords();
    private IReadOnlyCollection<Keyword> GetKeywords()
    {
        lock(_lock)
        {
            return Item.Keywords.Select(k => new Keyword(Module, k)).ToList();
        }
    }

    private void GetKeywordTypeGroup()
    {
        if (Item.Id != null)
        {
            var item = Module.KeywordTypeGroups.Find(Item.Id);
            if (item != null && item is KeywordTypeGroup ktg)
                _keyTypeGroup = ktg;
        }            
    }

    internal KeywordGroupModel GetModel()
        => Item;
    public Keyword? this[string name] => Find(name);
    public Keyword? this[long id] => Find(id);

    public Keyword? Find(string name)
    {
        lock(_lock)
        {
            return Keywords.FirstOrDefault(k => k.Name == name || k.SystemName == name);
        }
    }
    public Keyword? Find(long id)
    {
        lock (_lock)
        {
            return Keywords.FirstOrDefault(k => k.Id == id);
        }
    }

    [JsonIgnore]
    public int Count => Keywords.Count;    
    public List<Keyword> ToList() => Keywords.ToList();
    public Keyword[] ToArray() => Keywords.ToArray();   
    public bool Contains(IKeyword item)
    {
        lock(_lock)
        {
            return Keywords.Any(k => k.Id == item.Id);
        }
    }
    public void Remove(IKeyword item)
    {
        lock(_lock)
        {
            var keyword = Item.Keywords.FirstOrDefault(k => k.Id == item.Id.ToString());
            if (keyword?.Values != null)
                keyword.Values.Clear();
        }
    }
    protected KeywordModel ValiditityCheck(Keyword item)
    {
        lock(_lock)
        {
            if (!Item.Keywords.Any(k => k.Id == item.Id.ToString()))
                throw new Exception($"The keyword type {item.Name} ({item.Id}) does not belong to this keyword group.");
            return Item.Keywords.First(k => k.Id == item.Id.ToString());
        }
    }
    
}

public interface IKeywordGroup : IOnBaseItemService
{
    long Id { get; }    
    KeywordTypeGroupType GroupType { get; }
    KeywordTypeGroup? KeywordTypeGroup { get; }
    Keyword? this[string name] { get; }
    Keyword? this[long id] { get; }
    int Count { get; }
    List<Keyword> ToList();
    public Keyword[] ToArray();
    bool Contains(IKeyword item);
    public void Remove(IKeyword item);
    static IKeywordGroup Create(OnBaseCore core, KeywordGroupModel group, bool readOnly = true)
    {       
        if (group.Id == null)
            return new StandAloneKeywords(core, group);
        else if(long.TryParse(group.Id, out long id))
        {
            var ktg = core.KeywordTypeGroups[id];
            if(ktg != null)
            {
                if (ktg.StorageType == KeywordTypeGroupType.SingleInstance)
                    return new SingleInstanceKeywordGroup(core, group);
                else if(ktg.StorageType == KeywordTypeGroupType.MultiInstance)
                    return new MultiInstanceKeywordGroup(core, group);
            }
        }
        throw new Exception($"The KeywordGroup Type could not be determined by the KeywordGroupModel.");
    }
}