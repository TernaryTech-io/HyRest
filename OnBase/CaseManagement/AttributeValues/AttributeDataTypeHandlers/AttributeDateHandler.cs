using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyRest.CaseManagement;

public class AttributeDateProvider : AttributeDataTypeProvider
{
    public AttributeDateProvider(Attribute attribute, string formatString = "{0:yyyy-MM-dd}", IFormatProvider? optionalFormatProvider = null)
        : base(attribute, formatString, optionalFormatProvider)
    {

    }
    public override object? Parse(object? value)
        => ToDateOnly(value);
}
