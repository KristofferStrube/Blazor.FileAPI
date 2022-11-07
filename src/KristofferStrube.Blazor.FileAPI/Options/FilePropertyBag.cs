using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#dfn-FilePropertyBag">FilePropertyBag browser specs</see>
/// </summary>
public class FilePropertyBag : BlobPropertyBag
{
    /// <summary>
    /// When the new <see cref="File"/> was last modified.
    /// </summary>
    [JsonPropertyName("lastModified")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}
