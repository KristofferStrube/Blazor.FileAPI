using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#url">URL browser specs</see>
/// </summary>
public class URLService : IURLService
{
    protected readonly IJSRuntime jSRuntime;

    /// <summary>
    /// Constructs a <see cref="URLService"/> that can be used to access the partial part of the URL interface defined in the FileAPI definition.
    /// </summary>
    /// <param name="jSRuntime"></param>
    public URLService(IJSRuntime jSRuntime)
    {
        this.jSRuntime = jSRuntime;
    }

    /// <summary>
    /// Creates a new Blob URL and adds that to the current contexts Blob URL store.
    /// </summary>
    /// <param name="obj">The <see cref="Blob"/> that you wish to create a URL for.</param>
    /// <returns>a Blob Url which can be used as a source in different media like image, sound, iframe sources.</returns>
    public async Task<string> CreateObjectURLAsync(Blob obj)
    {
        return await jSRuntime.InvokeAsync<string>("URL.createObjectURL", obj.JSReference);
    }

    /// <summary>
    /// Removes a specific URL from the current contexts Blob URL store.
    /// </summary>
    /// <param name="url">The URL that is to be removed.</param>
    public async Task RevokeObjectURLAsync(string url)
    {
        await jSRuntime.InvokeVoidAsync("URL.revokeObjectURL", url);
    }
}
