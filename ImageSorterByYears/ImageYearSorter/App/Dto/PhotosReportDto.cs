using System.Collections.Immutable;

namespace ImageYearSorter.Contract.Dto
{
    public record PhotosReportDto(
        string FolderPath
        , int CountDirect
        , int CountAll
        , int CountToMove
        , int CountCorrectLocation
        
        , ImmutableSortedDictionary<string, int> ImagesByYearQuarter // folder prefix => count images
        );

}
