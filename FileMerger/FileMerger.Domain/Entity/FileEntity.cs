namespace FileMerger.Domain.Entity;

public class FileEntity : ComparableEntity
{
    public FileEntity(string fullPath, string host) : base(fullPath, host)
    {
    }

    public override string ToString()
    {
        return $"File: {FullName}";
    }
}
