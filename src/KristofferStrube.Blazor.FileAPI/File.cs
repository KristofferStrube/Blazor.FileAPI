using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// A File object is a <see cref="Blob"/> object with a <see cref="GetNameAsync"/> attribute, which is a string;
/// it can be created within the web application via a constructor,
/// or is a reference to a byte sequence from a file from the underlying (OS) file system.
/// </summary>
/// <remarks><see href="https://www.w3.org/TR/FileAPI/#file-section">See the API definition here</see></remarks>
[IJSWrapperConverter]
public class File : Blob, IJSCreatable<File>
{
    /// <inheritdoc/>
    public static new async Task<File> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static new Task<File> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new File(jSRuntime, jSReference, options));
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="File"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="File"/>.</param>
    /// <returns>A wrapper instance for a <see cref="File"/>.</returns>
    [Obsolete("This will be removed in the next major release as all creator methods should be asynchronous for uniformity. Use CreateAsync instead.")]
    public static new File Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return new File(jSRuntime, jSReference, new());
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
        object?[]? jsFileBits = fileBits.Select(blobPart => blobPart.Part switch
            {
                byte[] part => part,
                Blob part => part.JSReference,
                _ => blobPart.Part
            })
            .ToArray();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructFile", jsFileBits, fileName, options);
        return new File(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected File(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }

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
