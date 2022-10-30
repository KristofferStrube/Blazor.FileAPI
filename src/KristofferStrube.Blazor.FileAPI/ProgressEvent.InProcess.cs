using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://xhr.spec.whatwg.org/#progressevent">File browser specs</see>
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

    public bool LengthComputable => inProcessHelper.Invoke<bool>("getAttribute", JSReference, "lengthComputable");

    public ulong Loaded => inProcessHelper.Invoke<ulong>("getAttribute", JSReference, "loaded");

    public ulong Total => inProcessHelper.Invoke<ulong>("getAttribute", JSReference, "total");
}
