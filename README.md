[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE.md)
[![GitHub issues](https://img.shields.io/github/issues/KristofferStrube/Blazor.FileAPI)](https://github.com/KristofferStrube/Blazor.FileAPI/issues)
[![GitHub forks](https://img.shields.io/github/forks/KristofferStrube/Blazor.FileAPI)](https://github.com/KristofferStrube/Blazor.FileAPI/network/members)
[![GitHub stars](https://img.shields.io/github/stars/KristofferStrube/Blazor.FileAPI)](https://github.com/KristofferStrube/Blazor.FileAPI/stargazers)

<!--[![NuGet Downloads (official NuGet)](https://img.shields.io/nuget/dt/KristofferStrube.Blazor.FileAPI?label=NuGet%20Downloads)](https://www.nuget.org/packages/KristofferStrube.Blazor.FileAPI/)  -->

# Introduction
A Blazor wrapper for the browser [File API](https://www.w3.org/TR/FileAPI/)

The API provides a standard for representing file objects in the browser and ways to select them and access their data. One of the most used interfaces that is at the core of this API is the [Blob](https://www.w3.org/TR/FileAPI/#dfn-Blob) interface. This project implements a wrapper around the API for Blazor so that we can easily and safely interact with files in the browser.

# Demo
The sample project can be demoed at https://kristofferstrube.github.io/Blazor.FileAPI/

On each page you can find the corresponding code for the example in the top right corner.

On the *API Coverage Status* page you can get an overview over what parts of the API we support currently.

# Getting Started
## Prerequisites
You need to install .NET 6.0 or newer to use the library.

[Download .NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)

## Installation
You can install the package via NuGet with the Package Manager in your IDE or alternatively using the command line:
```bash
dotnet add package KristofferStrube.Blazor.FileAPI
```

# Usage
The package can be used in Blazor WebAssembly and Blazor Server projects.
## Import
You also need to reference the package in order to use it in your pages. This can be done in `_Import.razor` by adding the following.
```razor
@using KristofferStrube.Blazor.FileAPI
```

## Creating wrapper instances
Most of this library is wrapper classes which can be instantiated from your code using the static `Create` and `CreateAsync` methods on the wrapper classes.
An example could be to create an instance of a `Blob` that contains the text `"Hello World!"` and gets its `Size` and `Type`, read it as a `ReadableStream`, read as text directly, and slice it into a new `Blob` like this.
```csharp
Blob blob = await Blob.CreateAsync(
    JSRuntime,
    blobParts: new BlobPart[] {
        new("Hello "),
        new(new byte[] { 0X57, 0X6f, 0X72, 0X6c, 0X64, 0X21 })
    },
    options: new() { Type = "text/plain" }
);
ulong size = await blob.GetSizeAsync(); // 12
string type = await blob.GetTypeAsync(); // "text/plain"
ReadableStream stream = await blob.StreamAsync();
string text = await blob.TextAsync(); // "Hello World!"
Blob worldBlob = await blob.SliceAsync(6, 11); // Blob containing "World"
```
All creator methods take an `IJSRuntime` instance as the first parameter. The above sample will work in both Blazor Server and Blazor WebAssembly. If we only want to work with Blazor WebAssembly we can use the `InProcess` variant of the wrapper class. This is equivalent to the relationship between `IJSRuntime` and `IJSInProcessRuntime`. We can recreate the above sample using the `BlobInProcess` which will simplify some of the methods we can call on the `Blob` and how we access attributes.
```csharp
BlobInProcess blob = await BlobInProcess.CreateAsync(
    JSRuntime,
    blobParts: new BlobPart[] {
        new("Hello "),
        new(new byte[] { 0X57, 0X6f, 0X72, 0X6c, 0X64, 0X21 })
    },
    options: new() { Type = "text/plain" }
);
ulong size = blob.Size; // 12
string type = blob.Type; // "text/plain"
ReadableStreamInProcess stream = await blob.StreamAsync();
string text = await blob.TextAsync(); // "Hello World!"
BlobInProcess worldBlob = blob.Slice(6, 11); // BlobInProcess containing "World"
```
Some of the methods wrap a `Promise` so even in the `InProcess` variant we need to await it like we see for `TextAsync` above.

If you have an `IJSObjectReference` or an `IJSInProcessObjectReference` for a type equivalent to one of the classes wrapped in this package then you can construct a wrapper for it using another set of overloads of the static `Create` and `CreateAsync` methods on the appropriate class. In the below example we create wrapper instances from existing JS references to a `File` object.
```csharp
// Blazor Server compatible.
IJSObjectReference jSFile; // JS Reference from other package or your own JSInterop.
File file = File.Create(JSRuntime, jSFile)

// InProcess only supported in Blazor WebAssembly.
IJSInProcessObjectReference jSFileInProcess; // JS Reference from other package or your own JSInterop.
FileInProcess fileInProcess = await File.CreateAsync(JSRuntime, jSFileInProcess)
```

## Add to service collection
We have a single service in this package that wraps the `URL` interface. An easy way to make the service available in all your pages is by registering it in the `IServiceCollection` so that it can be dependency injected in the pages that need it. This is done in `Program.cs` by adding the following before you build the host and run it.
```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Other services are added.

builder.Services.AddURLService();

await builder.Build().RunAsync();
```
## Inject in page
Then the service can be injected in a page and be used to create Blob URLs and revoke them like so:
```razor
@implements IAsyncDisposable
@inject IURLService URL;

<img src="@blobURL" alt="Some blob as image" />

@code {
    private string blobURL = "";

    protected override async Task OnInitializedAsync()
    {
        Blob blob; // We have some blob from somewhere.

        blobURL = await URL.CreateObjectURLAsync(blob);
    }

    public async ValueTask DisposeAsync()
    {
        await URL.RevokeObjectURLAsync(blobURL);
    }
}
```
You can likewise add the `InProcess` variant of the service (`IURLServiceInProcess`) using the `AddURLServiceInProcess` extension method which is only supported in Blazor WebAssembly projects.

# Issues
Feel free to open issues on the repository if you find any errors with the package or have wishes for features.

# Related repositories
This project uses this package to return a rich `ReadableStream` from the `StreamAsync` method on a `Blob`.
- https://github.com/KristofferStrube/Blazor.Streams

This project is going to be used in this package to return a rich `File` object when getting the `File` from a `FileSystemFileHandle` and when writing a `Blob` to a `FileSystemWritableFileSystem`.
- https://github.com/KristofferStrube/Blazor.FileSystemAccess

This project uses a combination of the two styles present in the two above packages which both eventually will go more towards the style present in this project.

# Related articles
This repository was build with inspiration and help from the following series of articles:

- [Wrapping JavaScript libraries in Blazor WebAssembly/WASM](https://blog.elmah.io/wrapping-javascript-libraries-in-blazor-webassembly-wasm/)
- [Call anonymous C# functions from JS in Blazor WASM](https://blog.elmah.io/call-anonymous-c-functions-from-js-in-blazor-wasm/)
- [Using JS Object References in Blazor WASM to wrap JS libraries](https://blog.elmah.io/using-js-object-references-in-blazor-wasm-to-wrap-js-libraries/)
- [Blazor WASM 404 error and fix for GitHub Pages](https://blog.elmah.io/blazor-wasm-404-error-and-fix-for-github-pages/)
- [How to fix Blazor WASM base path problems](https://blog.elmah.io/how-to-fix-blazor-wasm-base-path-problems/)