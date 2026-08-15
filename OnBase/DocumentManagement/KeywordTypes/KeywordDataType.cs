using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Ternary.DataConversions.Providers;
using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public struct KeywordDataType : IEquatable<KeywordDataType>
{
    public static KeywordDataType Alphanumeric => new KeywordDataType
    {
        DataType = KeywordTypeDataType.Alphanumeric
    };
    public static KeywordDataType Currency => new KeywordDataType
    {
        DataType = KeywordTypeDataType.Currency
    };
    public static KeywordDataType SpecificCurrency => new KeywordDataType
    {
        DataType = KeywordTypeDataType.SpecificCurrency
    };
    public static KeywordDataType Date => new KeywordDataType
    {
        DataType = KeywordTypeDataType.Date
    };
    public static KeywordDataType DateTime => new KeywordDataType
    {
        DataType = KeywordTypeDataType.DateTime
    };
    public static KeywordDataType Numeric9 => new KeywordDataType
    {
        DataType = KeywordTypeDataType.Numeric9
    };
    public static KeywordDataType Numeric20 => new KeywordDataType
    {
        DataType = KeywordTypeDataType.Numeric20
    };
    public static KeywordDataType FloatingPoint => new KeywordDataType
    {
        DataType = KeywordTypeDataType.FloatingPoint
    };
    public static KeywordDataType Get(string DataType)
    {
        if (DataType ==  "Currency")
            return KeywordDataType.Currency;
        if (DataType == "SpecificCurrency")
            return KeywordDataType.SpecificCurrency;
        if (DataType == "Numeric20")
            return KeywordDataType.Numeric20;
        if (DataType == "Numeric9")
            return KeywordDataType.Numeric9;
        if (DataType == "Date")
            return KeywordDataType.Date;
        if (DataType == "DateTime")
            return KeywordDataType.DateTime;
        if (DataType == "FloatingPoint")
            return KeywordDataType.FloatingPoint;
        if (DataType == "Alphanumeric")
            return KeywordDataType.Alphanumeric;
        throw new Exception("The datatype has not been implemented.");
    }
    public static KeywordDataType Get(KeywordTypeDataType DataType)
    {
        if (DataType == KeywordTypeDataType.Currency)
            return KeywordDataType.Currency;
        if (DataType == KeywordTypeDataType.SpecificCurrency)
            return KeywordDataType.SpecificCurrency;
        if (DataType == KeywordTypeDataType.Numeric20)
            return KeywordDataType.Numeric20;
        if (DataType == KeywordTypeDataType.Numeric9)
            return KeywordDataType.Numeric9;
        if (DataType == KeywordTypeDataType.Date)
            return KeywordDataType.Date;
        if (DataType == KeywordTypeDataType.DateTime)
            return KeywordDataType.DateTime;
        if (DataType == KeywordTypeDataType.FloatingPoint)
            return KeywordDataType.FloatingPoint;
        if (DataType == KeywordTypeDataType.Alphanumeric)
            return KeywordDataType.Alphanumeric;
        throw new Exception("The datatype has not been implemented.");
    }
    [HyRestConverter<JsonStringEnumConverter>]
    public KeywordTypeDataType DataType { get; set; }
    [HyRestConverter<TypeToStringConverter>]
    public Type CommonType
    {
        get => DataType switch
        {
            KeywordTypeDataType.SpecificCurrency => typeof(decimal),
            KeywordTypeDataType.Currency => typeof(decimal),
            KeywordTypeDataType.Numeric20 => typeof(decimal),
            KeywordTypeDataType.Numeric9 => typeof(long),
            KeywordTypeDataType.DateTime => typeof(DateTime),
            KeywordTypeDataType.Date => typeof(DateOnly),
            KeywordTypeDataType.FloatingPoint => typeof(double),
            KeywordTypeDataType.Alphanumeric => typeof(string),
            _ => throw new NotImplementedException("This datatype has not been implemented.")
        };
    }
    public IDataTypeConversionProvider GetProvider(KeywordType type)
    {
        if (DataType == KeywordTypeDataType.Currency
            || DataType == KeywordTypeDataType.SpecificCurrency)
            return new KeywordCurrencyProvider(type);
        if (DataType == KeywordTypeDataType.Numeric20)
            return new KeywordNumeric20Provider(type);
        if (DataType == KeywordTypeDataType.Numeric9)
            return new KeywordNumeric9Provider(type);
        if (DataType == KeywordTypeDataType.Date)
            return new KeywordDateProvider(type);
        if (DataType == KeywordTypeDataType.DateTime)
            return new KeywordDateTimeProvider(type);
        if (DataType == KeywordTypeDataType.FloatingPoint)
            return new KeywordFloatingPointProvider(type);
        if (DataType == KeywordTypeDataType.Alphanumeric)
            return new KeywordAlphanumericProvider(type);
        throw new Exception("The datatype has not been implemented.");
    }

    bool IEquatable<KeywordDataType>.Equals(KeywordDataType other)
    {
        if (other.DataType == this.DataType)
            return true;
        else
            return false;
    }
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is KeywordDataType kdt)
            return kdt.DataType == this.DataType;
        else if (obj is KeywordTypeDataType ktdt)
            return ktdt == this.DataType;
        else if (obj is Type type)
            return type == CommonType;
        else
            return false;
    }
    public override int GetHashCode() => HashCode.Combine(DataType, CommonType);
    public static bool operator ==(KeywordDataType left, KeywordDataType right) => left.Equals(right);
    public static bool operator !=(KeywordDataType left, KeywordDataType right) => !left.Equals(right);
    public static bool operator ==(KeywordDataType left, object right) => left.Equals(right);
    public static bool operator !=(KeywordDataType left, object right) => !left.Equals(right);
}