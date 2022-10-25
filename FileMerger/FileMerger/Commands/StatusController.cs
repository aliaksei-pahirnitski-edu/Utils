using Cocona;
using FileMerger.Domain.Entity;
using System.Text.Json;

namespace FileMerger.App.Handlers
{
    /// <summary>
    /// Prints information about file
    /// </summary>
    public class StatusController
    {
        private readonly StatusHandler _statusHandler;

        public StatusController(StatusHandler statusHandler)
        {
            _statusHandler = statusHandler;
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
            var result = _statusHandler.StatusForFile(filePath);
            var prettyJson = JsonSerializer.Serialize(result, new JsonSerializerOptions() { WriteIndented = true });
            Console.WriteLine(prettyJson);
        }

        public void StatusForFolder(string folderPath)
        {
            var result = _statusHandler.StatusForFolder(folderPath);
            //foreach(var file in result.Folder.Children)
            //{
            //    file.Parent = null; // do not need, empty for json serializer to work
            //}
            result.Folder.Children?.Clear(); // do not need, empty for json serializer to work
            var prettyJson = JsonSerializer.Serialize(result, new JsonSerializerOptions() { WriteIndented = true });
            Console.WriteLine(prettyJson);
        }
    }
}
