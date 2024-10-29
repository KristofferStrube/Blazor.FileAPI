using KristofferStrube.Blazor.DOM;
using KristofferStrube.Blazor.DOM.Extensions;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#dfn-filereader">FileReader browser specs</see>
/// </summary>
[IJSWrapperConverter]
public class FileReaderInProcess : FileReader, IJSInProcessCreatable<FileReaderInProcess, FileReader>, IEventTargetInProcess
{
    /// <inheritdoc/>
    public new IJSInProcessObjectReference JSReference { get; set; }

    /// <summary>
    /// A helper module instance from the Blazor.FileAPI library.
    /// </summary>
    protected IJSInProcessObjectReference InProcessHelper { get; }

    /// <inheritdoc/>
    public static async Task<FileReaderInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static async Task<FileReaderInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new FileReaderInProcess(jSRuntime, inProcessHelper, jSReference, options);
    }

    /// <summary>
    /// Constructs a wrapper instance using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <returns>A wrapper instance for a <see cref="FileReader"/>.</returns>
    public static new async Task<FileReaderInProcess> CreateAsync(IJSRuntime jSRuntime)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        IJSInProcessObjectReference jSInstance = await inProcessHelper.InvokeAsync<IJSInProcessObjectReference>("constructFileReader");
        FileReaderInProcess fileReaderInProcess = new(jSRuntime, inProcessHelper, jSInstance, new() { DisposesJSReference = true });
        await inProcessHelper.InvokeVoidAsync("registerEventHandlers", DotNetObjectReference.Create(fileReaderInProcess), jSInstance);
        return fileReaderInProcess;
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)"/>
    protected FileReaderInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
        JSReference = jSReference;
        InProcessHelper = inProcessHelper;
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as an <see langword="byte"/>[] which can be read from <see cref="ResultAsByteArray"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <returns></returns>
    public void ReadAsArrayBuffer(Blob blob)
    {
        JSReference.InvokeVoid("readAsArrayBuffer", blob.JSReference);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as a binarily encoded <see langword="string"/> which can be read from <see cref="ResultAsString"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <returns></returns>
    public void ReadAsBinaryString(Blob blob)
    {
        JSReference.InvokeVoid("readAsBinaryString", blob.JSReference);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as a <see langword="string"/> which can be read from <see cref="ResultAsString"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <param name="encoding">An optional encoding for the text. The default is UTF-8.</param>
    /// <returns></returns>
    public void ReadAsText(Blob blob, string? encoding = null)
    {
        JSReference.InvokeVoid("readAsText", blob.JSReference, encoding);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as a base64 encoded Data URL which can be read from <see cref="ResultAsString"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <returns></returns>
    public void ReadAsDataURL(Blob blob)
    {
        JSReference.InvokeVoid("readAsDataURL", blob.JSReference);
    }

    /// <summary>
    /// Terminates the load if the <see cref="ReadyState"/> is <see cref="FileReader.LOADING"/> else it sets the result to <see langword="null"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> read that should be terminated.</param>
    /// <returns></returns>
    public void Abort(Blob blob)
    {
        JSReference.InvokeVoid("abort", blob.JSReference);
    }

    /// <summary>
    /// Gets the state of the <see cref="FileReader"/>.
    /// </summary>
    /// <returns>As a standard either <see cref="FileReader.EMPTY"/>, <see cref="FileReader.LOADING"/> or <see cref="FileReader.DONE"/></returns>
    public ushort ReadyState => InProcessHelper.Invoke<ushort>("getAttribute", JSReference, "readyState");

    /// <summary>
    /// Checks whether the result is a either a <see langword="string"/> or a byte array.
    /// </summary>
    /// <returns>Either the type of <see langword="string"/> or type of <see cref="byte"/>[].</returns>
    public Type? ResultType => InProcessHelper.Invoke<bool>("isArrayBuffer", JSReference) ? typeof(byte[]) : typeof(string);

    /// <summary>
    /// Gets the result of the read a <see langword="string"/>.
    /// </summary>
    /// <returns>A <see langword="string"/> representing the read. If there was no result from the read or if the read has not ended yet then it will be <see langword="null"/>.</returns>
    public string? ResultAsString => InProcessHelper.Invoke<string?>("getAttribute", JSReference, "result");

    /// <summary>
    /// Gets the result of the read a <see langword="byte"/>[].
    /// </summary>
    /// <returns>A <see langword="byte"/>[] representing the read. If there was no result from the read or if the read has not ended yet then it will be <see langword="null"/>.</returns>
    public byte[]? ResultAsByteArray => InProcessHelper.Invoke<byte[]?>("arrayBuffer", InProcessHelper.Invoke<IJSObjectReference>("getAttribute", JSReference, "result"));

    /// <summary>
    /// Gets the error object reference which will be <see langword="null"/> if no error occured.
    /// </summary>
    /// <returns>A nullable IJSObjectReference because it was out of scope to wrap the Exception API.</returns>
    public IJSObjectReference? Error => InProcessHelper.Invoke<IJSObjectReference?>("getAttribute", JSReference, "error");

    /// <summary>
    /// Invoked when a load starts.
    /// </summary>
    [Obsolete("This will be removed in the next major release in favor of AddOnLoadStartEventListener and RemoveOnLoadStartEventListener as they are more memory safe.")]
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnLoadStart { get; set; }

    /// <summary>
    /// Invoked when the progress of a load changes which includes when it ends.
    /// </summary>
    [Obsolete("This will be removed in the next major release in favor of AddOnProgressEventListener and RemoveOnProgressEventListener as they are more memory safe.")]
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnProgress { get; set; }

    /// <summary>
    /// Invoked when a load ends successfully.
    /// </summary>
    [Obsolete("This will be removed in the next major release in favor of AddOnLoadEventListener and RemoveOnLoadEventListener as they are more memory safe.")]
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnLoad { get; set; }

    /// <summary>
    /// Invoked when a load is aborted.
    /// </summary>
    [Obsolete("This will be removed in the next major release in favor of AddOnAbortEventListener and RemoveOnAbortEventListener as they are more memory safe.")]
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnAbort { get; set; }

    /// <summary>
    /// Invoked when a load fails due to an error.
    /// </summary>
    [Obsolete("This will be removed in the next major release in favor of AddOnErrorEventListener and RemoveOnErrorEventListener as they are more memory safe.")]
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnError { get; set; }

    /// <summary>
    /// Invoked when a load finishes successfully or not.
    /// </summary>
    [Obsolete("This will be removed in the next major release in favor of AddOnLoadEndEventListener and RemoveOnLoadEndEventListener as they are more memory safe.")]
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnLoadEnd { get; set; }

    /// <summary>Internal method.</summary>
    [Obsolete("This will be removed in the next major release.")]
    [JSInvokable]
    public void InvokeOnLoadStart(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnLoadStart is null)
        {
            return;
        }

        OnLoadStart.Invoke(new ProgressEventInProcess(JSRuntime, InProcessHelper, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method.</summary>
    [Obsolete("This will be removed in the next major release.")]
    [JSInvokable]
    public void InvokeOnProgress(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnProgress is null)
        {
            return;
        }

        OnProgress.Invoke(new ProgressEventInProcess(JSRuntime, InProcessHelper, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method.</summary>
    [Obsolete("This will be removed in the next major release.")]
    [JSInvokable]
    public void InvokeOnLoad(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnLoad is null)
        {
            return;
        }

        OnLoad.Invoke(new ProgressEventInProcess(JSRuntime, InProcessHelper, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method.</summary>
    [Obsolete("This will be removed in the next major release.")]
    [JSInvokable]
    public void InvokeOnAbort(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnAbort is null)
        {
            return;
        }

        OnAbort.Invoke(new ProgressEventInProcess(JSRuntime, InProcessHelper, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method.</summary>
    [Obsolete("This will be removed in the next major release.")]
    [JSInvokable]
    public void InvokeOnError(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnError is null)
        {
            return;
        }

        OnError.Invoke(new ProgressEventInProcess(JSRuntime, InProcessHelper, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method.</summary>
    [Obsolete("This will be removed in the next major release.")]
    [JSInvokable]
    public void InvokeOnLoadEnd(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnLoadEnd is null)
        {
            return;
        }

        OnLoadEnd.Invoke(new ProgressEventInProcess(JSRuntime, InProcessHelper, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <inheritdoc/>
    public void AddEventListener<TInProcessEvent, TEvent>(string type, EventListenerInProcess<TInProcessEvent, TEvent>? callback, AddEventListenerOptions? options = null)
         where TEvent : Event, IJSCreatable<TEvent> where TInProcessEvent : IJSInProcessCreatable<TInProcessEvent, TEvent>
    {
        JSReference.InvokeVoid("addEventListener", type, callback?.JSReference, options);
    }

    /// <inheritdoc/>
    public void AddEventListener<TInProcessEvent, TEvent>(EventListenerInProcess<TInProcessEvent, TEvent>? callback, AddEventListenerOptions? options = null)
         where TEvent : Event, IJSCreatable<TEvent> where TInProcessEvent : IJSInProcessCreatable<TInProcessEvent, TEvent>
    {
        JSReference.InvokeVoid("addEventListener", typeof(TEvent).Name, callback?.JSReference, options);
    }

    /// <inheritdoc/>
    public void RemoveEventListener<TInProcessEvent, TEvent>(string type, EventListenerInProcess<TInProcessEvent, TEvent>? callback, EventListenerOptions? options = null)
         where TEvent : Event, IJSCreatable<TEvent> where TInProcessEvent : IJSInProcessCreatable<TInProcessEvent, TEvent>
    {
        JSReference.InvokeVoid("removeEventListener", type, callback?.JSReference, options);
    }

    /// <inheritdoc/>
    public void RemoveEventListener<TInProcessEvent, TEvent>(EventListenerInProcess<TInProcessEvent, TEvent>? callback, EventListenerOptions? options = null)
         where TEvent : Event, IJSCreatable<TEvent> where TInProcessEvent : IJSInProcessCreatable<TInProcessEvent, TEvent>
    {
        JSReference.InvokeVoid("removeEventListener", typeof(TEvent).Name, callback?.JSReference, options);
    }

    /// <summary>
    /// Adds an <see cref="EventListenerInProcess{TInProcessEvent, TEvent}"/> for when a load is started.
    /// </summary>
    /// <param name="callback">Callback that will be invoked when the event is dispatched.</param>
    /// <param name="options"><inheritdoc cref="AddEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, AddEventListenerOptions?)" path="/param[@name='options']"/></param>
    public void AddOnLoadStartEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, AddEventListenerOptions? options = null)
    {
        AddEventListener("loadstart", callback, options);
    }

    /// <summary>
    /// Removes the event listener from the event listener list if it has been parsed to <see cref="AddOnLoadStartEventListener"/> previously.
    /// </summary>
    /// <param name="callback">The callback <see cref="EventListener{TEvent}"/> that you want to stop listening to events.</param>
    /// <param name="options"><inheritdoc cref="RemoveEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, EventListenerOptions?)" path="/param[@name='options']"/></param>
    public void RemoveOnLoadStartEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, EventListenerOptions? options = null)
    {
        RemoveEventListener("loadstart", callback, options);
    }

    /// <summary>
    /// Adds an <see cref="EventListenerInProcess{TInProcessEvent, TEvent}"/> for when the progress of a load changes which includes when it ends.
    /// </summary>
    /// <param name="callback">Callback that will be invoked when the event is dispatched.</param>
    /// <param name="options"><inheritdoc cref="AddEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, AddEventListenerOptions?)" path="/param[@name='options']"/></param>
    public void AddOnProgressEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, AddEventListenerOptions? options = null)
    {
        AddEventListener("progress", callback, options);
    }

    /// <summary>
    /// Removes the event listener from the event listener list if it has been parsed to <see cref="AddOnProgressEventListener"/> previously.
    /// </summary>
    /// <param name="callback">The callback <see cref="EventListener{TEvent}"/> that you want to stop listening to events.</param>
    /// <param name="options"><inheritdoc cref="RemoveEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, EventListenerOptions?)" path="/param[@name='options']"/></param>
    public void RemoveOnProgressEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, EventListenerOptions? options = null)
    {
        RemoveEventListener("progress", callback, options);
    }

    /// <summary>
    /// Adds an <see cref="EventListenerInProcess{TInProcessEvent, TEvent}"/> for when a load ends successfully.
    /// </summary>
    /// <param name="callback">Callback that will be invoked when the event is dispatched.</param>
    /// <param name="options"><inheritdoc cref="AddEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, AddEventListenerOptions?)" path="/param[@name='options']"/></param>
    public void AddOnLoadEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, AddEventListenerOptions? options = null)
    {
        AddEventListener("load", callback, options);
    }

    /// <summary>
    /// Removes the event listener from the event listener list if it has been parsed to <see cref="AddOnLoadEventListener"/> previously.
    /// </summary>
    /// <param name="callback">The callback <see cref="EventListener{TEvent}"/> that you want to stop listening to events.</param>
    /// <param name="options"><inheritdoc cref="RemoveEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, EventListenerOptions?)" path="/param[@name='options']"/></param>
    public void RemoveOnLoadEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, EventListenerOptions? options = null)
    {
        RemoveEventListener("load", callback, options);
    }

    /// <summary>
    /// Adds an <see cref="EventListenerInProcess{TInProcessEvent, TEvent}"/> for when a load is aborted.
    /// </summary>
    /// <param name="callback">Callback that will be invoked when the event is dispatched.</param>
    /// <param name="options"><inheritdoc cref="AddEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, AddEventListenerOptions?)" path="/param[@name='options']"/></param>
    public void AddOnAbortEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, AddEventListenerOptions? options = null)
    {
        AddEventListener("abort", callback, options);
    }

    /// <summary>
    /// Removes the event listener from the event listener list if it has been parsed to <see cref="AddOnAbortEventListener"/> previously.
    /// </summary>
    /// <param name="callback">The callback <see cref="EventListener{TEvent}"/> that you want to stop listening to events.</param>
    /// <param name="options"><inheritdoc cref="RemoveEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, EventListenerOptions?)" path="/param[@name='options']"/></param>
    public void RemoveOnAbortEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, EventListenerOptions? options = null)
    {
        RemoveEventListener("abort", callback, options);
    }

    /// <summary>
    /// Adds an <see cref="EventListenerInProcess{TInProcessEvent, TEvent}"/> for when a load fails due to an error.
    /// </summary>
    /// <param name="callback">Callback that will be invoked when the event is dispatched.</param>
    /// <param name="options"><inheritdoc cref="AddEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, AddEventListenerOptions?)" path="/param[@name='options']"/></param>
    public void AddOnErrorEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, AddEventListenerOptions? options = null)
    {
        AddEventListener("error", callback, options);
    }

    /// <summary>
    /// Removes the event listener from the event listener list if it has been parsed to <see cref="AddOnErrorEventListener"/> previously.
    /// </summary>
    /// <param name="callback">The callback <see cref="EventListener{TEvent}"/> that you want to stop listening to events.</param>
    /// <param name="options"><inheritdoc cref="RemoveEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, EventListenerOptions?)" path="/param[@name='options']"/></param>
    public void RemoveOnErrorEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, EventListenerOptions? options = null)
    {
        RemoveEventListener("error", callback, options);
    }

    /// <summary>
    /// Adds an <see cref="EventListenerInProcess{TInProcessEvent, TEvent}"/> for when a load finishes successfully or not.
    /// </summary>
    /// <param name="callback">Callback that will be invoked when the event is dispatched.</param>
    /// <param name="options"><inheritdoc cref="AddEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, AddEventListenerOptions?)" path="/param[@name='options']"/></param>
    public void AddOnLoadEndEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, AddEventListenerOptions? options = null)
    {
        AddEventListener("loadend", callback, options);
    }

    /// <summary>
    /// Removes the event listener from the event listener list if it has been parsed to <see cref="AddOnLoadEndEventListener"/> previously.
    /// </summary>
    /// <param name="callback">The callback <see cref="EventListener{TEvent}"/> that you want to stop listening to events.</param>
    /// <param name="options"><inheritdoc cref="RemoveEventListener{TInProcessEvent, TEvent}(string, EventListenerInProcess{TInProcessEvent, TEvent}?, EventListenerOptions?)" path="/param[@name='options']"/></param>
    public void RemoveOnLoadEndEventListener(EventListenerInProcess<ProgressEventInProcess, ProgressEvent> callback, EventListenerOptions? options = null)
    {
        RemoveEventListener("loadend", callback, options);
    }

    /// <inheritdoc/>
    public bool DispatchEvent(Event eventInstance)
    {
        return IEventTargetInProcessExtensions.DispatchEvent(this, eventInstance);
    }

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        await InProcessHelper.DisposeAsync();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
