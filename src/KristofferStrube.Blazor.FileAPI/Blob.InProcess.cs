using KristofferStrube.Blazor.Streams;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#blob-section">Blob browser specs</see>
/// </summary>
public class BlobInProcess : Blob
{
    public new IJSInProcessObjectReference JSReference;
    protected readonly IJSInProcessObjectReference inProcessHelper;

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="Blob"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Blob"/>.</param>
    /// <returns>A wrapper instance for a <see cref="Blob"/>.</returns>
    public static async Task<BlobInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        IJSInProcessObjectReference inProcesshelper = await jSRuntime.GetInProcessHelperAsync();
        return new BlobInProcess(jSRuntime, inProcesshelper, jSReference);
    }

    /// <summary>
    /// Constructs a wrapper instance using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="blobParts">The parts that will make the new <see cref="Blob"/>.</param>
    /// <param name="options">Options for constructing the new Blob which includes MIME type and line endings settings.</param>
    /// <returns></returns>
    public static new async Task<BlobInProcess> CreateAsync(IJSRuntime jSRuntime, IList<BlobPart>? blobParts = null, BlobPropertyBag? options = null)
    {
        IJSInProcessObjectReference inProcesshelper = await jSRuntime.GetInProcessHelperAsync();
        object?[]? jsBlobParts = blobParts?.Select<BlobPart, object?>(blobPart => blobPart.type switch
            {
                BlobPartType.BufferSource => blobPart.byteArrayPart,
                BlobPartType.Blob => blobPart.stringPart,
                _ => blobPart.blobPart?.JSReference
            })
            .ToArray();
        IJSInProcessObjectReference jSInstance = await inProcesshelper.InvokeAsync<IJSInProcessObjectReference>("constructBlob", jsBlobParts, options);
        return new BlobInProcess(jSRuntime, inProcesshelper, jSInstance);
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="Blob"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="inProcessHelper">An in process helper instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Blob"/>.</param>
    internal BlobInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference) : base(jSRuntime, jSReference)
    {
        this.inProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    /// <summary>
    /// The size of this blob.
    /// </summary>
    /// <returns>A <see langword="ulong"/> representing the size of the blob in bytes.</returns>
    public ulong Size => inProcessHelper.Invoke<ulong>("getAttribute", JSReference, "size");

    /// <summary>
    /// The media type of this blob. This is either a parseable MIME type or an empty string.
    /// </summary>
    /// <returns>The MIME type of this blob.</returns>
    public string Type => inProcessHelper.Invoke<string>("getAttribute", JSReference, "type");

    public Blob Slice(long? start = null, long? end = null, string? contentType = null)
    {
        start ??= 0;
        end ??= (long)Size;
        IJSObjectReference jSInstance = JSReference.Invoke<IJSObjectReference>("slice", start, end, contentType);
        return new Blob(jSRuntime, jSInstance);
    }
}
