using System.Collections.Immutable;

namespace ImageYearSorter.Contract.Dto
{
    public record PhotosReportDto(
        string FolderPath
        , IReadOnlyCollection<int> AllYears
        , int CountImagesDirect
        , int CountVideosDirect
        , int CountNotImagesDirect
        , int CountSubfolders
        , int CountImagesNested
        , int CountVideosNested
        , int CountNotImagesNested
        
        , ImmutableSortedDictionary<string, int> ImagesByYearQuarter // eq prefix => count images
        );
}
