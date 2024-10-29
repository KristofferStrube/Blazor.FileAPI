using KristofferStrube.Blazor.Streams;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// A <see cref="Blob"/> object refers to a byte sequence,
/// and has a <see cref="GetSizeAsync"/> attribute which is the total number of bytes in the byte sequence,
/// and a <see cref="GetTypeAsync"/> attribute, which is an ASCII-encoded string in lower case representing the media type of the byte sequence.
/// </summary>
/// <remarks><see href="https://www.w3.org/TR/FileAPI/#blob-section">See the API definition here</see></remarks>
[IJSWrapperConverter]
public class Blob : BaseJSWrapper, IJSCreatable<Blob>
{
    /// <inheritdoc/>
    public static async Task<Blob> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static Task<Blob> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new Blob(jSRuntime, jSReference, options));
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="Blob"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Blob"/>.</param>
    /// <returns>A wrapper instance for a <see cref="Blob"/>.</returns>
    [Obsolete("This will be removed in the next major release as all creator methods should be asynchronous for uniformity. Use CreateAsync instead.")]
    public static Blob Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return new Blob(jSRuntime, jSReference, new());
    }

    /// <summary>
    /// Constructs a wrapper instance using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="blobParts">The parts that will make the new <see cref="Blob"/>.</param>
    /// <param name="options">Options for constructing the new Blob which includes MIME type and line endings settings.</param>
    /// <returns></returns>
    public static async Task<Blob> CreateAsync(IJSRuntime jSRuntime, IList<BlobPart>? blobParts = null, BlobPropertyBag? options = null)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        object?[]? jsBlobParts = blobParts?.Select<BlobPart, object?>(blobPart => blobPart.Part switch
            {
                byte[] part => part,
                Blob part => part.JSReference,
                _ => blobPart.Part
            })
            .ToArray();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructBlob", jsBlobParts, options);
        return new Blob(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Blob(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }

    /// <summary>
    /// The size of this blob.
    /// </summary>
    /// <returns>A <see langword="ulong"/> representing the size of the blob in bytes.</returns>
    public async Task<ulong> GetSizeAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<ulong>("getAttribute", JSReference, "size");
    }

    /// <summary>
    /// The media type of this blob. This is either a parseable MIME type or an empty string.
    /// </summary>
    /// <returns>The MIME type of this blob.</returns>
    public async Task<string> GetTypeAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<string>("getAttribute", JSReference, "type");
    }

    /// <summary>
    /// Gets some range of the content of a <see cref="Blob"/> as a new <see cref="Blob"/>.
    /// </summary>
    /// <param name="start">The start index of the range. If <see langword="null"/> or negative then <c>0</c> is assumed.</param>
    /// <param name="end">The start index of the range. If <see langword="null"/> or larger than the size of the original <see cref="Blob"/> then the size of the original <see cref="Blob"/> is assumed.</param>
    /// <param name="contentType">An optional MIME type of the new <see cref="Blob"/>. If <see langword="null"/> then the MIME type of the original <see cref="Blob"/> is used.</param>
    /// <returns>A new <see cref="Blob"/>.</returns>
    public async Task<Blob> SliceAsync(long? start = null, long? end = null, string? contentType = null)
    {
        start ??= 0;
        end ??= (long)await GetSizeAsync();
        IJSObjectReference jSInstance = await JSReference.InvokeAsync<IJSObjectReference>("slice", start, end, contentType);
        return new Blob(JSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Creates a new <see cref="ReadableStream"/> from the <see cref="Blob"/>.
    /// </summary>
    /// <returns>A new wrapper for a <see cref="ReadableStream"/></returns>
    public async Task<ReadableStream> StreamAsync()
    {
        IJSObjectReference jSInstance = await JSReference.InvokeAsync<IJSObjectReference>("stream");
        return await ReadableStream.CreateAsync(JSRuntime, jSInstance);
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
        IJSObjectReference jSInstance = await JSReference.InvokeAsync<IJSObjectReference>("arrayBuffer");
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<byte[]>("arrayBuffer", jSInstance);
    }
}
