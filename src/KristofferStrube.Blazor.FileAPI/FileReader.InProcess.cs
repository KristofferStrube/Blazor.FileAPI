using Microsoft.JSInterop;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#dfn-filereader">FileReader browser specs</see>
/// </summary>
public class FileReaderInProcess : FileReader
{
    public new IJSInProcessObjectReference JSReference;
    protected readonly IJSInProcessObjectReference inProcessHelper;
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="FileReader"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="FileReader"/>.</param>
    /// <returns>A wrapper instance for a <see cref="FileReader"/>.</returns>
    public static async Task<FileReaderInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new FileReaderInProcess(jSRuntime, inProcessHelper, jSReference);
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
        FileReaderInProcess fileReaderInProcess = new FileReaderInProcess(jSRuntime, inProcessHelper, jSInstance);
        await inProcessHelper.InvokeVoidAsync("registerEventHandlers", DotNetObjectReference.Create(fileReaderInProcess), jSInstance);
        return fileReaderInProcess;
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="File"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="inProcessHelper">An in process helper instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="File"/>.</param>
    internal FileReaderInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference) : base(jSRuntime, jSReference)
    {
        this.inProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as an <see langword="byte"/>[] which can be read from <see cref="GetResultAsByteArrayAsync"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <returns></returns>
    public void ReadAsArrayBuffer(Blob blob)
    {
        JSReference.InvokeVoid("readAsArrayBuffer", blob.JSReference);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as a binarily encoded <see langword="string"/> which can be read from <see cref="GetResultAsStringAsync"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <returns></returns>
    public void ReadAsBinaryString(Blob blob)
    {
        JSReference.InvokeVoid("readAsBinaryString", blob.JSReference);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as a <see langword="string"/> which can be read from <see cref="GetResultAsStringAsync"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <param name="encoding">An optional encoding for the text. The default is UTF-8.</param>
    /// <returns></returns>
    public void ReadAsText(Blob blob, string? encoding = null)
    {
        JSReference.InvokeVoid("readAsText", blob.JSReference, encoding);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as a base64 encoded Data URL which can be read from <see cref="GetResultAsStringAsync"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <returns></returns>
    public void ReadAsDataURL(Blob blob)
    {
        JSReference.InvokeVoid("readAsDataURL", blob.JSReference);
    }

    /// <summary>
    /// Terminates the load if the <see cref="GetReadyStateAsync"/> is <see cref="LOADING"/> else it sets the result to <see langword="null"/>.
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
    /// <returns>As a standard either <see cref="EMPTY"/>, <see cref="LOADING"/> or <see cref="DONE"/></returns>
    public ushort ReadyState => inProcessHelper.Invoke<ushort>("getAttribute", JSReference, "readyState");

    /// <summary>
    /// Checks whether the result is a either a <see langword="string"/> or a byte array.
    /// </summary>
    /// <returns>Either the type of <see langword="string"/> or type of <see cref="byte"/>[].</returns>
    public Type? ResultType => inProcessHelper.Invoke<bool>("isArrayBuffer", JSReference) ? typeof(byte[]) : typeof(string);

    /// <summary>
    /// Gets the result of the read a <see langword="string"/>.
    /// </summary>
    /// <returns>A <see langword="string"/> representing the read. If there was no result from the read or if the read has not ended yet then it will be <see langword="null"/>.</returns>
    public string? ResultAsString => inProcessHelper.Invoke<string?>("getAttribute", JSReference, "result");

    /// <summary>
    /// Gets the result of the read a <see langword="byte"/>[].
    /// </summary>
    /// <returns>A <see langword="byte"/>[] representing the read. If there was no result from the read or if the read has not ended yet then it will be <see langword="null"/>.</returns>
    public byte[]? ResultAsByteArray => inProcessHelper.Invoke<byte[]?>("arrayBuffer", inProcessHelper.Invoke<IJSObjectReference>("getAttribute", JSReference, "result"));

    /// <summary>
    /// Gets the error object reference which will be <see langword="null"/> if no error occured.
    /// </summary>
    /// <returns>A nullable IJSObjectReference because it was out of scope to wrap the Exception API.</returns>
    public IJSObjectReference? Error => inProcessHelper.Invoke<IJSObjectReference?>("getAttribute", JSReference, "error");

    /// <summary>
    /// Invoked when a load starts.
    /// </summary>
    public new Action<ProgressEventInProcess>? OnLoadStart { get; set; }

    /// <summary>
    /// Invoked when the progress of a load changes which includes when it ends.
    /// </summary>
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnProgress { get; set; }

    /// <summary>
    /// Invoked when a load ends successfully.
    /// </summary>
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnLoad { get; set; }

    /// <summary>
    /// Invoked when a load is aborted.
    /// </summary>
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnAbort { get; set; }

    /// <summary>
    /// Invoked when a load fails due to an error.
    /// </summary>
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnError { get; set; }

    /// <summary>
    /// Invoked when a load finishes successfully or not.
    /// </summary>
    [JsonIgnore]
    public new Action<ProgressEventInProcess>? OnLoadEnd { get; set; }

    [JSInvokable]
    public void InvokeOnLoadStart(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnLoadStart is null)
        {
            return;
        }

        OnLoadStart.Invoke(new ProgressEventInProcess(jSRuntime, inProcessHelper, jsProgressEvent));
    }

    [JSInvokable]
    public void InvokeOnProgress(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnProgress is null)
        {
            return;
        }

        OnProgress.Invoke(new ProgressEventInProcess(jSRuntime, inProcessHelper, jsProgressEvent));
    }

    [JSInvokable]
    public void InvokeOnLoad(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnLoad is null)
        {
            return;
        }

        OnLoad.Invoke(new ProgressEventInProcess(jSRuntime, inProcessHelper, jsProgressEvent));
    }

    [JSInvokable]
    public void InvokeOnAbort(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnAbort is null)
        {
            return;
        }

        OnAbort.Invoke(new ProgressEventInProcess(jSRuntime, inProcessHelper, jsProgressEvent));
    }

    [JSInvokable]
    public void InvokeOnError(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnError is null)
        {
            return;
        }

        OnError.Invoke(new ProgressEventInProcess(jSRuntime, inProcessHelper, jsProgressEvent));
    }

    [JSInvokable]
    public void InvokeOnLoadEnd(IJSInProcessObjectReference jsProgressEvent)
    {
        if (OnLoadEnd is null)
        {
            return;
        }

        OnLoadEnd.Invoke(new ProgressEventInProcess(jSRuntime, inProcessHelper, jsProgressEvent));
    }
}
