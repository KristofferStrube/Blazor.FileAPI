using System.Text.Json;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

internal class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetUInt64(out ulong jsonValue))
        {
            return DateTime.UnixEpoch.AddMilliseconds(jsonValue);
        }
        throw new JsonException($"string {reader.GetString()} could not be parsed as a long.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Subtract(DateTime.UnixEpoch).TotalMilliseconds);
    }
}