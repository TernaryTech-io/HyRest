using Ternary.DataConversions.Providers;

namespace HyRest.CaseManagement;

public class AttributeValues : OnBaseRestService<IOnBaseWorkViewAPI>
{
    private List<AttributeValue> _items { get; set; }
    private OnBaseWorkView _workview => (OnBaseWorkView)base.Module;
    internal AttributeValues(OnBaseWorkView module, AttributeValuesModel items) : base(module)
    {
        _items = items.Select(i => new AttributeValue(module, i)).ToList();
    }
}

public class AttributeValue : OnBaseRestService<IOnBaseWorkViewAPI>
{
    private KeyValuePair<string, string> _item { get; set; }
    private OnBaseWorkView _workview => (OnBaseWorkView)base.Module;
    private Attribute? _attributeType { get; set; }
    private IDataTypeConversionProvider? _handler => AttributeType?.handler;
    internal AttributeValue(OnBaseWorkView module, KeyValuePair<string, string> item) : base(module)
    {
        _item = item;
    }
    public Attribute? AttributeType
    {
        get
        {
            if (_attributeType == null)
                _attributeType = _workview.Attributes.Find(_item.Key);
            return _attributeType;
        }
    }

    public object? Value => _handler?.Parse(_item.Value);
    public string? AlphaNumericValue => _handler?.ToString(_item.Value);
    public string? TextValue => _handler?.ToString(_item.Value);
    public string? FormatedTextValue => _handler?.ToString(_item.Value);
    public long? IntegerValue => _handler?.ToLong(_item.Value);
    public long? DocumentValue => _handler?.ToLong(_item.Value);
    public long? RelationValue => _handler?.ToLong(_item.Value);
    public decimal? DecimalValue => _handler?.ToDecimal(_item.Value);
    public decimal? CurrencyValue => _handler?.ToDecimal(_item.Value);
    public DateOnly? DateValue => _handler?.ToDateOnly(_item.Value);
    public DateTime? DateTimeValue => _handler?.ToDateTime(_item.Value);
    public double? FloatValue => _handler?.ToFloat(_item.Value);
    public bool? BoolValue => _handler?.ToBool(_item.Value);

}