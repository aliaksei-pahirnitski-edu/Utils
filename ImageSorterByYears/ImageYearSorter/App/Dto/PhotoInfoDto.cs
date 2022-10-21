using ImageYearSorter.Models.Dto;

namespace ImageYearSorter.App.Dto
{
    /// <summary>
    /// Full result with verbose of command to find picture creation info
    /// </summary>
    /// <param name="ImageFullPath"></param>
    /// <param name="Extention"></param>
    /// <param name="IsImage"></param>
    /// <param name="IsVideo"></param>
    /// <param name="PictureTakenAt"></param>
    /// <param name="FileCreatedAt"></param>
    /// <param name="FileModifiedAt"></param>
    public record PhotoInfoDto(
        string ImageFullPath,
        string Extention,
        bool IsImage,
        bool IsVideo,
        YearQuarterMarkerDto? PictureTakenAt,
        DateTimeOffset FileCreatedAt,
        DateTimeOffset FileModifiedAt
        );

}
