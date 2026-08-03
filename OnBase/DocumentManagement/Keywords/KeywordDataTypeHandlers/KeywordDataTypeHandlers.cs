using System.Globalization;
using Ternary.DataConversions.Providers;

namespace HyRest.DocumentManagement;

public abstract class KeywordDataTypeProvider : DataTypeConversionProvider
{
    private readonly KeywordType _keywordType;
    public KeywordDataTypeProvider(KeywordType keywordType, 
        string formatString = "{0}",
        IFormatProvider? optionalProvider = null,
        NumberStyles numberStyles = NumberStyles.None,
        DateTimeStyles dateTimeStyles = DateTimeStyles.None,
        string[]? dateFormatStrings = null)
        : base(formatString, optionalProvider ?? keywordType.Culture, numberStyles, dateTimeStyles,dateFormatStrings)
    {
        _keywordType = keywordType;
    }
    public KeywordType KeywordType => _keywordType;    
}