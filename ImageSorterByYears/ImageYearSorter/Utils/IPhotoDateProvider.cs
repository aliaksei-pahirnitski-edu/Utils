using ImageYearSorter.ValueObjects;

namespace ImageYearSorter.Utils
{
    public interface IPhotoDateProvider
    {
        /// <summary>
        /// Gets date when picture was taken at
        /// </summary>
        /// <remarks>
        /// To be tested:
        ///  0) not valid path
        ///  1) file is not existing
        ///  2) file is not an image (txt)
        ///  3) file is video (can we get date of video?)
        ///  4) OK: file is real photo
        ///  5) OK: file is resized photo (GPS coords dissapeared, but origin date still available)
        /// </remarks>
        /// <param name="imgFullPath"></param>
        /// <param name="pictureTakenAt"></param>
        /// <returns>Is success, i.e when file is image and has meta info of picture taken at date</returns>
        bool GetPictureDate(FilePath imgFullPath, out DateTimeOffset pictureTakenAt);
    }
}
