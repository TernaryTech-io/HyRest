using System.Globalization;

namespace HyRest.OnBase.Core;

public class KeywordCurrencyProvider : KeywordDataTypeProvider
{
    public KeywordCurrencyProvider(KeywordType keywordType, string formatString = "{0:#.00}", IFormatProvider? optionalFormatProvider = null)
        : base(keywordType, formatString, optionalFormatProvider, NumberStyles.Currency)
    {

    }    
    public static IFormatProvider? CreateCurrencyProvider(KeywordType keyType)
    {
        if (keyType.CurrencyFormat != null)
            return keyType.CurrencyFormat.FormatProvider;
        else return null;
    }
    public override object? Parse(object? value)
        =>  base.ToDecimal(value);
    public override string? ToString(object? value)
    {
        if (value == null)
            throw new ConversionException("The value passed was null");
        var result = base.ToString(value);
        if (result == null)
            throw new ConversionException("The value failed to convert to string.");
        return result;
    }
}