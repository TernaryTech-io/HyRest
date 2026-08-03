namespace HyRest.DocumentManagement;

public class KeywordDateProvider : KeywordDataTypeProvider
{
    public KeywordDateProvider(KeywordType keywordType, string formatString = "{0:yyyy-MM-dd}", IFormatProvider? optionalFormatProvider = null)
        : base(keywordType, formatString, optionalFormatProvider)
    {

    }
    public override object? Parse(object? value)
        => ToDateOnly(value);
}