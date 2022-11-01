using Cocona;
using FileMerger.Settings;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;

namespace FileMerger.App.Handlers
{
    /// <summary>
    /// Prints information about file
    /// </summary>
    public class StatusController
    {
        private readonly StatusHandler _statusHandler;
        private readonly CommonSettings _commonSettings;

        public StatusController(StatusHandler statusHandler, IOptions<CommonSettings> commonSettings)
        {
            _statusHandler = statusHandler;
            _commonSettings = commonSettings.Value;
        }

        [Command("status", Description = "For file and for folder - gets hash and check if has matches")]
        public void Status([Argument(Description = "Either file or folder")]string fileOfFolder)
        {
            if (Directory.Exists(fileOfFolder))
            {
                StatusForFolder(fileOfFolder);
            }
            else if (File.Exists(fileOfFolder))
            {
                StatusForFile(fileOfFolder);
            }
            else {
                Console.WriteLine($"arg is neither existing file nor folder: [{fileOfFolder}]");
            }
        }

        private void StatusForFile(string filePath)
        {
            if (_commonSettings.WorkingFolder == null) throw new Exception("WorkingFolder not set");
            Console.WriteLine($"Making status for file [{filePath}]");
            Console.WriteLine($"WorkingFolder = [{_commonSettings.WorkingFolder}]");
            Console.WriteLine($"Directory.GetCurrentDirectory() = [{Directory.GetCurrentDirectory()}]");

            var snapshotFilePath = Path.Combine(_commonSettings.WorkingFolder, _commonSettings.SnapshotCompareTo);
            var result = _statusHandler.StatusForFile(filePath, snapshotFilePath);
            var prettyJson = JsonSerializer.Serialize(result, new JsonSerializerOptions() { WriteIndented = true });
            Console.WriteLine(prettyJson);
        }

        private void StatusForFolder(string folderPath)
        {            
            Console.WriteLine($"Making status for folder [{folderPath}]");
            Console.WriteLine($"WorkingFolder = [{_commonSettings.WorkingFolder}]");
            Console.WriteLine($"Directory.GetCurrentDirectory() = [{Directory.GetCurrentDirectory()}]");
            
            var sw = Stopwatch.StartNew();
            var result = _statusHandler.StatusForFolder(folderPath);
            //foreach(var file in result.Folder.Children)
            //{
            //    file.Parent = null; // do not need, empty for json serializer to work
            //}
            result.Folder.Children?.Clear(); // do not need, empty for json serializer to work
            var prettyJson = JsonSerializer.Serialize(result, new JsonSerializerOptions() { WriteIndented = true });
            Console.WriteLine(prettyJson);
            sw.Stop();
            Console.WriteLine($"Occupied {sw.Elapsed:hh\\:mm\\:ss\\_fff\\.ff}");
        }
    }
}
