using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://xhr.spec.whatwg.org/#progressevent">File browser specs</see>
/// </summary>
public class ProgressEvent : BaseJSWrapper
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="ProgressEvent"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="ProgressEvent"/>.</param>
    internal ProgressEvent(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }

    public async Task<bool> GetLengthComputableAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<bool>("getAttribute", JSReference, "lengthComputable");
    }

    public async Task<ulong> GetLoadedAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<ulong>("getAttribute", JSReference, "loaded");
    }

    public async Task<ulong> GetTotalAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<ulong>("getAttribute", JSReference, "total");
    }
}
