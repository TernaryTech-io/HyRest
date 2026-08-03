namespace HyRest.CaseManagement;

public class AttributeFloatProvider : AttributeDataTypeProvider
{
    public AttributeFloatProvider(Attribute attribute, string formatString = "{#.#}", IFormatProvider? optionalFormatProvider = null)
        : base(attribute, formatString, optionalFormatProvider)
    {

    }
    public override object? Parse(object? value)
        => ToDouble(value);
}