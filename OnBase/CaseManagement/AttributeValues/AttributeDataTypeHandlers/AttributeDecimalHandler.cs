using System.Globalization;

namespace HyRest.CaseManagement;

public class AttributeDecimalProvider : AttributeDataTypeProvider
{
    public AttributeDecimalProvider(Attribute attribute, string formatString = "{0:#.00}", IFormatProvider? optionalFormatProvider = null)
        : base(attribute, formatString, optionalFormatProvider, NumberStyles.AllowDecimalPoint)
    {

    }
    public override object? Parse(object? value)
        => ToDecimal(value);
}
