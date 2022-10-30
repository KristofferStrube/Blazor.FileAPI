using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#dfn-filereader">File browser specs</see>
/// </summary>
public class FileReader : BaseJSWrapper
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="FileReader"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="FileReader"/>.</param>
    /// <returns>A wrapper instance for a <see cref="FileReader"/>.</returns>
    public static FileReader Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return new FileReader(jSRuntime, jSReference);
    }

    /// <summary>
    /// Constructs a wrapper instance using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <returns></returns>
    public static async Task<FileReader> CreateAsync(IJSRuntime jSRuntime)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructFileReader");
        return new FileReader(jSRuntime, jSInstance);
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="FileReader"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="FileReader"/>.</param>
    internal FileReader(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }

    public async Task ReadAsArrayBufferAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("readAsArrayBuffer", blob.JSReference);
    }

    public async Task ReadAsBinaryStringAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("readAsBinaryString", blob.JSReference);
    }

    public async Task ReadAsTextAsync(Blob blob, string? encoding = null)
    {
        await JSReference.InvokeVoidAsync("readAsText", blob.JSReference, encoding);
    }

    public async Task ReadAsDataURLAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("readAsDataURL", blob.JSReference);
    }

    public async Task AbortAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("abort", blob.JSReference);
    }

    const ushort EMPTY = 0;
    const ushort LOADING = 1;
    const ushort DONE = 2;

    public async Task<ushort> GetReadyStateAsync()
    {
        var helper = await helperTask.Value;
        return await helper.InvokeAsync<ushort>("readyState");
    }
}
