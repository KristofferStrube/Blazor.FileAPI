using System.ComponentModel;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

[JsonConverter(typeof(EnumDescriptionConverter<EndingType>))]
public enum EndingType
{
    [Description("transparent")]
    Transparent,
    [Description("native")]
    Native,
}
