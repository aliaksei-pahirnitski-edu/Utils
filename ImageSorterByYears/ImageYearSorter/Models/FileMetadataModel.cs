using ImageYearSorter.App.Dto;
using ImageYearSorter.Utils;
using ImageYearSorter.ValueObjects;

namespace ImageYearSorter.Models
{
    public class FileMetadataModel
    {
        private readonly FilePath _filePath;

        public DateTimeOffset? TakenAt { get; private set; }
        public YearQuarterMarker? YearQuarter { get; private set; }

        public FilePath FilePath => _filePath;

        /// <summary>The constructor</summary>
        public FileMetadataModel(FilePath filePath)
        {
            _filePath = filePath;
        }        

        /// <summary>The constructor when we iterating by existing files</summary>
        public FileMetadataModel(FileInfo finfo) : this(FilePath.Create(finfo.FullName).OkResult)
        { 
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

            var folderPrefix = string.Empty;
            if (isImage && YearQuarter != null)
            {
                folderPrefix = YearQuarter?.YearQuaterPrefix!;
            }
            else if (_filePath.IsVideo)
            {
                folderPrefix = YearQuarter?.YearQuaterPrefix + "Vid";
            }
            else
            {
                folderPrefix = "Other";
            }

            return new FileMetadataDto(
                ImageFullPath: _filePath.NormalizedFullPath
                , Extention: _filePath.Extension
                , IsImage: isImage && YearQuarter != null
                , IsVideo: isVideo
                , TakenAt: TakenAt
                , YearQuarter: YearQuarter
                , FileCreatedAt: fileCreationTime
                , FileModifiedAt: fileLastWriteTime
                , FolderPrefix: folderPrefix
                );
        }
    }
}
