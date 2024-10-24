﻿using KristofferStrube.Blazor.DOM;
using KristofferStrube.Blazor.WebIDL;
using Microsoft.JSInterop;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#dfn-filereader">FileReader browser specs</see>
/// </summary>
[IJSWrapperConverter]
public class FileReader : EventTarget, IJSCreatable<FileReader>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.FileAPI library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> fileApiHelperTask;

    /// <inheritdoc/>
    public static new async Task<FileReader> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static new async Task<FileReader> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        FileReader fileReader = new(jSRuntime, jSReference, options);
        await helper.InvokeVoidAsync("registerEventHandlersAsync", DotNetObjectReference.Create(fileReader), jSReference);
        return fileReader;
    }

    /// <summary>
    /// Constructs a wrapper instance using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <returns>A wrapper instance for a <see cref="FileReader"/>.</returns>
    public static new async Task<FileReader> CreateAsync(IJSRuntime jSRuntime)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructFileReader");
        FileReader fileReader = new(jSRuntime, jSInstance, new() { DisposesJSReference = true });
        await helper.InvokeVoidAsync("registerEventHandlersAsync", DotNetObjectReference.Create(fileReader), jSInstance);
        return fileReader;
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected FileReader(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
        fileApiHelperTask = new(jSRuntime.GetHelperAsync);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as an <see langword="byte"/>[] which can be read from <see cref="GetResultAsByteArrayAsync"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <returns></returns>
    public async Task ReadAsArrayBufferAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("readAsArrayBuffer", blob.JSReference);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as a binarily encoded <see langword="string"/> which can be read from <see cref="GetResultAsStringAsync"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <returns></returns>
    public async Task ReadAsBinaryStringAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("readAsBinaryString", blob.JSReference);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as a <see langword="string"/> which can be read from <see cref="GetResultAsStringAsync"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <param name="encoding">An optional encoding for the text. The default is UTF-8.</param>
    /// <returns></returns>
    public async Task ReadAsTextAsync(Blob blob, string? encoding = null)
    {
        await JSReference.InvokeVoidAsync("readAsText", blob.JSReference, encoding);
    }

    /// <summary>
    /// Starts a new read for some <see cref="Blob"/> as a base64 encoded Data URL which can be read from <see cref="GetResultAsStringAsync"/> once the load has ended which can be checked by setting the action <see cref="OnLoadEnd"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> that should be read asynchronously.</param>
    /// <returns></returns>
    public async Task ReadAsDataURLAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("readAsDataURL", blob.JSReference);
    }

    /// <summary>
    /// Terminates the load if the <see cref="GetReadyStateAsync"/> is <see cref="LOADING"/> else it sets the result to <see langword="null"/>.
    /// </summary>
    /// <param name="blob">The <see cref="Blob"/> read that should be terminated.</param>
    /// <returns></returns>
    public async Task AbortAsync(Blob blob)
    {
        await JSReference.InvokeVoidAsync("abort", blob.JSReference);
    }

    /// <summary>
    /// The value returned by <see cref="GetReadyStateAsync"/> if the state of the <see cref="FileReader"/> is empty.
    /// </summary>
    public const ushort EMPTY = 0;

    /// <summary>
    /// The value returned by <see cref="GetReadyStateAsync"/> if the state of the <see cref="FileReader"/> is loading.
    /// </summary>
    public const ushort LOADING = 1;

    /// <summary>
    /// The value returned by <see cref="GetReadyStateAsync"/> if the state of the <see cref="FileReader"/> is done.
    /// </summary>
    public const ushort DONE = 2;

    /// <summary>
    /// Gets the state of the <see cref="FileReader"/>.
    /// </summary>
    /// <returns>As a standard either <see cref="EMPTY"/>, <see cref="LOADING"/> or <see cref="DONE"/></returns>
    public async Task<ushort> GetReadyStateAsync()
    {
        IJSObjectReference helper = await fileApiHelperTask.Value;
        return await helper.InvokeAsync<ushort>("getAttribute", JSReference, "readyState");
    }

    /// <summary>
    /// Checks whether the result is a either a <see langword="string"/> or a byte array.
    /// </summary>
    /// <returns>Either the type of <see langword="string"/> or type of <see cref="byte"/>[].</returns>
    public async Task<Type?> GetResultTypeAsync()
    {
        IJSObjectReference helper = await fileApiHelperTask.Value;
        bool isArrayBuffer = await helper.InvokeAsync<bool>("isArrayBuffer", JSReference);
        return isArrayBuffer ? typeof(byte[]) : typeof(string);
    }

    /// <summary>
    /// Gets the result of the read a <see langword="string"/>.
    /// </summary>
    /// <returns>A <see langword="string"/> representing the read. If there was no result from the read or if the read has not ended yet then it will be <see langword="null"/>.</returns>
    public async Task<string?> GetResultAsStringAsync()
    {
        IJSObjectReference helper = await fileApiHelperTask.Value;
        return await helper.InvokeAsync<string?>("getAttribute", JSReference, "result");
    }

    /// <summary>
    /// Gets the result of the read a <see langword="byte"/>[].
    /// </summary>
    /// <returns>A <see langword="byte"/>[] representing the read. If there was no result from the read or if the read has not ended yet then it will be <see langword="null"/>.</returns>
    public async Task<byte[]?> GetResultAsByteArrayAsync()
    {
        IJSObjectReference helper = await fileApiHelperTask.Value;
        IJSObjectReference jSResult = await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, "result");
        return await helper.InvokeAsync<byte[]?>("arrayBuffer", jSResult);
    }

    /// <summary>
    /// Gets the error object reference which will be <see langword="null"/> if no error occured.
    /// </summary>
    /// <returns>A nullable IJSObjectReference because it was out of scope to wrap the Exception API.</returns>
    public async Task<IJSObjectReference?> GetErrorAsync()
    {
        IJSObjectReference helper = await fileApiHelperTask.Value;
        return await helper.InvokeAsync<IJSObjectReference?>("getAttribute", JSReference, "error");
    }

    /// <summary>
    /// Invoked when a load starts.
    /// </summary>
    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnLoadStart { get; set; }

    /// <summary>
    /// Invoked when the progress of a load changes which includes when it ends.
    /// </summary>
    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnProgress { get; set; }

    /// <summary>
    /// Invoked when a load ends successfully.
    /// </summary>
    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnLoad { get; set; }

    /// <summary>
    /// Invoked when a load is aborted.
    /// </summary>
    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnAbort { get; set; }

    /// <summary>
    /// Invoked when a load fails due to an error.
    /// </summary>
    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnError { get; set; }

    /// <summary>
    /// Invoked when a load finishes successfully or not.
    /// </summary>
    [JsonIgnore]
    public Func<ProgressEvent, Task>? OnLoadEnd { get; set; }

    /// <summary>Internal method that will be removed in next major release.</summary>
    [JSInvokable]
    public async Task InvokeOnLoadStartAsync(IJSObjectReference jsProgressEvent)
    {
        if (OnLoadStart is null)
        {
            return;
        }

        await OnLoadStart.Invoke(new ProgressEvent(JSRuntime, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method that will be removed in next major release.</summary>
    [JSInvokable]
    public async Task InvokeOnProgressAsync(IJSObjectReference jsProgressEvent)
    {
        if (OnProgress is null)
        {
            return;
        }

        await OnProgress.Invoke(new ProgressEvent(JSRuntime, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method that will be removed in next major release.</summary>
    [JSInvokable]
    public async Task InvokeOnLoadAsync(IJSObjectReference jsProgressEvent)
    {
        if (OnLoad is null)
        {
            return;
        }

        await OnLoad.Invoke(new ProgressEvent(JSRuntime, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method that will be removed in next major release.</summary>
    [JSInvokable]
    public async Task InvokeOnAbortAsync(IJSObjectReference jsProgressEvent)
    {
        if (OnAbort is null)
        {
            return;
        }

        await OnAbort.Invoke(new ProgressEvent(JSRuntime, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method that will be removed in next major release.</summary>
    [JSInvokable]
    public async Task InvokeOnErrorAsync(IJSObjectReference jsProgressEvent)
    {
        if (OnError is null)
        {
            return;
        }

        await OnError.Invoke(new ProgressEvent(JSRuntime, jsProgressEvent, new() { DisposesJSReference = true }));
    }

    /// <summary>Internal method that will be removed in next major release.</summary>
    [JSInvokable]
    public async Task InvokeOnLoadEndAsync(IJSObjectReference jsProgressEvent)
    {
        if (OnLoadEnd is null)
        {
            return;
        }

        await OnLoadEnd.Invoke(new ProgressEvent(JSRuntime, jsProgressEvent, new() { DisposesJSReference = true }));
    }
}
