using System.Globalization;

namespace HyRest.OnBase.WorkView;

public class AttributeCurrencyProvider : AttributeDataTypeProvider
{
    public AttributeCurrencyProvider(Attribute attribute, string formatString = "{0:#.00}", IFormatProvider? optionalFormatProvider = null)
        : base(attribute, formatString, optionalFormatProvider, NumberStyles.Currency)
    {

    }
    public override object? Parse(object? value)
        => ToDecimal(value);
}