namespace KristofferStrube.Blazor.FileAPI;

/// <inheritdoc/>
public interface IURLServiceInProcess : IURLService
{
    /// <inheritdoc/>
    string CreateObjectURL(Blob obj);

    /// <inheritdoc/>
    void RevokeObjectURL(string url);
}