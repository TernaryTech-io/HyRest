using System.Globalization;
using Ternary.DataConversions;

namespace HyRest.OnBase.Core;

public class KeywordNumeric20Provider: KeywordDataTypeProvider
{
    public KeywordNumeric20Provider(KeywordType keywordType, string formatString = "{0:#}", IFormatProvider? optionalFormatProvider = null) 
        : base(keywordType, formatString, optionalFormatProvider ?? CreateNumericFormat())
    {

    }
    public override object? Parse(object? value)
        => ToDecimal(value);
    public override decimal? ToDecimal(object? value)
    {
        var str = value?.ToString() ?? string.Empty;
        if (str.Length > 20)
            throw new ConversionException("The value was over the alloted 20 character limit");
        return base.ToDecimal(value);
    }
    public override string? ToString(object? value)
    {
        if (value == null)
            throw new ConversionException("The value passed was null");
        string? result = base.ToString(value)?.Trim();
        if(result == null)
            throw new ConversionException("The converted value was null");
        if (result.Length > 20)
            throw new ConversionException("The value was over the alloted 20 character limit");
        if (KeywordType.MaskSettings != null && KeywordType.MaskSettings.MaskString != null)
        {
            var mask = new OnBaseKeywordMask(KeywordType.MaskSettings.MaskString, KeywordType.MaskSettings.StaticCharacters);
            if (!mask.TryApplyMask(result, out result))
                return result;
        }
        return result;
    }
    public static CultureInfo CreateNumericFormat()
    {
        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        var nfi = (NumberFormatInfo)culture.NumberFormat.Clone();
        nfi.CurrencyDecimalDigits = 0;
        nfi.NumberDecimalDigits = 0;
        nfi.PercentDecimalDigits = 0;
        culture.NumberFormat = nfi;
        return culture;
    }
}
