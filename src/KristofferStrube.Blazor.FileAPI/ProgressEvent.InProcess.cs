using KristofferStrube.Blazor.DOM;
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
    /// A helper module instance from the Blazor.FileAPI library.
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
        IJSInProcessObjectReference InProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new ProgressEventInProcess(jSRuntime, InProcessHelper, jSReference, options);
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

    #region inherited from the the in-process event

    /// <summary>
    /// Returns the type of this <see cref="Event"/>
    /// </summary>
    public string Type => InProcessHelper.Invoke<string>("getAttribute", JSReference, "type");

    /// <summary>
    /// Gets the target of this <see cref="Event"/>.
    /// </summary>
    public new async Task<EventTargetInProcess?> GetTargetAsync()
    {
        IJSInProcessObjectReference? jSInstance = await InProcessHelper.InvokeAsync<IJSInProcessObjectReference?>("getAttribute", JSReference, "target");
        return jSInstance is null ? null : await EventTargetInProcess.CreateAsync(JSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Gets the current target of this <see cref="Event"/>.
    /// </summary>
    /// <returns>The object whose event listener’s callback is currently being invoked.</returns>
    public new async Task<EventTargetInProcess?> GetCurrentTargetAsync()
    {
        IJSInProcessObjectReference? jSInstance = await InProcessHelper.InvokeAsync<IJSInProcessObjectReference?>("getAttribute", JSReference, "currentTarget");
        return jSInstance is null ? null : await EventTargetInProcess.CreateAsync(JSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Returns the invocation target objects of event’s path (objects on which listeners will be invoked), except for any nodes in shadow trees of which the shadow root’s mode is "closed" that are not reachable from event’s currentTarget.
    /// </summary>
    /// <returns>An array of <see cref="EventTargetInProcess"/>s</returns>
    public new async Task<EventTargetInProcess[]> ComposedPathAsync()
    {
        IJSObjectReference jSArray = await JSReference.InvokeAsync<IJSObjectReference>("composedPath");
        int length = await InProcessHelper.InvokeAsync<int>("getAttribute", jSArray, "length");
        return (await Task.WhenAll(Enumerable
            .Range(0, length)
            .Select(async i => await EventTargetInProcess.CreateAsync(JSRuntime, await InProcessHelper.InvokeAsync<IJSInProcessObjectReference>("getAttribute", jSArray, i)))))
            .ToArray();
    }

    /// <summary>
    /// Returns the <see cref="Event"/>'s phase.
    /// </summary>
    public EventPhase EventPhase => InProcessHelper.Invoke<EventPhase>("getAttribute", JSReference, "eventPhase");

    /// <summary>
    /// When dispatched in a tree, invoking this method prevents the event from reaching any objects other than the current object.
    /// </summary>
    /// <returns></returns>
    public void StopPropagation()
    {
        JSReference.InvokeVoid("stopPropagation");
    }

    /// <summary>
    /// Invoking this method prevents event from reaching any registered event listeners after the current one finishes running and, when dispatched in a tree, also prevents event from reaching any other objects.
    /// </summary>
    public void StopImmediatePropagation()
    {
        JSReference.InvokeVoid("stopImmediatePropagation");
    }

    /// <summary>
    /// Returns <see langword="true"/> if the event goes through its target’s ancestors in reverse tree order; otherwise <see langword="false"/>.
    /// </summary>
    public bool Bubbles => InProcessHelper.Invoke<bool>("getAttribute", JSReference, "bubbles");

    /// <summary>
    /// Its value does not always carry meaning, but <see langword="true"/> can indicate that part of the operation during which event was dispatched,can be canceled by invoking the <see cref="PreventDefault"/> method.
    /// </summary>
    public bool Cancelable => InProcessHelper.Invoke<bool>("getAttribute", JSReference, "cancelable");

    /// <summary>
    /// If invoked when the cancelable attribute value is <see langword="true"/>, and while executing a listener for the event with passive set to <see langword="false"/>, then it signals to the operation that caused event to be dispatched that it needs to be canceled.
    /// </summary>
    public void PreventDefault()
    {
        JSReference.InvokeVoid("preventDefault");
    }

    /// <summary>
    /// Returns <see langword="true"/> if <see cref="PreventDefault"/> was invoked successfully to indicate cancelation; otherwise <see langword="false"/>.
    /// </summary>
    public bool DefaultPrevented => InProcessHelper.Invoke<bool>("getAttribute", JSReference, "defaultPrevented");

    /// <summary>
    /// Returns <see langword="true"/> if the event invokes listeners past a ShadowRoot node that is the root of its target; otherwise <see langword="false"/>.
    /// </summary>
    public bool Composed => InProcessHelper.Invoke<bool>("getAttribute", JSReference, "composed");

    /// <summary>
    /// Returns <see langword="true"/> if the event was dispatched by the user agent, and <see langword="false"/> otherwise.
    /// </summary>
    public bool IsTrusted => InProcessHelper.Invoke<bool>("getAttribute", JSReference, "isTrusted");

    /// <summary>
    /// Returns the event’s timestamp as the number of milliseconds measured relative to the <see href="https://w3c.github.io/hr-time/#dfn-get-time-origin-timestamp">time origin</see>.
    /// </summary>
    public double TimeStamp => InProcessHelper.Invoke<double>("getAttribute", JSReference, "timeStamp");

    #endregion 

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        await InProcessHelper.DisposeAsync();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
