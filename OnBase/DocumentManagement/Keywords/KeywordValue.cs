
using Ternary.DataConversions.Providers;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class KeywordValue : OnBaseItemService<IOnBaseDocumentAPI, OnBaseCore, KeywordValueModel>
{
    private readonly IDataTypeConversionProvider _handler;
    internal KeywordValue(OnBaseCore core, KeywordValueModel value, IDataTypeConversionProvider handler) : base(core,value)
    {
        _handler = handler;
    }
    public string? FormattedValue => Item.FormattedValue;
    public object? Value => _handler.Parse(Item.Value);
    public string? AlphanumericValue => _handler.ToString(Value);
    public DateTime? DateTimeValue => _handler.ToDateTime(Value);
    public DateOnly? DateValue => _handler.ToDateOnly(Value);
    public decimal? Numeric20Value => _handler.ToDecimal(Value);
    public long? Numeric9Value => _handler.ToLong(Value);
    public decimal? CurrencyValue => _handler.ToDecimal(Value);
    public double? FloatingPointValue => _handler.ToDouble(Value);
    public override string? ToString()
    {
        return _handler.ToString(Value);
    }
    internal KeywordValueModel GetModel()
        => Item;
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}