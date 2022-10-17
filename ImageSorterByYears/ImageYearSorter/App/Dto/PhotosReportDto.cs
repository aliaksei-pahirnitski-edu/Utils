namespace ImageYearSorter.Contract.Dto
{
    public record PhotosReportDto(
        string folderPath
        , int CountNotImages
        , IReadOnlyDictionary<int, int> CountByYears
        );
}
