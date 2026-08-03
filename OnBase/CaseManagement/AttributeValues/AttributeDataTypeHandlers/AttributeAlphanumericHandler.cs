namespace HyRest.CaseManagement;

public class AttributeAlphanumericProvider : AttributeDataTypeProvider
{
    public AttributeAlphanumericProvider(Attribute attribute, string formatString = "{0}", IFormatProvider? optionalFormatProvider = null)
        : base(attribute, formatString, optionalFormatProvider)
    {

    }
    public override string? Parse(object? value)
        => ToString(value);    
}