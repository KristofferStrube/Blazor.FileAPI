using KristofferStrube.Blazor.DOM;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://xhr.spec.whatwg.org/#progressevent">ProgressEvent browser specs</see>
/// </summary>
public class ProgressEvent : Event, IJSCreatable<ProgressEvent>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.FileAPI library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> fileApiHelperTask;

    /// <inheritdoc/>
    public static new async Task<ProgressEvent> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static new Task<ProgressEvent> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new ProgressEvent(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected internal ProgressEvent(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
        fileApiHelperTask = new(jSRuntime.GetHelperAsync);
    }

    /// <summary>
    /// Indicates whether the total can be calculated.
    /// </summary>
    /// <returns>A <see langword="bool"/> indicating if the total length was computable.</returns>
    public async Task<bool> GetLengthComputableAsync()
    {
        IJSObjectReference helper = await fileApiHelperTask.Value;
        return await helper.InvokeAsync<bool>("getAttribute", JSReference, "lengthComputable");
    }

    /// <summary>
    /// The loaded number of bytes of the total.
    /// </summary>
    /// <returns>The length of the currently loaded part.</returns>
    public async Task<ulong> GetLoadedAsync()
    {
        IJSObjectReference helper = await fileApiHelperTask.Value;
        return await helper.InvokeAsync<ulong>("getAttribute", JSReference, "loaded");
    }

    /// <summary>
    /// The total number of bytes if it was computable else this is <c>0</c>.
    /// </summary>
    /// <returns>The total length of the read.</returns>
    public async Task<ulong> GetTotalAsync()
    {
        IJSObjectReference helper = await fileApiHelperTask.Value;
        return await helper.InvokeAsync<ulong>("getAttribute", JSReference, "total");
    }

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        if (fileApiHelperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
