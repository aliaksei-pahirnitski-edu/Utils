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
        /// Tree or forest of files and folder metadata
        /// </summary>
        IEnumerable<ComparableEntity> Items { get; }

        /// <summary>
        /// External info
        /// </summary>
        IDictionary<string, object> Tag { get; }
    }
}
