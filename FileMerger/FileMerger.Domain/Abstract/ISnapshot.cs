using FileMerger.Domain.Entity;

namespace FileMerger.Domain.Abstract
{
    /// <summary>
    /// Collection of files and folder metadata
    /// </summary>
    public interface ISnapshot
    {
        /// <summary>
        /// PC name if known
        /// </summary>
        string Host { get; }

        /// <summary>
        /// External info
        /// </summary>
        IDictionary<string, object> Tag { get; }

        /// <summary>
        /// Tree or forest of files and folder metadata
        /// </summary>
        IEnumerable<ComparableEntity> Items { get; }

        IReadOnlyCollection<ComparableEntity> Find(FileEntity file)
        {
            return Items
                .Where(x => x.Hash == file.Hash && x.Size == file.Size) // search by shortname?
                .Where(x => x.Host != file.Host || x.FullName != file.FullName) // exclude itself from match
                .ToList();
        }
    }
}
