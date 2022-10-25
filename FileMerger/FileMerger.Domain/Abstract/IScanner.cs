using FileMerger.Domain.Entity;

namespace FileMerger.Domain.Abstract
{
    public interface IScanner
    {
        FileEntity ScanFile(string fullPath);
        FolderEntity ScanFolder(string fullPath);
    }
}
