using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#blob-section">Blob browser specs</see>
/// </summary>
public class Blob : BaseJSWrapper
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="Blob"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Blob"/>.</param>
    /// <returns>A wrapper instance for a <see cref="Blob"/>.</returns>
    public static Blob Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return new Blob(jSRuntime, jSReference);
    }

    /// <summary>
    /// Constructs a wrapper instance using the standard constructor.
    /// </summary>
    /// <param name="blobParts">The parts that will make the new <see cref="Blob"/>.</param>
    /// <param name="options">Options for constructing the new Blob which includes MIME type and line endings settings.</param>
    /// <returns></returns>
    public static async Task<Blob> CreateAsync(IJSRuntime jSRuntime, IList<BlobPart>? blobParts = null, BlobPropertyBag? options = null)
    {
        var helper = await jSRuntime.GetHelperAsync();
        var jsBlobParts = blobParts is null ? null :
            blobParts
                .Select<BlobPart, object?>(blobPart => blobPart.type switch
                {
                    BlobPartType.BufferSource => blobPart.byteArrayPart,
                    BlobPartType.Blob => blobPart.stringPart,
                    _ => blobPart.blobPart?.JSReference
                })
                .ToArray();
        var jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructBlob", jsBlobParts, options);
        return new Blob(jSRuntime, jSInstance);
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="Blob"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Blob"/>.</param>
    internal Blob(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }

    /// <summary>
    /// The size of this blob.
    /// </summary>
    /// <returns>A <see langword="ulong"/> representing the size of the blob in bytes.</returns>
    public async Task<ulong> GetSizeAsync()
    {
        var helper = await helperTask.Value;
        return await helper.InvokeAsync<ulong>("getAttribute", JSReference, "size");
    }
    
    /// <summary>
    /// The media type of this blob. This is either a parseable MIME type or an empty string.
    /// </summary>
    /// <returns>The MIME type of this blob.</returns>
    public async Task<string> GetTypeAsync()
    {
        var helper = await helperTask.Value;
        return await helper.InvokeAsync<string>("getAttribute", JSReference, "type");
    }

    /// <summary>
    /// The content of the blob as a string.
    /// </summary>
    /// <returns>The content of the file in UTF-8 format.</returns>
    public async Task<string> TextAsync()
    {
        return await JSReference.InvokeAsync<string>("text");
    }

    /// <summary>
    /// The content of the blob as a byte array.
    /// </summary>
    /// <returns>All bytes in the blob.</returns>
    public async Task<byte[]> ArrayBufferAsync()
    {
        var helper = await helperTask.Value;
        return await helper.InvokeAsync<byte[]>("arrayBuffer", JSReference);
    }
}
