using System.ComponentModel;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#enumdef-endingtype">EndingType browser specs</see>
/// </summary>
[JsonConverter(typeof(EnumDescriptionConverter<EndingType>))]
public enum EndingType
{
    [Description("transparent")]
    Transparent,
    [Description("native")]
    Native,
}
