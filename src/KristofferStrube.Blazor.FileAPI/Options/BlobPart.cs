namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#typedefdef-blobpart">BlobPart browser specs</see>
/// </summary>
public class BlobPart
{
    internal readonly object Part;

    internal BlobPart(object part)
    {
        Part = part;
    }
    [Obsolete("We added implicit converters from byte[] to BlobPart so you can parse it directly without using this constructor first.")]
    public BlobPart(byte[] part)
    {
        Part = part;
    }
    [Obsolete("We added implicit converters from Blob to BlobPart so you can parse it directly without using this constructor first.")]
    public BlobPart(Blob part)
    {
        Part = part;
    }
    [Obsolete("We added implicit converters from string to BlobPart so you can parse it directly without using this constructor first.")]
    public BlobPart(string part)
    {
        Part = part;
    }


    public static implicit operator BlobPart(byte[] part)
    {
        return new((object)part);
    }
    public static implicit operator BlobPart(Blob part)
    {
        return new((object)part);
    }
    public static implicit operator BlobPart(string part)
    {
        return new((object)part);
    }
}