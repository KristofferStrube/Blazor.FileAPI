using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

public class FilePropertyBag : BlobPropertyBag
{
    /// <summary>
    /// When the new <see cref="File"/> was last modified.
    /// </summary>
    [JsonPropertyName("lastModified")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
}
