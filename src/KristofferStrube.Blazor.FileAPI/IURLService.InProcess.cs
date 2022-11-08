namespace KristofferStrube.Blazor.FileAPI;

public interface IURLServiceInProcess : IURLService
{
    string CreateObjectURL(Blob obj);
    void RevokeObjectURL(string url);
}