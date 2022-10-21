using ImageYearSorter.ValueObjects;

namespace ImageYearSorter.App
{
    /// <summary>
    /// Handler for commands to 
    ///  - reports status if relocation of pictures needed
    ///  - perform relocation
    ///  - check if relocated earlier or manually images in correct subfolder
    /// </summary>
    internal class FolderReorganizationHandler
    {
        public FolderReorganizationHandler()
        {
        }

        /// <summary>
        /// Checks pictures relocated to year and quater subfolders, if theirs picture date matches subfolder prefix
        /// </summary>
        /// <param name="folderPath">Root folder where to search image files</param>
        public void Check(string folderPath)
        {
            Console.WriteLine("Todo Check not implemented..");
        }

        /// <summary>
        /// Performs relocation of images, except ones already located in year-with-quarter subfolder
        /// </summary>
        /// <param name="folderPath">Root folder where to search image files</param>
        public void Run(string folderPath)
        {
            Console.WriteLine("Todo Run not implemented..");
        }

        /// <summary>
        /// Reports status of wherther relocation of pictures needed and how many
        /// </summary>
        /// <param name="folderPath">Root folder where to search image files</param>
        public void Status(string folderPath)
        {
            Console.WriteLine($"Starting checking status of the folder [{folderPath}] ...");
            var checkFolderResult = FolderPath.Create(folderPath);
            if (!checkFolderResult.IsOk())
            {
                foreach (var invalidation in checkFolderResult.Invalidations!)
                {
                    Console.WriteLine(invalidation.GetMessage());
                }

                return;
            }
            
            folderPath = checkFolderResult.OkResult.NormalizedFullPath;



        }
    }
}
