namespace HyRest.CaseManagement;

public class AttributeBooleanProvider : AttributeDataTypeProvider
{
    public AttributeBooleanProvider(Attribute attribute, string formatString = "{0}", IFormatProvider? optionalFormatProvider = null)
        : base(attribute, formatString, optionalFormatProvider)
    {

    }
    public override object? Parse(object? value)
        => ToBool(value);  
}