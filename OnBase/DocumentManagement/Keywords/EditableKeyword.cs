
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.Core;

public sealed class EditableKeyword : Keyword, IEditableKeyword
{
    private readonly bool _keywordGroupMember;
    internal EditableKeyword(OnBaseCore core, KeywordModel keyword, bool keywordGroupMember) : base(core, keyword)
    {
        _keywordGroupMember = keywordGroupMember;
    }
    public EditableKeyword AddRange(IEnumerable<object> values)
    {
        values.ToList().ForEach(v => Add(v));
        return this;
    }
    public EditableKeyword TryAddRange(IEnumerable<object> values, out List<Exception> exceptions)
    {
        exceptions = [];
        Values.TryAddRange(values, _keywordGroupMember, out exceptions);
        return this;
    }
    public EditableKeyword Add(object value)
    {
        Values.Add(value, _keywordGroupMember);
        return this;
    }
    public EditableKeyword TryAdd(object value, out Exception? ex)
    {
        ex = null;
        Values.TryAdd(value, _keywordGroupMember, out ex);
        return this;
    }
    public EditableKeyword Update(object oldValue, object newValue)
    {
        Values.Update(oldValue, newValue);
        return this;
    }
    public EditableKeyword Remove(object oldValue)
    {
        Values.Remove(oldValue);
        return this;
    }
    public EditableKeyword ClearValues()
    {
       Values.ClearValues();
        return this;
    }
    internal Keyword ToKeyword()
    {
        return new Keyword(Module, Item);
    }
    public new EditableKeywordValueCollection Values
        => Values.AsEditable();
    IEditableKeyword IEditableKeyword.AddRange(IEnumerable<object> values)
        => AddRange(values);
    IEditableKeyword IEditableKeyword.Add(object value)
        => Add(value);
    IEditableKeyword IEditableKeyword.Update(object oldValue, object newValue)
        => Update(oldValue, newValue);
    IEditableKeyword IEditableKeyword.ClearValues()
        => ClearValues();
}

public interface IEditableKeyword : IKeyword
{
    IEditableKeyword AddRange(IEnumerable<object> values);
    IEditableKeyword Add(object value);
    IEditableKeyword Update(object oldValue, object newValue);
    IEditableKeyword ClearValues();
}