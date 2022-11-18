using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#file-section">File browser specs</see>
/// </summary>
public class File : Blob
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="File"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="File"/>.</param>
    /// <returns>A wrapper instance for a <see cref="File"/>.</returns>
    public static new File Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return new File(jSRuntime, jSReference);
    }

    /// <summary>
    /// Constructs a wrapper instance using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="fileBits">The bits that will make the new <see cref="File"/>.</param>
    /// <param name="fileName">The name of the new file.</param>
    /// <param name="options">Options for constructing the new Blob which includes MIME type, line endings, and last modified date.</param>
    /// <returns></returns>
    public static async Task<File> CreateAsync(IJSRuntime jSRuntime, IList<BlobPart> fileBits, string fileName, FilePropertyBag? options = null)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        object?[]? jsFileBits = fileBits.Select<BlobPart, object?>(blobPart => blobPart.Part switch
            {
                byte[] part => part,
                Blob part => part.JSReference,
                _ => blobPart.Part
            })
            .ToArray();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructFile", jsFileBits, fileName, options);
        return new File(jSRuntime, jSInstance);
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="File"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="File"/>.</param>
    internal File(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }

    /// <summary>
    /// The name of the file including file extension.
    /// </summary>
    /// <returns>The file name.</returns>
    public async Task<string> GetNameAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<string>("getAttribute", JSReference, "name");
    }

    /// <summary>
    /// The time that the file was last modified.
    /// </summary>
    /// <returns>A new <see cref="DateTime"/> object representing when the file was last modified.</returns>
    public async Task<DateTime> GetLastModifiedAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return DateTime.UnixEpoch.AddMilliseconds(await helper.InvokeAsync<long>("getAttribute", JSReference, "lastModified"));
    }
}
