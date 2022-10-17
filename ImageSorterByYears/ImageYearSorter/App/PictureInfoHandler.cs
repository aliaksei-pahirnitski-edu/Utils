using System.Diagnostics;
using System.Text.Json;
using Cocona;
using ImageYearSorter.App.Dto;
using ImageYearSorter.Models;
using ImageYearSorter.Utils;
using ImageYearSorter.ValueObjects;

namespace ImageYearSorter.App
{
    /// <summary>
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
            var testNormalizedPath = Path.GetFullPath(imageFullPath);
            Debug.Assert(imageFullPath == testNormalizedPath); //todo, thinking..

            var checkFilePathResult = FilePath.Convert(imageFullPath);
            if (!checkFilePathResult.IsOk())
            {
                foreach (var invalidation in checkFilePathResult.Invalidations!)
                {
                    Console.WriteLine(invalidation.GetMessage());
                }

                return;
            }
            if (!_photoDateProvider.GetPictureDate(checkFilePathResult.Value!, out DateTimeOffset pictureTakenAt))
            {
                Console.WriteLine("Image has no metadata");
                return;
            }

            var photoModel = new PhotoYearQuaterModel(pictureTakenAt);

            var finfo = new FileInfo(imageFullPath);
            var test0 = finfo.CreationTime;
            var test1 = finfo.LastAccessTime;
            var test2 = finfo.LastWriteTime;

            Debug.Assert(finfo.CreationTime == File.GetCreationTime(imageFullPath));
            Debug.Assert(finfo.LastAccessTime == File.GetLastAccessTime(imageFullPath));
            Debug.Assert(finfo.LastWriteTime == File.GetLastWriteTime(imageFullPath));
            Debug.Assert(finfo.LastWriteTimeUtc == File.GetLastWriteTimeUtc(imageFullPath));

            

            File.GetCreationTime(imageFullPath);

            var pictureInfoWithVerbose = new PhotoInfoDto(
                ImageFullPath: imageFullPath
                , Extention: Path.GetExtension(imageFullPath)
                , IsImage: false // todo
                , IsVideo: false // todo
                , PictureTakenAt: photoModel.AsDto()
                , FileCreatedAt: finfo.CreationTime
                , FileModifiedAt: finfo.LastWriteTime);

            var prettyResult = JsonSerializer.Serialize(pictureInfoWithVerbose, new JsonSerializerOptions() { WriteIndented = true });
            Console.WriteLine(prettyResult);
        }
    }
}
