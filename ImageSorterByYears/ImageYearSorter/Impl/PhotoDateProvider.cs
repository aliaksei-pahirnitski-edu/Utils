using ImageYearSorter.ValueObjects;
using ExifLib;
using ImageYearSorter.Utils;

namespace ImageYearSorter.Impl
{
    internal class PhotoDateProvider : IPhotoDateProvider
    {
        public bool GetPictureDate(FilePath imgFilePath, out DateTimeOffset pictureTakenAt)
        {
            pictureTakenAt = DateTimeOffset.MinValue;

            using (ExifReader reader = new ExifReader(imgFilePath.NormalizedFullPath))
            {
                // Extract the tag data using the ExifTags enumeration
                if (reader.GetTagValue(ExifTags.DateTimeDigitized, out DateTime datePictureTaken))
                {
                    pictureTakenAt = datePictureTaken;
                    return true;
                }
            }
            return false;
        }
    }
}
