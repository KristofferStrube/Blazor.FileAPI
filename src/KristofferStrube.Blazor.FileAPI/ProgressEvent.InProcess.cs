using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://xhr.spec.whatwg.org/#progressevent">ProgressEvent browser specs</see>
/// </summary>
public class ProgressEventInProcess : ProgressEvent, IJSInProcessCreatable<ProgressEventInProcess, ProgressEvent>
{
    /// <inheritdoc/>
    public new IJSInProcessObjectReference JSReference { get; }

    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.FileAPI library.
    /// </summary>
    protected readonly IJSInProcessObjectReference InProcessHelper;

    /// <inheritdoc/>
    public static async Task<ProgressEventInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static async Task<ProgressEventInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new ProgressEventInProcess(jSRuntime, inProcessHelper, jSReference, options);
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)"/>
    protected internal ProgressEventInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
        JSReference = jSReference;
        InProcessHelper = inProcessHelper;
    }

    /// <summary>
    /// Indicates whether the total can be calculated.
    /// </summary>
    /// <returns>A <see langword="bool"/> indicating if the total length was computable.</returns>
    public bool LengthComputable => InProcessHelper.Invoke<bool>("getAttribute", JSReference, "lengthComputable");

    /// <summary>
    /// The loaded number of bytes of the total.
    /// </summary>
    /// <returns>The length of the currently loaded part.</returns>
    public ulong Loaded => InProcessHelper.Invoke<ulong>("getAttribute", JSReference, "loaded");

    /// <summary>
    /// The total number of bytes if it was computable else this is <c>0</c>.
    /// </summary>
    /// <returns>The total length of the read.</returns>
    public ulong Total => InProcessHelper.Invoke<ulong>("getAttribute", JSReference, "total");
}
