using ImageYearSorter.ValueObjects;
using ExifLib;
using ImageYearSorter.Utils;

namespace ImageYearSorter.Impl
{
    /// <summary>
    /// Gets date when picture was taken at
    /// </summary>
    /// <remarks>
    /// To be tested:
    ///  A) Negative
    ///  0) not valid path  => System.IO.FileNotFoundException: 'Could not find file ..
    ///  0.5) path to folder instead of file: System.UnauthorizedAccessException: 'Access to the path 'E:\..' is denied.'
    ///     or System.IO.IOException: 'The filename, directory name, or volume label syntax is incorrect.
    ///  1) file is not existing => System.IO.FileNotFoundException: 'Could not find file  ...
    ///  2) file is not an image (txt) => ExifLib.ExifLibException: 'File is not a valid JPEG'
    ///  3) file is video (can we get date of video?) => !!! ExifLib.ExifLibException: 'File is not a valid JPEG' !!! => for Video need another lib
    ///  4) file is image but not jpeg (example PNG) => ExifLib.ExifLibException: 'File is not a valid JPEG'
    ///  5) file is JPEG but without metadata (png converted fo jpeg) => OK, prints Image has no metadata
    ///  
    ///  B) Positive
    ///  B1) file is real photo => Ok
    ///  B2) file is resized photo (GPS coords dissapeared, but origin date still available) => Ok
    ///  B3) file is JPEG, but extension removed, or replaced to .xyz => Ok, opens and finds date
    /// </remarks>
    /// <param name="imgFullPath"></param>
    /// <param name="pictureTakenAt"></param>
    /// <returns>Is success, i.e when file is image and has meta info of picture taken at date</returns>
    internal class PhotoDateProvider : IPhotoDateProvider
    {
        public bool GetPictureDate(FilePath imgFilePath, out DateTimeOffset pictureTakenAt)
        {            
            try
            {
                return GetPictureDateInternal(imgFilePath.NormalizedFullPath, out pictureTakenAt);
            }
            catch
            {
                pictureTakenAt = DateTimeOffset.MinValue;
                return false;
            }            
        }

        private  bool GetPictureDateInternal(string imgFilePath, out DateTimeOffset pictureTakenAt)
        {
            pictureTakenAt = DateTimeOffset.MinValue;

            using (ExifReader reader = new ExifReader(imgFilePath))
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
