namespace KristofferStrube.Blazor.FileAPI;

public class BlobPart
{
    internal readonly byte[]? byteArrayPart;
    internal readonly Blob? blobPart;
    internal readonly string? stringPart;
    internal readonly BlobPartType type;

    public BlobPart(byte[] part)
    {
        byteArrayPart = part;
        type = BlobPartType.BufferSource;
    }

    public BlobPart(Blob part)
    {
        blobPart = part;
        type = BlobPartType.Blob;
    }

    public BlobPart(string part)
    {
        stringPart = part;
        type = BlobPartType.String;
    }
}

internal enum BlobPartType
{
    BufferSource,
    Blob,
    String
}