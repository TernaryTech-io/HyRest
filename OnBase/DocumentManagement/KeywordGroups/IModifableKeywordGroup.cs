namespace HyRest.OnBase.Core;

public class EditableMultiInstanceRecord : EditableKeywordGroup, IModifiableKeywordRecord
{
    public EditableMultiInstanceRecord(OnBaseCore core, KeywordGroupModel group) : base(core, group) { }

    public override string Name => KeywordTypeGroup?.Name ?? string.Empty;
    public override string SystemName => KeywordTypeGroup?.SystemName ?? string.Empty;
    public string? InstanceId => Item.InstanceId;
    public override KeywordTypeGroupType GroupType => KeywordTypeGroupType.MultiInstance;    
    internal MultiInstanceKeywordGroup ToReadOnlyGroup()
        => new MultiInstanceKeywordGroup(Module, Item);
}
public class EditableSingleInstanceRecord : EditableKeywordGroup, IModifiableKeywordRecord
{
    public EditableSingleInstanceRecord(OnBaseCore core, KeywordGroupModel group) : base(core, group) { }
    public override KeywordTypeGroupType GroupType => KeywordTypeGroupType.SingleInstance;
    public override string Name => KeywordTypeGroup?.Name ?? string.Empty;
    public override string SystemName => KeywordTypeGroup?.SystemName ?? string.Empty;
    internal SingleInstanceKeywordGroup ToReadOnlyKeywordGroup()
        => new SingleInstanceKeywordGroup(Module, Item);
}
public interface IModifiableKeywordRecord : IKeywordGroup, IOnBaseItemService
{
    void Add(EditableKeyword item);
    void AddRange(IEnumerable<EditableKeyword> items);
    void Clear();
}