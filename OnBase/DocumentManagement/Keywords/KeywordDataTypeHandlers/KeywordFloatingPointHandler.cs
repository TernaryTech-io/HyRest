using System.Globalization;

namespace HyRest.DocumentManagement;

public class KeywordFloatingPointProvider : KeywordDataTypeProvider
{
    public KeywordFloatingPointProvider(KeywordType keywordType, string formatString = "{0:0.000000}", IFormatProvider? optionalFormatProvider = null)
        : base(keywordType, formatString, optionalFormatProvider ?? CreateFloatingPointFormat())
    {

    }   
    public static CultureInfo CreateFloatingPointFormat()
    {
        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        var nfi = (NumberFormatInfo)culture.NumberFormat.Clone();
        nfi.CurrencyDecimalDigits = 6;
        nfi.NumberDecimalDigits = 6;
        nfi.PercentDecimalDigits = 6;
        culture.NumberFormat = nfi;
        return culture;
    }
    public override double? ToDouble(object? value)
    {
        return base.ToDouble(value);
    }
    public override object? Parse(object? value)
        => ToDouble(value);
}