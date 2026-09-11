using Ternary.DataConversions.Providers;

namespace HyRest.OnBase.Core;


public class EditableKeywordValueCollection : KeywordValueCollection
{
    internal EditableKeywordValueCollection(OnBaseCore core, IEnumerable<KeywordValueModel> values, IDataTypeConversionProvider handler) 
        : base(core,values,handler)
    {

    }
    internal void AddRange(IEnumerable<object> values, bool keywordGroupMember)
    {
        values.ToList().ForEach(v => Add(v, keywordGroupMember));
    }
    internal void TryAddRange(IEnumerable<object> values, bool keywordGroupMember, out List<Exception> exceptions)
    {
        exceptions = [];
        foreach (var value in values.ToList())
        {
            if (!TryAdd(value, keywordGroupMember, out Exception? ex))
                exceptions.Add(ex);
        }
    }
    internal void Add(object value, bool keywordGroupMember)
    {
        if(!TryAdd(value, keywordGroupMember, out Exception? ex))
        {
            throw ex?.InnerException ?? ex ?? new Exception("The was an unhandled exception while trying to validate the keyword value");
        } 
    }
    internal bool TryAdd(object value, bool keywordGroupMember, out Exception? ex)
    {
        ex = null;
        try
        {
            string? strValue = _handler.ToString(_handler.Parse(value));
            if (strValue == null)
            {                
                ex = new Exception($"Failed to add value '{value.ToString()}' to keyword", _handler.Exception);
                return false;
            }
            lock(_lock)
            {
                if (_modelItems.Any(v => v.Value == strValue))
                    return true;

                if (keywordGroupMember)
                    _modelItems.Clear();
                _modelItems.Add(new KeywordValueModel { Value = strValue });
            }
            return true;
        }
        catch (Exception e)
        {
            ex = e;
            return false;
        }
    }
    internal void Update(object oldValue, object newValue)
    {
        var value = _handler.ToString(_handler.Parse(oldValue));
        lock(_lock)
        {
            var existing = _modelItems.FirstOrDefault(v => v.Value != null && v.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));
            if (existing != null)
                existing.Value = value;
            else
                _modelItems.Add(new KeywordValueModel { Value = value });
        }        
    }
    internal void Remove(object oldValue)
    {
        var value = _handler.ToString(_handler.Parse(oldValue));
        var existing = _modelItems.FirstOrDefault(v => v.Value != null && v.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        if (existing != null)
        {
            lock(_lock)
            {
                _modelItems.Remove(existing);
            }
        }
    }
    internal void ClearValues()
    {
        lock(_lock)
        {
            _modelItems.Clear();
        }
    }
}

public class KeywordValueCollection : ValueCollection<KeywordValue, KeywordValueModel>
{
    private readonly OnBaseCore _core;
    protected readonly IDataTypeConversionProvider _handler;
    internal KeywordValueCollection(OnBaseCore core, IEnumerable<KeywordValueModel> values, IDataTypeConversionProvider handler) : base(values)
    {
        _core = core;
        _handler = handler;
    }

    protected override List<KeywordValue> GetItems()
    {
        lock (_lock)
        {
            return _modelItems.Select(v => new KeywordValue(_core, v, _handler)).ToList();
        }
    }
    protected override KeywordValue FromModel(KeywordValueModel model)
        => new KeywordValue(_core, model, _handler);
    protected override KeywordValueModel ToModel(KeywordValue item)
        => item.GetModel();
    internal EditableKeywordValueCollection AsEditable()
        => new EditableKeywordValueCollection(_core, _modelItems, _handler);
}
