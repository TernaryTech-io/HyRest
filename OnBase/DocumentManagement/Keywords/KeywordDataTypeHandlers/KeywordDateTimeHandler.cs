namespace HyRest.OnBase.Core;
public class KeywordDateTimeProvider : KeywordDataTypeProvider
{
    public KeywordDateTimeProvider(KeywordType keywordType, string formatString = "{0:yyyy-MM-ddTHH:mm:ss}", IFormatProvider? optionalFormatProvider = null)
        : base(keywordType, formatString, optionalFormatProvider)
    {

    }
    public override object? Parse(object? value)
        => ToDateTime(value);
}
