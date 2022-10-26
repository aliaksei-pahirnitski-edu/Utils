namespace FileMerger.Domain.Entity;

public class FileEntity : ComparableEntity
{
    public FileEntity(string fullPath, string host) : base(fullPath, host)
    {
    }

    /// <summary>
    /// Constructor for entity framework
    /// </summary>
    private FileEntity()
    {
    }

    public override string ToString()
    {
        return $"File: {FullName}";
    }
}
