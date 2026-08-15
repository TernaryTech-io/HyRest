using System.Globalization;
using Ternary.DataConversions.Providers;
using HyRest.Utilities;
using System.Text.Json.Serialization;

namespace HyRest.OnBase.Core;

public class KeywordType : OnBaseItemTypeService<OnBaseCore, KeywordTypeModel>
{
    private CurrencyFormat? _currencyFormat { get; set; }
    internal CultureInfo Culture => new CultureInfo(Module.App.ClientOptions.DefaultLanguage);
    public KeywordType(OnBaseCore core, KeywordTypeModel keywordType) : base(core, keywordType)
    {

    }
    [HyRestConverter<DataTypeToStringConverter>]
    public KeywordDataType DataType => KeywordDataType.Get(Item.DataType);
    public bool UsedForRetrieval => Item.UsedForRetrieval;
    public bool Invisible => Item.Invisible;
    public AlphanumericSettings? AlphanumericSettings => Item.AlphanumericSettings;
    [JsonIgnore]
    public CurrencyFormat? CurrencyFormat
    {
        get
        {
            if (_currencyFormat == null && Item.CurrencyFormatId != null)
                _currencyFormat = Module.CurrencyFormats[Item.CurrencyFormatId];
            return _currencyFormat;
        }
    }
    public bool IsSecurityMasked => Item.IsSecurityMasked;
    public KeywordTypeMaskSettings? MaskSettings => Item.MaskSettings;
    public IDataTypeConversionProvider CreateKeywordDataTypeHandler()
        => DataType.GetProvider(this);
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
    
