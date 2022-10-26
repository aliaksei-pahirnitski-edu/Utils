namespace FileMerger.Domain.Abstract
{
    public interface IPersist
    {
        /// <summary>
        /// Saves scanned hashes snapshot
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fullPath"></param>
        /// <returns>Modified full filename</returns>
        string SaveToFile(ISnapshot data, string fullPath);

        ISnapshot ReadFromFile(string fullPath);
        string SuggestFileName(ISnapshot data);
    }
}
