using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <inheritdoc cref="IURLService"/>
public class URLService : IURLService
{
    /// <summary>
    /// <see cref="IJSRuntime"/> used for making calls in the <see cref="URLService"/>.
    /// </summary>
    protected readonly IJSRuntime jSRuntime;

    /// <summary>
    /// Constructs a <see cref="URLService"/> that can be used to access the partial part of the URL interface defined in the FileAPI definition.
    /// </summary>
    /// <param name="jSRuntime"></param>
    public URLService(IJSRuntime jSRuntime)
    {
        this.jSRuntime = jSRuntime;
    }

    /// <inheritdoc/>
    public async Task<string> CreateObjectURLAsync(Blob obj)
    {
        return await jSRuntime.InvokeAsync<string>("URL.createObjectURL", obj.JSReference);
    }

    /// <inheritdoc/>
    public async Task RevokeObjectURLAsync(string url)
    {
        await jSRuntime.InvokeVoidAsync("URL.revokeObjectURL", url);
    }
}
