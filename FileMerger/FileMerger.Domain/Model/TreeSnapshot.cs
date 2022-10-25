using FileMerger.Domain.Abstract;
using FileMerger.Domain.Entity;

namespace FileMerger.Domain.Model;

public class TreeSnapshot : ISnapshot
{
    public List<ComparableEntity> Items { get; }
    public FolderEntity Root { get; }
    public string Host { get; set; }
    public IDictionary<string, object> Tag { get; set; } = new Dictionary<string, object>();

    public TreeSnapshot(IEnumerable<ComparableEntity> items, string? mashineName = null)
    {
        Items = items.ToList();
        Root = FindRoot(Items);
        Host = mashineName ?? Environment.MachineName;
    }


    public TreeSnapshot(FolderEntity rootFolder)
    {
        Items = rootFolder.DeeplyEnumerate().ToList();
        Root = rootFolder;
        Host = Environment.MachineName;
    }

    IEnumerable<ComparableEntity> ISnapshot.Items => Items;


    private FolderEntity FindRoot(IEnumerable<ComparableEntity> items)
    {
        var roots = items.Where(x => x is FolderEntity folder && folder.Parent == null)
            .Select(x => (FolderEntity)x).ToList();
        if (!roots.Any())
        {
            return null;
        }

        if (roots.Count() > 1)
        {
            throw new ArgumentException(nameof(items));
        }

        return roots.First();
    }
}
