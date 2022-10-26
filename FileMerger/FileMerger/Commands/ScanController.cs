using FileMerger.Domain.Abstract;
using FileMerger.Domain.Model;
using FileMerger.Settings;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace FileMerger.App.Handlers
{
    /// <summary>
    /// Generates hash and created sqlite db
    /// </summary>
    public class ScanController
    {
        private readonly CommonSettings _commonSettings;
        private readonly IScanner _scanner;
        private readonly IPersist _repo;

        // private readonly IConfiguration _conf;

        public ScanController(IOptions<CommonSettings> commonSettings
            , IScanner scanner
            , IPersist repo)
        {
            _commonSettings = commonSettings.Value;
            _scanner = scanner;
            _repo = repo;
        }

        public void Scan(string folder)
        {
            if (_commonSettings.WorkingFolder == null)
            {
                throw new Exception("WorkingFolder not set in appsettings.json!");
            }
            if (Directory.Exists(folder))
            {
                Console.WriteLine($"start scan [{folder}] ..");
                var sw = Stopwatch.StartNew();
                var scannedFolder = _scanner.ScanFolder(folder);
                var snapshot = new TreeSnapshot(scannedFolder);
                if (_commonSettings.Verbose)
                {
                    var scanTime = sw.Elapsed;
                    Console.WriteLine($"Snapshot scanned in {scanTime:hh\\:mm\\:ss\\_fff\\.ff}");
                }
                var fileToSave = _repo.SuggestFileName(snapshot);
                var fileSavedAt = _repo.SaveToFile(snapshot, Path.Combine(_commonSettings.WorkingFolder, fileToSave));
                sw.Stop();
                Console.WriteLine($"Snapshot created in {sw.Elapsed:hh\\:mm\\:ss\\_fff\\.ff}");
                Console.WriteLine($"Snapshot saved to [{fileSavedAt}]");
            }
            else
            {
                Console.WriteLine($"Folder not valid or not exists: [{folder}]");
            }
        }
    }
}
