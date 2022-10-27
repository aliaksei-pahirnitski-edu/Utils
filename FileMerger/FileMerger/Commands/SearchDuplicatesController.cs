using FileMerger.Domain.Abstract;
using FileMerger.Domain.Entity;
using FileMerger.Domain.Model;
using FileMerger.Settings;
using FilesHashComparer.Domain.Result;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMerger.App.Handlers
{
    /// <summary>
    /// Compares files or folder with snapshot and prints matchings or relocates to "/existing/" subfolder
    /// </summary>
    internal class SearchDuplicatesController
    {
        private readonly CommonSettings _settings;
        private readonly IScanner _scanner;
        private readonly IPersist _repo;
        private int _countExisting;
        private int _countUnique;
        private FolderEntity _scannedFolder;
        private ISnapshot _etalonSnapshot;

        public SearchDuplicatesController(IOptions<CommonSettings> settings
            , IScanner scanner
            , IPersist repo)
        {
            _settings = settings.Value;
            _scanner = scanner;
            _repo = repo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder">full folder path</param>
        /// <exception cref="Exception">when appsettings has no WorkingFolder;</exception>
        public void ProcessDuplicates(string folder)
        {
            if (_settings.WorkingFolder == null)
            {
                throw new Exception("WorkingFolder not set in appsettings.json!");
            }
            if (!Directory.Exists(folder))
            {
                Console.WriteLine($"Folder not valid or not exists: [{folder}]");
            }

            Console.WriteLine($"start search duplicates for [{folder}] ..");
            var sw = Stopwatch.StartNew();
            _scannedFolder = _scanner.ScanFolder(folder);
            if (_settings.Verbose)
            {
                var scanTime = sw.Elapsed;
                Console.WriteLine($"Snapshot scanned in {scanTime:hh\\:mm\\:ss\\_fff\\.ff}");
            }
            var snapshotFilePath = Path.Combine(_settings.WorkingFolder, _settings.SnapshotCompareTo);
            _etalonSnapshot = _repo.ReadFromFile(snapshotFilePath);
            if (_settings.Verbose)
            {
                var scanTime = sw.Elapsed;
                Console.WriteLine($"Snapshot compare to opened {scanTime:hh\\:mm\\:ss\\_fff\\.ff}");
            }
            _countExisting = 0;
            _countUnique = 0;
            Parallel.ForEach(_scannedFolder.DeeplyEnumerate(), CheckIfDuplicate);


            sw.Stop();
            Console.WriteLine($"Done! Took {sw.Elapsed:hh\\:mm\\:ss\\_fff\\.ff}");
        }

        private void CheckIfDuplicate(ComparableEntity entity)
        {
            if (entity is FileEntity fileEntity)
            {
                CheckIfDuplicate(fileEntity);
            }
        }

        private void CheckIfDuplicate(FileEntity fileEntity)
        {
            var matches = _etalonSnapshot.Find(fileEntity);
            if (matches == null || matches.Count == 0)
            {
                Interlocked.Increment(ref _countUnique);
            }
            else
            {
                Interlocked.Increment(ref _countExisting);
                if (_settings.Verbose)
                {
                    // var matchResult  = new MatchItemResult(fileEntity.FullName, matches);
                    // Console.WriteLine($"...");
                }
                var relativePath = Path.GetRelativePath(_scannedFolder.FullName, fileEntity.FullName);
                var destination = Path.Combine(_scannedFolder.FullName, "Existing", relativePath);
                File.Move(fileEntity.FullName, destination);
            }
        }
    }
}
