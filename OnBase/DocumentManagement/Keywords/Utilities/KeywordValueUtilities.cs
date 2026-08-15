using System.Text.Json;
using System.Text.Json.Serialization;
using HyRest.OnBase.Core;

namespace HyRest.Utilities;

public class KeywordValuesToStringConverter : JsonConverter<IReadOnlyCollection<KeywordValue>>
{
    public override IReadOnlyCollection<KeywordValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
    public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<KeywordValue> values, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var value in values)
        {
            JsonSerializer.Serialize(writer, value.ToString(), options);
        }
        writer.WriteEndArray();
    }
}
public class KeywordValueToStringConverter : JsonConverter<KeywordValue>
{
    public override KeywordValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
    public override void Write(Utf8JsonWriter writer, KeywordValue value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
public class DataTypeToStringConverter : JsonConverter<KeywordDataType>
{
    public override KeywordDataType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Expected a string for Type.");

        string[] value = reader.GetString()?.Split(' ') ?? [];
        if (value.Length != 2)
            return new KeywordDataType { };
        return KeywordDataType.Get(value[0]);
    }
    public override void Write(Utf8JsonWriter writer, KeywordDataType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"{value.DataType} ({value.CommonType})");
    }
}