using ImageYearSorter.Models.Dto;

namespace ImageYearSorter.Models
{
    public class PhotoYearQuaterModel
    {
        public DateTimeOffset PictureDate { get; init; }
        public int Year { get; init; }
        public EQuarter Quarter { get; init; }
        public string PrefixYQ{ get; init; }

        public PhotoYearQuaterModel(DateTimeOffset date) 
        {
            PictureDate = date;
            Year = date.Year;
            Quarter = ToQuarter(date);
            PrefixYQ = Year + Quarter.ToString();
        }

        public static EQuarter ToQuarter(DateTimeOffset date)
        {
            var month = date.Month;
            return month switch
            {
                <= 3 => EQuarter.Q1,
                > 3 and <= 6 => EQuarter.Q2,
                > 6 and <= 9 => EQuarter.Q2,
                > 9 => EQuarter.Q4,
            };
        }

        public PhotoTakenAtDto AsDto() => new PhotoTakenAtDto(PictureDate, Year, Quarter, PrefixYQ);
    }
}
