namespace ImageYearSorter.Models.Dto
{
    /// <summary>Extended info about date when picture was taken at</summary>
    /// <param name="PictureTakenAt"></param>
    /// <param name="Year">year part of the picture date</param>
    /// <param name="Quater">quarter</param>
    /// <param name="YearQuaterPrefix">Ex: 2021Q3</param>
    public record PhotoTakenAtDto(
        DateTimeOffset PictureTakenAt,
        int Year,
        EQuarter Quater,
        string YearQuaterPrefix
        );
}
