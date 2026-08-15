using System.Net;
using Ternary.DataConversions;

namespace HyRest.OnBase.WorkView;

public class AttributeFormattedTextProvider : AttributeDataTypeProvider
{
    public AttributeFormattedTextProvider(Attribute attribute, string formatString = "{0}", IFormatProvider? optionalFormatProvider = null)
        : base(attribute, formatString, optionalFormatProvider)
    {
        
    }
    public override string? Parse(object? value)
        => ToString(value);
    public override string ToString(object? value)
    {
        if (value == null)
            throw new ConversionException("The provided value is null");
        var strValue = value.ToString();

        if (string.IsNullOrEmpty(strValue) || string.IsNullOrWhiteSpace(strValue))
            throw new ConversionException("The value was null or empty after being converted to string.");

        return WebUtility.HtmlEncode(strValue);
    }
}