using System.Text.Json.Serialization;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class KeywordCollection : OnBaseItemService<OnBaseCore, KeywordCollectionModel>
{
    [JsonIgnore]
    public override long Id => base.Id;
    private List<IKeywordGroup> _groups => Item.Items.Select(g => IKeywordGroup.Create(Module, g)).ToList();
    private StandAloneKeywords _standAloneKeywords => _groups.Where(g => g is StandAloneKeywords)
        .Select(g => (StandAloneKeywords)g).FirstOrDefault() ?? new StandAloneKeywords(Module, new KeywordGroupModel());
    private SingleInstanceGroupCollection _singleInstanceGroups
        => new SingleInstanceGroupCollection(_groups.Where(g => g is SingleInstanceKeywordGroup)
            .Select(g => (SingleInstanceKeywordGroup)g).ToList());
    private SortedMultiInstanceCollections _multiInstanceGroups
        => new SortedMultiInstanceCollections(_groups.Where(g => g is MultiInstanceKeywordGroup)
            .Select(g => (MultiInstanceKeywordGroup)g).ToList());
    public KeywordCollection(OnBaseCore core, KeywordCollectionModel collection)
        : base(core, collection)
    {
               
    }
    public Guid KeywordGuid => Item.KeywordGuid != null ? Guid.Parse(Item.KeywordGuid) : Guid.Empty;
    public StandAloneKeywords StandAloneKeywords => _standAloneKeywords;
    public SingleInstanceGroupCollection SingleInstanceGroups => _singleInstanceGroups;
    public SortedMultiInstanceCollections MultiInstanceGroups => _multiInstanceGroups;

    public EditableMultiInstanceRecord CreateEditableMultiInstanceRecord(string name, string? instanceId = null)
    {
        
        var groupType = Module.KeywordTypeGroups.Find(name);
        if (groupType == null)
            throw new Exception($"Could not find keyword group type {name}.");
        return CreateEditableMultiInstanceRecord(groupType.Id, instanceId);
        
    }

    /// <summary>
    /// Create an editable Keyword Group Record, either new or existing.
    /// </summary>
    /// <param name="typeId">Required. The Id of the KeywordGroup Type.</param>
    /// <param name="instanceId">Optional. The instance Id of the group to modify.</param>
    /// <returns></returns>
    public EditableMultiInstanceRecord CreateEditableMultiInstanceRecord(long typeId, string? instanceId = null)
    {
        var group = Item.Items.Where(k => k.Id == typeId.ToString()).ToList();
        if (group == null || group.Count == 0)
            throw new Exception($"Could not locate the multi instance keyword groupd with id {typeId}");
        if (instanceId != null)
        {
            var inst = group.FirstOrDefault(g => g.InstanceId == instanceId);
            if (inst == null)
                throw new Exception($"The keyword group record with the instance id of: {instanceId}, was not found.");
            return new EditableMultiInstanceRecord(Module, inst);
        }        
        var model = new KeywordGroupModel
        {
            Id = typeId.ToString()
        };
        foreach(var k in group.First().Keywords)
        {
            var kw = new KeywordModel
            {
                Id = k.Id
            };
            model.Keywords.Add(kw);
        }
        Item.Items.Add(model);
        return new EditableMultiInstanceRecord(Module, model);
    }    
    public EditableSingleInstanceRecord CreateEditableSingleInstanceRecord(string name)
    {
        var groupType = Module.KeywordTypeGroups.Find(name);
        if (groupType == null)
            throw new Exception($"Could not find keyword group type {name}.");
        return CreateEditableSingleInstanceRecord(groupType.Id);
    }
    public EditableSingleInstanceRecord CreateEditableSingleInstanceRecord(long typeId)
    {
        var record = Item.Items.FirstOrDefault(g => g.Id == typeId.ToString());
        if (record == null)
            throw new Exception($"Could not find the single instance keyword group with id {typeId}");
        return new EditableSingleInstanceRecord(Module, record);
    }
    public EditableKeyword CreateEditableKeyword(string name)
    {
        var keyType = Module.KeywordTypes.Find(name);
        if (keyType == null)
            throw new Exception($"Could not find keyword type {name}.");
        return CreateEditableKeyword(keyType.Id);
    }
    public EditableKeyword CreateEditableKeyword(long typeId)
    {
        var existing = Item.Items.FirstOrDefault(g =>
        {
            if (g.Id == null && g.Keywords.Any(k => k.Id == typeId.ToString()))
                return true;
            else
                return false;
        });
        if (existing == null)
            throw new Exception($"Could not find a stand alone keyword with Id {typeId}");
        var key = existing.Keywords.FirstOrDefault(k => k.Id == typeId.ToString());
        if(key == null)
            throw new Exception($"Could not find a stand alone keyword with Id {typeId}");
        return new EditableKeyword(Module, key, false);
    }
    public void RemoveMultiInstanceRecord(long groupId)
    {
        var existing = Item.Items
            .FirstOrDefault(r => r.GroupId == groupId.ToString());
        if (existing == null)
            throw new Exception($"The keyword group with groupId, {groupId}, does not exist in the collection.");
        Item.Items.Remove(existing);
    }
    public void RemoveMultiInstanceRecord(string instanceId)
    {
        var existing = Item.Items
            .FirstOrDefault(r => r.InstanceId == instanceId);
        if (existing == null)
            throw new Exception($"The keyword group with instance id, {instanceId}, does not exist in the collection.");
        Item.Items.Remove(existing);
    }
    internal KeywordCollectionModel GetModel()
        => Item;
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}

public enum KeywordTypeGroupType
{
    MultiInstance,
    SingleInstance,
    StandAlone
}