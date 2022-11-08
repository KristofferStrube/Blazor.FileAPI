namespace KristofferStrube.Blazor.FileAPI;

public interface IURLService
{
    Task<string> CreateObjectURLAsync(Blob obj);
    Task RevokeObjectURLAsync(string url);
}