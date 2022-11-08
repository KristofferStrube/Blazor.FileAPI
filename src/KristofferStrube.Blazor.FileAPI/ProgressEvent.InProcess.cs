using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://xhr.spec.whatwg.org/#progressevent">ProgressEvent browser specs</see>
/// </summary>
public class ProgressEventInProcess : ProgressEvent
{
    public new IJSInProcessObjectReference JSReference;
    protected readonly IJSInProcessObjectReference inProcessHelper;

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="ProgressEvent"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="inProcessHelper">An in process helper instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="ProgressEvent"/>.</param>
    internal ProgressEventInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference) : base(jSRuntime, jSReference)
    {
        this.inProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    /// <summary>
    /// Indicates whether the total can be calculated.
    /// </summary>
    /// <returns>A <see langword="bool"/> indicating if the total length was computable.</returns>
    public bool LengthComputable => inProcessHelper.Invoke<bool>("getAttribute", JSReference, "lengthComputable");

    /// <summary>
    /// The loaded number of bytes of the total.
    /// </summary>
    /// <returns>The length of the currently loaded part.</returns>
    public ulong Loaded => inProcessHelper.Invoke<ulong>("getAttribute", JSReference, "loaded");

    /// <summary>
    /// The total number of bytes if it was computable else this is <c>0</c>.
    /// </summary>
    /// <returns>The total length of the read.</returns>
    public ulong Total => inProcessHelper.Invoke<ulong>("getAttribute", JSReference, "total");
}
