using Microsoft.JSInterop;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#dfn-filereader">FileReader browser specs</see>
/// </summary>
public class FileReader : BaseJSWrapper
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="FileReader"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="FileReader"/>.</param>
    /// <returns>A wrapper instance for a <see cref="FileReader"/>.</returns>
    public static FileReader Create(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return new FileReader(jSRuntime, jSReference);
    }

    /// <summary>
    /// Constructs a wrapper instance using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <returns></returns>
    public static async Task<FileReader> CreateAsync(IJSRuntime jSRuntime)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructFileReader");
        var fileReader = new FileReader(jSRuntime, jSInstance);
        await helper.InvokeVoidAsync("registerEventHandlers", fileReader, jSInstance);
        return fileReader;
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="FileReader"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="FileReader"/>.</param>
    internal FileReader(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) {
        ObjRef = DotNetObjectReference.Create(this);
    }

    public DotNetObjectReference<FileReader> ObjRef { get; init; }

    public async Task ReadAsArrayBufferAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("readAsArrayBuffer", blob.JSReference);
    }

    public async Task ReadAsBinaryStringAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("readAsBinaryString", blob.JSReference);
    }

    public async Task ReadAsTextAsync(Blob blob, string? encoding = null)
    {
        await JSReference.InvokeVoidAsync("readAsText", blob.JSReference, encoding);
    }

    public async Task ReadAsDataURLAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("readAsDataURL", blob.JSReference);
    }

    public async Task AbortAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("abort", blob.JSReference);
    }

    public const ushort EMPTY = 0;
    public const ushort LOADING = 1;
    public const ushort DONE = 2;

    public async Task<ushort> GetReadyStateAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<ushort>("getAttribute", JSReference, "readyState");
    }

    /// <summary>
    /// Checks whether the result is a either a string or a byte array.
    /// </summary>
    /// <returns>Either the type of <see langword="string"/> or type of <see cref="byte"/>[].</returns>
    public async Task<Type?> GetResultTypeAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        var isArrayBuffer = await helper.InvokeAsync<bool>("isArrayBuffer", JSReference);
        return isArrayBuffer ? typeof(byte[]) : typeof(string);
    }

    public async Task<string?> GetResultAsStringAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<string>("getAttribute", JSReference, "result");
    }

    public async Task<byte[]?> GetResultAsByteArrayAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        var jSResult = await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, "result");
        return await helper.InvokeAsync<byte[]>("arrayBuffer", jSResult);
    }

    /// <summary>
    /// Gets the error object reference which will be null if no error occured.
    /// </summary>
    /// <returns>A nullable IJSObjectReference because it was out of scope to wrap the Exception API.</returns>
    public async Task<IJSObjectReference?> GetErrorAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<IJSObjectReference?>("getAttribute", JSReference, "error");
    }

    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnLoadStart { get; set; }

    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnProgress { get; set; }

    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnLoad { get; set; }

    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnAbort { get; set; }

    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnError { get; set; }

    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnLoadEnd { get; set; }

    [JSInvokable]
    public async Task InvokeOnLoadStart(IJSObjectReference jsProgressEvent)
    {
        if (OnLoadStart is null) return;
        await OnLoadStart.Invoke(new ProgressEvent(jSRuntime, jsProgressEvent));
    }

    [JSInvokable]
    public async Task InvokeOnProgress(IJSObjectReference jsProgressEvent)
    {
        if (OnProgress is null) return;
        await OnProgress.Invoke(new ProgressEvent(jSRuntime, jsProgressEvent));
    }

    [JSInvokable]
    public async Task InvokeOnLoad(IJSObjectReference jsProgressEvent)
    {
        if (OnLoad is null) return;
        await OnLoad.Invoke(new ProgressEvent(jSRuntime, jsProgressEvent));
    }

    [JSInvokable]
    public async Task InvokeOnAbort(IJSObjectReference jsProgressEvent)
    {
        if (OnAbort is null) return;
        await OnAbort.Invoke(new ProgressEvent(jSRuntime, jsProgressEvent));
    }

    [JSInvokable]
    public async Task InvokeOnError(IJSObjectReference jsProgressEvent)
    {
        if (OnError is null) return;
        await OnError.Invoke(new ProgressEvent(jSRuntime, jsProgressEvent));
    }

    [JSInvokable]
    public async Task InvokeOnLoadEnd(IJSObjectReference jsProgressEvent)
    {
        if (OnLoadEnd is null) return;
        await OnLoadEnd.Invoke(new ProgressEvent(jSRuntime, jsProgressEvent));
    }


}
