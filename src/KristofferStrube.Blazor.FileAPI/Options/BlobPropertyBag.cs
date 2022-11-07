using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#dfn-BlobPropertyBag">BlobPropertyBag browser specs</see>
/// </summary>
public class BlobPropertyBag
{
    /// <summary>
    /// The MIME type of the new <see cref="Blob"/> in lowercase.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    /// <summary>
    /// The line endings for the new <see cref="Blob"/>.
    /// </summary>
    [JsonPropertyName("endings")]
    public EndingType Endings { get; set; } = EndingType.Transparent;
}
