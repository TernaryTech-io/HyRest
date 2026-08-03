
using Microsoft.Extensions.Logging;

namespace HyRest.DocumentManagement;

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
        foreach(var value in values.ToList())
        {
            TryAdd(value, out Exception? ex);
            if (ex != null)
                exceptions.Add(ex);
        }
        return this;
    }
    public EditableKeyword Add(object value)
    {
        TryAdd(value, out Exception? ex);
        if (ex == null)
            return this;
        else
            throw ex.InnerException ?? ex ?? new Exception("The was an unhandled exception while trying to validate the keyword value");
    }
    public EditableKeyword TryAdd(object value, out Exception? ex)
    {
        ex = null;
        try
        {
            string? strValue = _handler.ToString(_handler.Parse(value));
            if (strValue == null)
            {
                Module.App.Logger.LogWarning($"Failed to add value '{value.ToString()}' to keyword {Name}", _handler.Exception);
                ex = new Exception($"Failed to add value '{value.ToString()}' to keyword {Name}", _handler.Exception);
                return this;
            }
            if (Item.Values.Any(v => v.Value == strValue))
                return this;
            if (_keywordGroupMember)
                Item.Values.Clear();
            Item.Values.Add(new KeywordValueModel { Value = strValue });
        }
        catch (Exception e)
        {
            Module.App.Logger.LogError($"Failed to add value '{value?.ToString()}' to keyword {Name}", e);
            ex = new Exception($"Failed to add value '{value?.ToString()}' to keyword {Name}", e);
        }
        return this;
    }
    public EditableKeyword Update(object oldValue, object newValue)
    {
        var value = _handler.ToString(_handler.Parse(oldValue));
        var existing = Item.Values.FirstOrDefault(v => v.Value != null && v.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        if (existing != null)
            existing.Value = value;
        else
            Item.Values.Add(new KeywordValueModel { Value = value });
        return this;
    }
    public EditableKeyword Remove(object oldValue)
    {
        var value = _handler.ToString(_handler.Parse(oldValue));
        var existing = Item.Values.FirstOrDefault(v => v.Value != null && v.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        if (existing != null)
            Item.Values.Remove(existing);
        return this;
    }
    public EditableKeyword ClearValues()
    {
        Item.Values.Clear();
        return this;
    }
    internal Keyword ToKeyword()
    {
        return new Keyword(Module, Item);
    }
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