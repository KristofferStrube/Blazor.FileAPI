using KristofferStrube.Blazor.FileAPI.Options;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

internal static class IJSRuntimeExtensions
{
    internal static async Task<IJSObjectReference> GetHelperAsync(this IJSRuntime jSRuntime, FileApiOptions? options = null)
    {
        options ??= FileApiOptions.DefaultInstance;
        return await jSRuntime.InvokeAsync<IJSObjectReference>(
            "import", options.FullScriptPath);
    }
    internal static async Task<IJSInProcessObjectReference> GetInProcessHelperAsync(this IJSRuntime jSRuntime, FileApiOptions? options = null)
    {
        options ??= FileApiOptions.DefaultInstance;
        return await jSRuntime.InvokeAsync<IJSInProcessObjectReference>(
            "import", options.FullScriptPath);
    }
}
