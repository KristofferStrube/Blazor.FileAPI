using KristofferStrube.Blazor.Streams;
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
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="blobParts">The parts that will make the new <see cref="Blob"/>.</param>
    /// <param name="options">Options for constructing the new Blob which includes MIME type and line endings settings.</param>
    /// <returns></returns>
    public static async Task<Blob> CreateAsync(IJSRuntime jSRuntime, IList<BlobPart>? blobParts = null, BlobPropertyBag? options = null)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        object?[]? jsBlobParts = blobParts?.Select<BlobPart, object?>(blobPart => blobPart.type switch
            {
                BlobPartType.BufferSource => blobPart.byteArrayPart,
                BlobPartType.Blob => blobPart.stringPart,
                _ => blobPart.blobPart?.JSReference
            })
            .ToArray();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructBlob", jsBlobParts, options);
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

    public async Task<Blob> SliceAsync(long? start = null, long? end = null, string? contentType = null)
    {
        start ??= 0;
        end ??= (long)await GetSizeAsync();
        IJSObjectReference jSInstance = await JSReference.InvokeAsync<IJSObjectReference>("slice", start, end, contentType);
        return new Blob(jSRuntime, jSInstance);
    }

    public async Task<ReadableStream> StreamAsync()
    {
        IJSObjectReference jSInstance = await JSReference.InvokeAsync<IJSObjectReference>("stream");
        return ReadableStream.Create(jSRuntime, jSInstance);
    }

    public async Task<ReadableStreamInProcess> StreamInProcessAsync()
    {
        IJSInProcessObjectReference jSInstance = await JSReference.InvokeAsync<IJSInProcessObjectReference>("stream");
        return await ReadableStreamInProcess.CreateAsync(jSRuntime, jSInstance);
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
        IJSInProcessObjectReference jSInstance = await JSReference.InvokeAsync<IJSInProcessObjectReference>("arrayBuffer");
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<byte[]>("arrayBuffer", jSInstance);
    }
}
