

using System.Text.Json.Serialization;
using Ternary.DataConversions.Providers;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;
public class Keyword : OnBaseItemService<IOnBaseDocumentAPI, OnBaseCore, KeywordModel>, IKeyword
{
    protected readonly IDataTypeConversionProvider _handler;
    private KeywordType? _keyType { get; set; }    
    internal Keyword(OnBaseCore core, KeywordModel keyword) : base(core, keyword)
    {
        _handler = KeywordType.CreateKeywordDataTypeHandler();
    }
    public override string Name => KeywordType.Name;
    public override string SystemName => KeywordType.SystemName;
    [HyRestConverter<DataTypeToStringConverter>]
    public KeywordDataType DataType => KeywordType.DataType;
    public object? this[int i] => Values.ElementAt(i).Value;
    public object? this[object value] => Values.FirstOrDefault(v => v.Value == value || v.FormattedValue == _handler.ToString(value));
    public string? AlphanumericValue(int index = 0) => Values.ElementAtOrDefault(index)?.AlphanumericValue;
    public long? Numeric9Value(int index = 0) => Values.ElementAtOrDefault(index)?.Numeric9Value;
    public decimal? Numeric20Value(int index = 0) => Values.ElementAtOrDefault(index)?.Numeric20Value;
    public decimal? CurrencyValue(int index = 0) => Values.ElementAtOrDefault(index)?.CurrencyValue;
    public double? FloatingPointValue(int index = 0) => Values.ElementAtOrDefault(index)?.FloatingPointValue;
    public DateOnly? DateValue(int index = 0) => Values.ElementAtOrDefault(index)?.DateValue;
    public DateTime? DateTimeValue(int index = 0) => Values.ElementAtOrDefault(index)?.DateTimeValue;
    public object? Value(int index = 0) => Values.ElementAtOrDefault(index)?.Value;
    [JsonIgnore]
    public int ValueCount => Item.Values.Count();
    [JsonIgnore]
    public bool HasValues => Item.Values.Count() > 0;
    [HyRestConverter<KeywordValuesToStringConverter>]
    public IReadOnlyCollection<KeywordValue> Values => Item.Values.Select(v => new KeywordValue(Module, v, _handler)).ToList().AsReadOnly();
    [JsonIgnore]
    public KeywordType KeywordType
    {
        get
        {
            if (_keyType == null)
                PopulateKeywordType().Wait();
            return _keyType;            
        }
    }    
    internal KeywordModel GetModel()
     => Item;
    private async Task PopulateKeywordType()
    { 
        if(Item.Id != null)
        {
            var item = await Module.Run(Api.GetKeywordTypeById(Item.Id, Options.DefaultLanguage));
            if (item != null)
                _keyType = new KeywordType(Module, item);
        }
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
public interface IKeyword : IOnBaseItemService
{
    long Id { get; }
    object? this[int i] { get; }
    public object? this[object value] { get; }
    int ValueCount { get; }
    KeywordType KeywordType { get; }
    IReadOnlyCollection<KeywordValue> Values { get; }
    string? AlphanumericValue(int index = 0);
    long? Numeric9Value(int index = 0);
    decimal? Numeric20Value(int index = 0);
    decimal? CurrencyValue(int index = 0);
    double? FloatingPointValue(int index = 0);
    DateOnly? DateValue(int index = 0);
    DateTime? DateTimeValue(int index = 0);
    object? Value(int index = 0);
    bool HasValues { get; }
}
