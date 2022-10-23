namespace ImageYearSorter.ValueObjects;

/// <summary>Wrapper to string, meaning it is valid and existing file full path</summary>
public sealed class FilePath : ValueObject
{
    public const string JPEG = ".jpeg";
    public const string JPG = ".jpg";

    public const string MP4 = ".mp4";
    public const string MOV = ".mov";
    public const string _3GP = ".3gp";

    public string NormalizedFullPath { get; init; }
    
    /// <summary>Extension to lower and including period (dot)</summary>
    public string Extension { get; init; }
    
    private FilePath(string fullPath)
    {
        if (!File.Exists(fullPath)) throw new ArgumentException($"Not valid file path [{fullPath}]", nameof(fullPath));
        NormalizedFullPath = Path.GetFullPath(fullPath);
        Extension = Path.GetExtension(NormalizedFullPath).ToLower();
    }

    public static Result<FilePath> Create(string fullPath)
    {
        if (File.Exists(fullPath))
        {
            return new FilePath(fullPath);
        }
        return Result<FilePath>.Error(new Invalidation("Not valid or not existing file path [{0}]", fullPath));
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NormalizedFullPath;
    }

    public override string ToString() => NormalizedFullPath;

    public bool IsImage => Extension == JPG || Extension == JPEG;
    public bool IsVideo => Extension == MOV || Extension == MP4 || Extension == _3GP;
}
