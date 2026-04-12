using System.Text.Json;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI.Converters;

internal class EndingTypeConverter : JsonConverter<EndingType>
{
    public override EndingType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "transparent" => EndingType.Transparent,
            "native" => EndingType.Native,
            var value => throw new ArgumentException($"Value '{value}' was not a valid {nameof(EndingType)}."),
        };
    }

    public override void Write(Utf8JsonWriter writer, EndingType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value switch
        {
            EndingType.Transparent => "transparent",
            EndingType.Native => "native",
            _ => throw new ArgumentException($"Value '{value}' was not a valid {nameof(EndingType)}.")
        });
    }
}
