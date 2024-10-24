namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#url">URL browser specs</see>
/// </summary>
public interface IURLService
{
    /// <summary>
    /// Creates a new Blob URL and adds that to the current contexts Blob URL store.
    /// </summary>
    /// <param name="obj">The <see cref="Blob"/> that you wish to create a URL for.</param>
    /// <returns>a Blob Url which can be used as a source in different media like image, sound, iframe sources.</returns>
    Task<string> CreateObjectURLAsync(Blob obj);

    /// <summary>
    /// Removes a specific URL from the current contexts Blob URL store.
    /// </summary>
    /// <param name="url">The URL that is to be removed.</param>
    Task RevokeObjectURLAsync(string url);
}