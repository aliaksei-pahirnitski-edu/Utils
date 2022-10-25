using FileMerger.Domain.Abstract;
using FileMerger.Domain.Entity;

namespace FileMerger.Domain.Model;

public class ForestSnapshots : ISnapshot
{
    private readonly List<TreeSnapshot> _trees = new List<TreeSnapshot>();
    public string Host { get; private set; }
    public IDictionary<string, object> Tag { get; set; } = new Dictionary<string, object>();

    public ForestSnapshots AddTree(TreeSnapshot tree)
    {
        if (!string.IsNullOrEmpty(Host)
            && !string.IsNullOrEmpty(tree.Host)
            && Host != tree.Host)
        {
            throw new ArgumentException($"Tree host not matches: {tree.Host} vs {Host}");
        }

        if (string.IsNullOrEmpty(Host) && !string.IsNullOrEmpty(tree.Host))
        {
            Host = tree.Host;
        }

        var newRoot = tree.Root;
        if (newRoot != null)
        {
            foreach (var treeSnapshot in _trees)
            {
                if (treeSnapshot.Root != null && treeSnapshot.Root.DeeplyContains(newRoot))
                {
                    // skip, not add duplicate
                    return this;
                }
            }
        }
        _trees.Add(tree);

        return this;
    }

    IEnumerable<ComparableEntity> ISnapshot.Items
    {
        get
        {
            foreach (var tree in _trees)
            {
                foreach (var item in tree.Items)
                {
                    yield return item;
                }
            }
        }
    }

    public IEnumerable<TreeSnapshot> Trees => _trees;
}
