using HyRest.Utilities;
using System.Globalization;
using Ternary.DataConversions.Providers;

namespace HyRest.OnBase.Core;

public sealed class KeywordType : OnBaseItemTypeService<OnBaseCore, KeywordTypeModel>
{
    private CurrencyFormat? _currencyFormat { get; set; }
    internal CultureInfo Culture => new CultureInfo(Module.App.ClientOptions.DefaultLanguage);
    public KeywordType(OnBaseCore core, KeywordTypeModel keywordType) : base(core, keywordType)
    {
        
    }
    public override string Name
    {
        get
        {
            if (Item.Name == null)
                PopulateDetails();
            return Item.Name;
        }
    }
    public override string? SystemName
    {
        get
        {
            if (Item.SystemName == null)
                PopulateDetails();
            return Item.SystemName;
        }
    }
    [HyRestConverter<DataTypeToStringConverter>]
    public KeywordDataType DataType => KeywordDataType.Get(Item.DataType);
    public bool UsedForRetrieval => Item.UsedForRetrieval;
    public bool Invisible => Item.Invisible;
    public AlphanumericSettings? AlphanumericSettings => Item.AlphanumericSettings;
    public string? CurrencyFormatId => Item.CurrencyFormatId;
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

    private void PopulateDetails()
    {
        var keyType = Module.KeywordTypes[Item.Id];
        if (keyType != null)
            ReplaceModel(keyType.Item);
    }
}
    
