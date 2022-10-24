using System.Collections.Immutable;

namespace ImageYearSorter.Contract.Dto
{
    public record PhotosReportDto(
        string FolderPath
        , int CountDirect
        , int CountAll
        , int CountToMove
        , int CountCorrectOrManualLocation

        , ImmutableSortedDictionary<string, int> ImagesByYearQuarterMove // folder prefix => count images (only to be moved to)
        , ImmutableSortedDictionary<string, int> ImagesByYearQuarterManual // folder prefix => count images (like 2016! or 2016Q2! prefix !)
        , ImmutableSortedDictionary<string, int> ImagesByYearQuarterCorrect // folder prefix => count images
        );

}
