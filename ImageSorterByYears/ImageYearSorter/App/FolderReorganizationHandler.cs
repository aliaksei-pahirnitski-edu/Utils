using Cocona;
using ImageYearSorter.Models;
using ImageYearSorter.Utils;
using ImageYearSorter.ValueObjects;
using System.Diagnostics;
using System.Text.Json;

namespace ImageYearSorter.App
{
    /// <summary>
    /// Handler for commands to 
    ///  - status: reports status if relocation of pictures needed + check if relocated earlier or manually images in correct subfolder
    ///  - run: perform relocation
    /// </summary>
    internal class FolderReorganizationHandler
    {
        private readonly IPhotoDateProvider _photoDateProvider;
        public const string GuiTimeSmallSpanFormat = "hh\\:mm\\:ss\\.fff";

        public FolderReorganizationHandler(IPhotoDateProvider photoDateProvider)
        {
            _photoDateProvider = photoDateProvider;
        }

        ///// <summary>
        ///// Checks pictures relocated to year and quater subfolders, if theirs picture date matches subfolder prefix
        ///// </summary>
        ///// <param name="folderPath">Root folder where to search image files</param>
        //public void Check(string folder)
        //{
        //    Console.WriteLine("Todo Check not implemented..");
        //}

        /// <summary>
        /// Performs relocation of images, except ones already located in year-with-quarter subfolder
        /// </summary>
        /// <param name="folderPath">Root folder where to search image files</param>
        public void Run(string folder)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"Running folder sorting by image taken date in the folder [{folder}] ...");
            var checkFolderResult = FolderPath.Create(folder);
            if (!checkFolderResult.IsOk())
            {
                foreach (var invalidation in checkFolderResult.Invalidations!)
                {
                    Console.WriteLine(invalidation.GetMessage());
                }

                return;
            }

            var folderProcessor = new FolderRootModel(checkFolderResult.OkResult);
            folderProcessor.Move(_photoDateProvider);
            sw.Stop();
            var timespan = sw.Elapsed.ToString(GuiTimeSmallSpanFormat);
            Console.WriteLine($"Done!  in {sw.Elapsed:hh\\:mm\\:ss\\.fff}");
        }

        /// <summary>
        /// Reports status of wherther relocation of pictures needed and how many
        /// </summary>
        /// <param name="folder">Root folder where to search image files</param>
        public void Status(string folder)
        {
            Console.WriteLine($"Starting checking status of the folder [{folder}] ...");
            var checkFolderResult = FolderPath.Create(folder);
            if (!checkFolderResult.IsOk())
            {
                foreach (var invalidation in checkFolderResult.Invalidations!)
                {
                    Console.WriteLine(invalidation.GetMessage());
                }

                return;
            }

            var folderProcessor = new FolderRootModel(checkFolderResult.OkResult);
            var status = folderProcessor.GetStatus(_photoDateProvider);

            var prettyResult = JsonSerializer.Serialize(status, new JsonSerializerOptions() { WriteIndented = true });
            Console.WriteLine(prettyResult);
        }
    }
}
