using FileMerger.App.Dto;
using FileMerger.Domain.Abstract;
using FileMerger.Domain.Entity;
using FilesHashComparer.Domain.Result;

namespace FileMerger.App.Handlers
{
    /// <summary>
    /// Prints information about file
    /// </summary>
    public class StatusHandler
    {
        private readonly IScanner _scanner;
        private readonly IPersist _repo;

        public StatusHandler(IScanner scanner, IPersist repo)
        {
            _scanner = scanner;
            _repo = repo;
        }

        public StatusForFileDto StatusForFile(string filePath, string snapshotFilePath)
        {
            var fileEntity = _scanner.ScanFile(filePath);

            IReadOnlyCollection<ComparableEntity> matches = Array.Empty<ComparableEntity>();
            if (File.Exists(snapshotFilePath))
            {
                var snapshot = _repo.ReadFromFile(snapshotFilePath);
                matches = snapshot.Find(fileEntity);
            }

            return new StatusForFileDto(
                Exists: matches.Any()
                , File: fileEntity
                , Matches: new MatchItemResult(filePath, matches)
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
