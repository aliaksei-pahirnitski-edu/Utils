using FileMerger.Domain.Entity;
using FilesHashComparer.Domain.Result;

namespace FileMerger.Domain.Abstract
{
    public interface IHashComparer
    {
        /// <summary>
        /// Check that size and hash equal
        /// </summary>
        /// <param name="item">file or folder</param>
        /// <param name="otherSnapshot">list form another PC or another folder</param>
        /// <returns></returns>
        MatchItemResult IsMatch(ComparableEntity item, ISnapshot otherSnapshot);

        /// <summary>
        /// Not only check that size and hash equal but also returns matched ones
        /// </summary>
        /// <param name="item">file or folder</param>
        /// <param name="listToCompare">list form another PC or another folder</param>
        /// <returns></returns>
        IReadOnlyCollection<ComparableEntity> GetMatches(ComparableEntity item, IEnumerable<ComparableEntity> listToCompare);

        /// <summary>
        /// Search matching and not matching items
        /// </summary>
        /// <param name="snapshotToCheck">Snapshot to be compared</param>
        /// <param name="otherSnapshot">list form another PC or another folder</param>
        /// <returns></returns>
        IReadOnlyCollection<MatchItemResult> Compare(ISnapshot snapshotToCheck, ISnapshot otherSnapshot);
    }
}
