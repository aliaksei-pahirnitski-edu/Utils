namespace ImageYearSorter.ValueObjects;

/// <summary>Wrapper to string, meaning it is valid and existing folder path</summary>
public sealed class FolderPath : ValueObject
{
    public string NormalizedFullPath { get; init; }
    private FolderPath(string fullPath)
    {
        if (!Directory.Exists(fullPath)) throw new ArgumentException($"Not valid directory path [{fullPath}]", nameof(fullPath));
        NormalizedFullPath = Path.GetFullPath(fullPath);
    }

    public static Result<FolderPath> Create(string fullPath)
    {
        if (File.Exists(fullPath))
        {
            return new FolderPath(fullPath);
        }
        return Result<FolderPath>.Error(new Invalidation("Not valid or not existing directory path"));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NormalizedFullPath;
    }
}
