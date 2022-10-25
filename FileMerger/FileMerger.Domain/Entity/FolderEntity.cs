namespace FileMerger.Domain.Entity;

public class FolderEntity : ComparableEntity
{
    public FolderEntity(string fullPath, string host) : base(fullPath, host)
    {
    }

    public List<ComparableEntity> Children { get; set; } = new List<ComparableEntity>();

    public bool DeeplyContains(ComparableEntity otherItem)
    {
        if (otherItem is FolderEntity otherFolder)
            return DeeplyContains(otherFolder);
        return DeeplyContains(otherItem.Parent);
    }

    public bool DeeplyContains(FolderEntity otherFolder)
    {
        if (otherFolder == null) return false;

        var thisUpper = FullName.ToUpper();
        var otherUpper = otherFolder.FullName.ToUpper();
        return otherUpper.StartsWith(thisUpper);
    }

    /// <summary>
    /// Starting from (including) this folder as Root - all deeply nested children
    /// </summary>
    public IEnumerable<ComparableEntity> DeeplyEnumerate()
    {
        yield return this;
        if (Children == null) yield break;

        foreach (var fileItem in Children.Where(x => x is FileEntity).Cast<FileEntity>())
        {
            yield return fileItem;
        }

        foreach (var childFolder in Children.Where(x => x is FolderEntity).Cast<FolderEntity>())
        {
            foreach (var nestedChild in childFolder.DeeplyEnumerate())
            {
                yield return nestedChild;
            }
        }
    }

    public override string ToString()
    {
        return $"Folder: {FullName}";
    }
}
