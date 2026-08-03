using System.Diagnostics.CodeAnalysis;
using Ternary.DataConversions.Providers;

/*
 * TO DO: Validate DataTypes, Create DataTypeConversionProviders
 * 
 */
namespace HyRest.CaseManagement;

public struct AttributeDataType : IEquatable<AttributeDataType>
{
    public static AttributeDataType LargeInteger => new AttributeDataType
    {
        DataType = AttributeTypeDataType.LargeInt
    };
    public static AttributeDataType Currency => new AttributeDataType
    {
        DataType = AttributeTypeDataType.Currency
    };
    public static AttributeDataType Float => new AttributeDataType
    {
        DataType = AttributeTypeDataType.Float
    };
    public static AttributeDataType Date => new AttributeDataType
    {
        DataType = AttributeTypeDataType.Date
    };
    public static AttributeDataType DateTime => new AttributeDataType
    {
        DataType = AttributeTypeDataType.DateTime
    };
    public static AttributeDataType Alphanumeric => new AttributeDataType
    {
        DataType = AttributeTypeDataType.Char
    };
    public static AttributeDataType Text => new AttributeDataType
    {
        DataType = AttributeTypeDataType.Text
    };
    public static AttributeDataType Relation => new AttributeDataType
    {
        DataType = AttributeTypeDataType.Relation
    };
    public static AttributeDataType Boolean => new AttributeDataType
    {
        DataType = AttributeTypeDataType.Boolean
    };
    public static AttributeDataType Document => new AttributeDataType
    {
        DataType = AttributeTypeDataType.Document
    };
    public static AttributeDataType FormattedText => new AttributeDataType
    {
        DataType = AttributeTypeDataType.FormattedText
    };
    public static AttributeDataType Decimal => new AttributeDataType
    {
        DataType = AttributeTypeDataType.Decimal
    };
    public static AttributeDataType EncryptedAlphanumeric => new AttributeDataType
    {
        DataType = AttributeTypeDataType.EncryptedAlphanumeric
    };
    public static AttributeDataType Get(AttributeTypeDataType DataType)
    {
        if (DataType == AttributeTypeDataType.LargeInt)
            return AttributeDataType.LargeInteger;
        if (DataType == AttributeTypeDataType.Currency)
            return AttributeDataType.Currency;
        if (DataType == AttributeTypeDataType.Float)
            return AttributeDataType.Float;
        if (DataType == AttributeTypeDataType.Date)
            return AttributeDataType.Date;
        if (DataType == AttributeTypeDataType.DateTime)
            return AttributeDataType.DateTime;
        if (DataType == AttributeTypeDataType.Char)
            return AttributeDataType.Alphanumeric;
        if (DataType == AttributeTypeDataType.Text)
            return AttributeDataType.Text;
        if (DataType == AttributeTypeDataType.Relation)
            return AttributeDataType.Relation;
        if (DataType == AttributeTypeDataType.Boolean)
            return AttributeDataType.Boolean;
        if (DataType == AttributeTypeDataType.Document)
            return AttributeDataType.Document;
        if (DataType == AttributeTypeDataType.FormattedText)
            return AttributeDataType.FormattedText;
        if (DataType == AttributeTypeDataType.Decimal)
            return AttributeDataType.Decimal;
        if (DataType == AttributeTypeDataType.EncryptedAlphanumeric)
            return AttributeDataType.EncryptedAlphanumeric;
        throw new Exception("The datatype has not been implemented.");
    }
    public AttributeTypeDataType DataType { get; set; }
    public Type CommonType
    {
        get => DataType switch
        {
            AttributeTypeDataType.LargeInt => typeof(long),
            AttributeTypeDataType.Relation => typeof(long),
            AttributeTypeDataType.Document => typeof(long),
            AttributeTypeDataType.DateTime => typeof(DateTime),
            AttributeTypeDataType.Date => typeof(DateOnly),
            AttributeTypeDataType.Currency => typeof(decimal),
            AttributeTypeDataType.Decimal => typeof(decimal),
            AttributeTypeDataType.Char => typeof(string),
            AttributeTypeDataType.FormattedText => typeof(string),
            AttributeTypeDataType.EncryptedAlphanumeric => typeof(string),
            AttributeTypeDataType.Text => typeof(string),
            AttributeTypeDataType.Float => typeof(double),
            _ => throw new NotImplementedException("This datatype has not been implemented.")
        };
    }
    public IDataTypeConversionProvider GetProvider(Attribute type)
    {
        if (DataType == AttributeTypeDataType.Currency)
            return new AttributeCurrencyProvider(type);
        if (DataType == AttributeTypeDataType.Char 
            || DataType == AttributeTypeDataType.EncryptedAlphanumeric)
            return new AttributeAlphanumericProvider(type);
        if (DataType == AttributeTypeDataType.LargeInt 
            || DataType == AttributeTypeDataType.Relation 
            || DataType == AttributeTypeDataType.Document)
            return new AttributeIntegerHandler(type);
        if (DataType == AttributeTypeDataType.Text)
            return new AttributeTextProvider(type);
        if(DataType == AttributeTypeDataType.FormattedText)
            return new AttributeFormattedTextProvider(type);
        if (DataType == AttributeTypeDataType.Date)
            return new AttributeDateProvider(type);
        if (DataType == AttributeTypeDataType.DateTime)
            return new AttributeDateTimeProvider(type);
        if (DataType == AttributeTypeDataType.Float)
            return new AttributeFloatProvider(type);        
        throw new Exception("The datatype has not been implemented.");
    }

    bool IEquatable<AttributeDataType>.Equals(AttributeDataType other)
    {
        if (other.DataType == this.DataType)
            return true;
        else
            return false;
    }
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is AttributeDataType kdt)
            return kdt.DataType == this.DataType;
        else if (obj is AttributeTypeDataType ktdt)
            return ktdt == this.DataType;
        else if (obj is Type type)
            return type == CommonType;
        else
            return false;
    }
    public override int GetHashCode() => HashCode.Combine(DataType, CommonType);
    public static bool operator ==(AttributeDataType left, AttributeDataType right) => left.Equals(right);
    public static bool operator !=(AttributeDataType left, AttributeDataType right) => !left.Equals(right);
    public static bool operator ==(AttributeDataType left, object right) => left.Equals(right);
    public static bool operator !=(AttributeDataType left, object right) => !left.Equals(right);
    public override string ToString()
    {
        return DataType.ToString();
    }
}
