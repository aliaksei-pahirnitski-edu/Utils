using System.Text.Json;
using Cocona;
using ImageYearSorter.Models;
using ImageYearSorter.Utils;
using ImageYearSorter.ValueObjects;

namespace ImageYearSorter.App
{
    /// <summary>
    /// This handler prints file metadata, mostly for photo images, but also added workaround for video.
    /// Used mostly for testing purposes
    ///  - to check that we receive correct date at which picture was maid
    ///  - to check different exceptional situations
    /// </summary>
    public class PictureInfoHandler
    {
        private readonly IPhotoDateProvider _photoDateProvider;

        public PictureInfoHandler(IPhotoDateProvider photoDateProvider)
        {
            _photoDateProvider = photoDateProvider;
        }

        /// <summary>
        /// Prints image metainfo
        /// Usage: print-info "full/image/path.ext"
        /// </summary>
        /// <param name="imageFullPath">Full path to image</param>
        public void PrintInfo([Argument] string imageFullPath)
        {
            var checkFilePathResult = FilePath.Create(imageFullPath);
            if (!checkFilePathResult.IsOk())
            {
                foreach (var invalidation in checkFilePathResult.Invalidations!)
                {
                    Console.WriteLine(invalidation.GetMessage());
                }

                return;
            }            

            var photoModel = new FileMetadataModel(checkFilePathResult.OkResult);
            var pictureMetadata = photoModel.FindMetadata(_photoDateProvider);

            var prettyResult = JsonSerializer.Serialize(pictureMetadata, new JsonSerializerOptions() { WriteIndented = true });
            Console.WriteLine(prettyResult);

            if (!pictureMetadata.HasMetadata)
            {
                Console.WriteLine("Image has no metadata!");
                return;
            }
        }
    }
}
