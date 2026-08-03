namespace HyRest.DocumentManagement;

public class KeywordAlphanumericProvider : KeywordDataTypeProvider
{
    public KeywordAlphanumericProvider(KeywordType keywordType, string formatString = "{0}", bool trimValues = true,
        IFormatProvider? optionalFormatProvider = null)
        : base(keywordType, formatString, optionalFormatProvider)
    {
        
    }
    public override string? Parse(object? value)
        => ToString(value);
    public override string? ToString(object? value)
    {
        if (value == null)
            throw new ConversionException("The value passed was null");
        string? result = base.ToString(value)?.Trim();
        if (result == null)
            throw new ConversionException("The value could not be converted to a string.");
        if (KeywordType.AlphanumericSettings != null)
        {
            if (KeywordType.AlphanumericSettings.CaseOptions == AlphanumericCaseOptions.Uppercase)
                result = result.ToUpper();
            if (result.Length > KeywordType.AlphanumericSettings.MaximumLength)
                result = result.Substring(0, (int)KeywordType.AlphanumericSettings.MaximumLength);
        }
        if(KeywordType.MaskSettings != null && KeywordType.MaskSettings.MaskString != null)
        {
            var mask = new OnBaseKeywordMask(KeywordType.MaskSettings.MaskString, KeywordType.MaskSettings.StaticCharacters);
            if (!mask.TryApplyMask(result, out string masked))
                return result;
            else throw new ConversionException($"The KeywordType mask can not be applied to the value provided.");
        }
            
        return result;
    }
}
