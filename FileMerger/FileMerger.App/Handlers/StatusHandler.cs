using FileMerger.App.Dto;
using FileMerger.Domain.Abstract;
using FilesHashComparer.Domain.Result;

namespace FileMerger.App.Handlers
{
    /// <summary>
    /// Prints information about file
    /// </summary>
    public class StatusHandler
    {
        private readonly IScanner _scanner;

        public StatusHandler(IScanner scanner)
        {
            _scanner = scanner;
        }

        public StatusForFileDto StatusForFile(string filePath)
        {
            var fileEntity = _scanner.ScanFile(filePath);

            return new StatusForFileDto(
                Exists: false // todo
                , File: fileEntity
                , Matches: Array.Empty<MatchItemResult>()
                );
        }


        public StatusForFolderDto StatusForFolder(string folderPath)
        {
            var folderEntity = _scanner.ScanFolder(folderPath);            

            return new StatusForFolderDto(
                Exists: false // ok, we don't focus on folders
                , Folder: folderEntity
                );
        }
    }
}
