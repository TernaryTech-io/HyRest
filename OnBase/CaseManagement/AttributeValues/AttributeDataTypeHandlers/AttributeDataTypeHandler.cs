using System.Globalization;
using Ternary.DataConversions.Providers;

namespace HyRest.CaseManagement;

public abstract class AttributeDataTypeProvider
    : DataTypeConversionProvider
{
    private readonly Attribute _attribute;
    public AttributeDataTypeProvider(Attribute attribute,
        string formatString = "{0}",
        IFormatProvider? optionalProvider = null,
        NumberStyles numberStyles = NumberStyles.None,
        DateTimeStyles dateTimeStyles = DateTimeStyles.None,
        string[]? dateFormatStrings = null)
        : base(formatString, optionalProvider ?? attribute.Culture, numberStyles, dateTimeStyles, dateFormatStrings)
    {
        _attribute = attribute;
    }
    public Attribute Attribute => _attribute;
}
