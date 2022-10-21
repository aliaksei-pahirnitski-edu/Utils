namespace ImageYearSorter.ValueObjects;

/// <summary>Wrapper to string, meaning it is valid and existing file full path</summary>
public sealed class FilePath : ValueObject
{
    public string NormalizedFullPath { get; init; }
    private FilePath(string fullPath)
    {
        if (!File.Exists(fullPath)) throw new ArgumentException($"Not valid file path [{fullPath}]", nameof(fullPath));
        NormalizedFullPath = Path.GetFullPath(fullPath);
    }

    public static Result<FilePath> Create(string fullPath)
    {
        if (File.Exists(fullPath))
        {
            return new FilePath(fullPath);
        }
        return Result<FilePath>.Error(new Invalidation("Not valid or not existing file path"));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NormalizedFullPath;
    }
}
