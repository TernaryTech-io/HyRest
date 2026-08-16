using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyRest.OnBase.WorkView;

public class AttributeTextProvider : AttributeDataTypeProvider
{
    public AttributeTextProvider(Attribute attribute, string formatString = "{0}", IFormatProvider? optionalFormatProvider = null)
        : base(attribute, formatString, optionalFormatProvider)
    {
        
    }
    public override string? Parse(object? value)
        => ToString(value);
}