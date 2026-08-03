using System.Globalization;
using Ternary.DataConversions.Providers;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class KeywordType : OnBaseItemTypeService<IOnBaseDocumentAPI, OnBaseCore, KeywordTypeModel>
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
    public CurrencyFormat? CurrencyFormat
    {
        get
        {
            if (_currencyFormat == null)
                PopulateCurrencyFormat().Wait();
            return _currencyFormat;
        }
    }
    public bool IsSecurityMasked => Item.IsSecurityMasked;
    public KeywordTypeMaskSettings? MaskSettings => Item.MaskSettings;
    private async Task PopulateCurrencyFormat()
    {
        if (Item.CurrencyFormatId != null)
        {
            var item = await Module.Run(Api.GetCurrencyFormatById(Item.CurrencyFormatId));
            if (item != null)
                _currencyFormat = new CurrencyFormat(Module, item);
        }
    }
    public IDataTypeConversionProvider CreateKeywordDataTypeHandler()
        => DataType.GetProvider(this);
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
    
