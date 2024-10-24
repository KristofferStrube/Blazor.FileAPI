namespace KristofferStrube.Blazor.FileAPI;


/// <summary>
/// Union Type representing either a <see cref="byte"/>[], a <see cref="Blob"/>, or a <see cref="string"/>.
/// </summary>
/// <summary>
/// <see href="https://www.w3.org/TR/FileAPI/#typedefdef-blobpart">BlobPart browser specs</see>
/// </summary>
public class BlobPart
{
    internal readonly object Part;

    /// <summary>
    /// Creates a <see cref="BlobPart"/> from a <see cref="byte"/>[] explicitly instead of using the implicit converter.
    /// </summary>
    /// <param name="part">A <see cref="byte"/>[].</param>
    public BlobPart(byte[] part)
    {
        Part = part;
    }

    /// <summary>
    /// Creates a <see cref="BlobPart"/> from a <see cref="Blob"/> explicitly instead of using the implicit converter.
    /// </summary>
    /// <param name="part">A <see cref="Blob"/>.</param>
    public BlobPart(Blob part)
    {
        Part = part;
    }

    /// <summary>
    /// Creates a <see cref="BlobPart"/> from a <see cref="string"/> explicitly instead of using the implicit converter.
    /// </summary>
    /// <param name="part">A <see cref="string"/>.</param>
    public BlobPart(string part)
    {
        Part = part;
    }

    /// <summary>
    /// Creates a <see cref="BlobPart"/> from a <see cref="byte"/>[].
    /// </summary>
    /// <param name="part">A <see cref="byte"/>[].</param>
    public static implicit operator BlobPart(byte[] part)
    {
        return new(part);
    }

    /// <summary>
    /// Creates a <see cref="BlobPart"/> from a <see cref="Blob"/>.
    /// </summary>
    /// <param name="part">A <see cref="Blob"/>.</param>
    public static implicit operator BlobPart(Blob part)
    {
        return new(part);
    }

    /// <summary>
    /// Creates a <see cref="BlobPart"/> from a <see cref="string"/>.
    /// </summary>
    /// <param name="part">A <see cref="string"/>.</param>
    public static implicit operator BlobPart(string part)
    {
        return new(part);
    }
}