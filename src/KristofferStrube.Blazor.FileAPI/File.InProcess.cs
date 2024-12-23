﻿using KristofferStrube.Blazor.Streams;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <inheritdoc/>
[IJSWrapperConverter]
public class FileInProcess : File, IJSInProcessCreatable<FileInProcess, File>
{
    /// <inheritdoc/>
    public new IJSInProcessObjectReference JSReference { get; }

    /// <summary>
    /// A helper module instance from the Blazor.FileAPI library.
    /// </summary>
    protected IJSInProcessObjectReference InProcessHelper { get; }

    /// <inheritdoc/>
    public static async Task<FileInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static async Task<FileInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new FileInProcess(jSRuntime, inProcessHelper, jSReference, options);
    }

    /// <summary>
    /// Constructs a wrapper instance using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="fileBits">The bits that will make the new <see cref="File"/>.</param>
    /// <param name="fileName">The name of the new file.</param>
    /// <param name="options">Options for constructing the new Blob which includes MIME type, line endings, and last modified date.</param>
    /// <returns></returns>
    public static new async Task<FileInProcess> CreateAsync(IJSRuntime jSRuntime, IList<BlobPart> fileBits, string fileName, FilePropertyBag? options = null)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        object?[]? jsFileBits = fileBits.Select(blobPart => blobPart.Part switch
            {
                byte[] part => part,
                Blob part => part.JSReference,
                _ => blobPart.Part
            })
            .ToArray();
        IJSInProcessObjectReference jSInstance = await inProcessHelper.InvokeAsync<IJSInProcessObjectReference>("constructFile", jsFileBits, fileName, options);
        return new FileInProcess(jSRuntime, inProcessHelper, jSInstance, new() { DisposesJSReference = true });
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)"/>
    protected FileInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
        JSReference = jSReference;
        InProcessHelper = inProcessHelper;
    }

    /// <summary>
    /// Creates a new <see cref="ReadableStreamInProcess"/> from the <see cref="Blob"/>.
    /// </summary>
    /// <returns>A new wrapper for a <see cref="ReadableStreamInProcess"/></returns>
    public new async Task<ReadableStreamInProcess> StreamAsync()
    {
        IJSInProcessObjectReference jSInstance = JSReference.Invoke<IJSInProcessObjectReference>("stream");
        return await ReadableStreamInProcess.CreateAsync(JSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// The size of this blob.
    /// </summary>
    /// <returns>A <see langword="ulong"/> representing the size of the blob in bytes.</returns>
    public ulong Size => InProcessHelper.Invoke<ulong>("getAttribute", JSReference, "size");

    /// <summary>
    /// The media type of this blob. This is either a parseable MIME type or an empty string.
    /// </summary>
    /// <returns>The MIME type of this blob.</returns>
    public string Type => InProcessHelper.Invoke<string>("getAttribute", JSReference, "type");

    /// <summary>
    /// Gets some range of the content of a <see cref="Blob"/> as a new <see cref="Blob"/>.
    /// </summary>
    /// <param name="start">The start index of the range. If <see langword="null"/> or negative then <c>0</c> is assumed.</param>
    /// <param name="end">The start index of the range. If <see langword="null"/> or larger than the size of the original <see cref="Blob"/> then the size of the original <see cref="Blob"/> is assumed.</param>
    /// <param name="contentType">An optional MIME type of the new <see cref="Blob"/>. If <see langword="null"/> then the MIME type of the original <see cref="Blob"/> is used.</param>
    /// <returns>A new <see cref="BlobInProcess"/>.</returns>
    public BlobInProcess Slice(long? start = null, long? end = null, string? contentType = null)
    {
        start ??= 0;
        end ??= (long)Size;
        IJSInProcessObjectReference jSInstance = JSReference.Invoke<IJSInProcessObjectReference>("slice", start, end, contentType);
        return new BlobInProcess(JSRuntime, InProcessHelper, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// The name of the file including file extension.
    /// </summary>
    /// <returns>The file name.</returns>
    public string Name => InProcessHelper.Invoke<string>("getAttribute", JSReference, "name");

    /// <summary>
    /// The time that the file was last modified.
    /// </summary>
    /// <returns>A new <see cref="DateTime"/> object representing when the file was last modified.</returns>
    public DateTime LastModified => DateTime.UnixEpoch.AddMilliseconds(InProcessHelper.Invoke<ulong>("getAttribute", JSReference, "lastModified"));

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        await InProcessHelper.DisposeAsync();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
