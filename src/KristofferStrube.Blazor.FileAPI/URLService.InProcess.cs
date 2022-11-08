using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#url">URL browser specs</see>
/// </summary>
public class URLServiceInProcess : URLService, IURLServiceInProcess
{
    protected new readonly IJSInProcessRuntime jSRuntime;

    /// <summary>
    /// Constructs a <see cref="URLServiceInProcess"/> that can be used to access the partial part of the URL interface defined in the FileAPI definition.
    /// </summary>
    /// <param name="jSRuntime"></param>
    public URLServiceInProcess(IJSInProcessRuntime jSRuntime) : base(jSRuntime)
    {
        this.jSRuntime = jSRuntime;
    }

    /// <summary>
    /// Creates a new Blob URL and adds that to the current contexts Blob URL store.
    /// </summary>
    /// <param name="obj">The <see cref="Blob"/> that you wish to create a URL for.</param>
    /// <returns>a Blob Url which can be used as a source in different media like image, sound, iframe sources.</returns>
    public string CreateObjectURL(Blob obj)
    {
        return jSRuntime.Invoke<string>("URL.createObjectURL", obj.JSReference);
    }

    /// <summary>
    /// Removes a specific URL from the current contexts Blob URL store.
    /// </summary>
    /// <param name="url">The URL that is to be removed.</param>
    public void RevokeObjectURL(string url)
    {
        jSRuntime.InvokeVoid("URL.revokeObjectURL", url);
    }
}
