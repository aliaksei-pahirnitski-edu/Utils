namespace FileMerger.Domain.Entity;

public class ComparableEntity
{
    public ComparableEntity(string fullPath, string host)
    {
        Host = host;

        try
        {
            FullName = fullPath;

            // or path too long (260 char) and .net v less 4.6.2 - throws exception
            FullName = Path.GetFullPath(fullPath);
        }
        catch
        {
            // skip
        }
    }

    /// <summary>
    /// Constructor for entity framework
    /// </summary>
    protected ComparableEntity()
    {
    }

    /// <summary>
    /// PC name if known
    /// </summary>
    public string Host { get; init; }

    public long Size { get; set; }
    public string Hash { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; init; }

    public FolderEntity Parent { get; set; }
}
