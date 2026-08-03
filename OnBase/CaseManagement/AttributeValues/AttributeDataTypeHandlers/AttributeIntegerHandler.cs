using System.Globalization;
using Ternary.DataConversions;

namespace HyRest.CaseManagement;

public class AttributeIntegerHandler : AttributeDataTypeProvider
{
    public AttributeIntegerHandler(Attribute attribute, 
        string formatString = "{0:#}", IFormatProvider? 
        optionalProvider = null) 
        : base(attribute, formatString, optionalProvider)
    {

    }
    public override long? ToLong(object? value)
    {
        if (value == null)
            throw new ConversionException("The value passed was null");
        var str = value?.ToString() ?? string.Empty;
        return base.ToLong(value);
    }
    public override object? Parse(object? value)
        => ToLong(value);
}
