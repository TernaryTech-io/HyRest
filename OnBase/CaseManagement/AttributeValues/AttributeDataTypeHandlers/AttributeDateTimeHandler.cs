namespace HyRest.CaseManagement;

public class AttributeDateTimeProvider : AttributeDataTypeProvider
{
    public AttributeDateTimeProvider(Attribute attribute, string formatString = "{0:yyyy-MM-ddTHH:mm:ss}", IFormatProvider? optionalFormatProvider = null)
        : base(attribute, formatString, optionalFormatProvider)
    {

    }
    public override object? Parse(object? value)
        => ToDateTime(value);
}