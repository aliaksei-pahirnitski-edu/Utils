using ImageYearSorter.App.Dto;
using ImageYearSorter.Utils;
using ImageYearSorter.ValueObjects;

namespace ImageYearSorter.Models
{
    public class FileMetadataModel
    {
        private readonly FilePath _filePath;
        private bool _metadataInited = false;

        public DateTimeOffset? TakenAt { get; private set; }
        public YearQuarterMarker? YearQuarter { get; private set; }
                
        /// <summary>Constructor</summary>
        public FileMetadataModel(FilePath filePath)
        {
            _filePath = filePath;
        }

        /// <summary>Depending on YearQuarter and if it is video or image or other</summary>
        public string? FolderPrefix() {
            if (!_metadataInited) throw new Exception("Find Metadata before");

            if (_filePath.IsImage)
            {
                return YearQuarter?.YearQuaterPrefix;
            }
            else if (_filePath.IsVideo)
            {
                return YearQuarter?.YearQuaterPrefix + "Video";
            }
            else
            {
                return "Other";
            }
        }

        public FileMetadataDto FindMetadata(IPhotoDateProvider photoDateProvider)
        {
            bool isImage = _filePath.IsImage;
            bool isVideo = _filePath.IsVideo;
            var fileCreationTime = File.GetCreationTime(_filePath.NormalizedFullPath);
            var fileLastWriteTime = File.GetLastWriteTime(_filePath.NormalizedFullPath);
            if (isImage)
            {
                if (photoDateProvider.GetPictureDate(_filePath, out DateTimeOffset pictureTakenAt))
                {
                    TakenAt = pictureTakenAt;
                    YearQuarter = YearQuarterMarker.Create(pictureTakenAt);
                }
            } 
            else if (isVideo)
            {
                TakenAt = fileLastWriteTime;
                YearQuarter = YearQuarterMarker.Create(fileLastWriteTime);
            }
            _metadataInited = true;

            return new FileMetadataDto(
                ImageFullPath: _filePath.NormalizedFullPath
                , Extention: _filePath.Extension
                , IsImage: isImage
                , IsVideo: isVideo
                , HasMetadata: isImage && YearQuarter != null
                , TakenAt: TakenAt
                , YearQuarter: YearQuarter
                , FileCreatedAt: fileCreationTime
                , FileModifiedAt: fileLastWriteTime);
        }
    }
}
