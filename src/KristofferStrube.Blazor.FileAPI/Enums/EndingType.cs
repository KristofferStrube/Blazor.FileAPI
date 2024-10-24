using System.ComponentModel;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// It controls how line endings are handled in <see cref="string"/> elements within <see cref="BlobPart"/>s
/// </summary>
/// <remarks>
/// <see href="https://www.w3.org/TR/FileAPI/#enumdef-endingtype">EndingType browser specs</see>
/// </remarks>
[JsonConverter(typeof(EnumDescriptionConverter<EndingType>))]
public enum EndingType
{
    /// <summary>
    /// Line endings in <see cref="string"/> elements within <see cref="BlobPart"/>s remain unchanged, with no conversion applied, preserving the original format.
    /// </summary>
    [Description("transparent")]
    Transparent,

    /// <summary>
    /// Line endings in <see cref="string"/> elements within <see cref="BlobPart"/>s are converted to the platform's native format (<c>\n</c> or <c>\r\n</c>), ensuring consistency with the system's convention.
    /// </summary>
    [Description("native")]
    Native,
}
