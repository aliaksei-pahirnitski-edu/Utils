using ImageYearSorter.ValueObjects;

namespace ImageYearSorter.App.Dto
{
    /// <summary>
    /// Full result with verbose of command to find picture creation info
    /// </summary>
    /// <param name="ImageFullPath"></param>
    /// <param name="Extention"></param>
    /// <param name="IsImage">by extension</param>
    /// <param name="IsVideo">by extension</param>
    /// <param name="HasMetadata"> = IsImage and PictureTakenAt != null</param>
    /// <param name="PictureTakenAt">picture by metadata or video by last write time</param>
    /// <param name="FileCreatedAt">Look like it is not needed as it is time when it was copied to current folder</param>
    /// <param name="FileModifiedAt">Looks like it matched PictureTakenAt, so will use it for video</param>
    public record FileMetadataDto(
        string ImageFullPath,
        string Extention,
        bool IsImage,
        bool IsVideo,
        bool HasMetadata,
        DateTimeOffset? TakenAt,
        YearQuarterMarker? YearQuarter,
        DateTimeOffset FileCreatedAt,
        DateTimeOffset FileModifiedAt
        );

}
