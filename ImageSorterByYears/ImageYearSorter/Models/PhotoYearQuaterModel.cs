using ImageYearSorter.Models.Dto;
using ImageYearSorter.ValueObjects;

namespace ImageYearSorter.Models
{
    public class PhotoYearQuaterModel
    {
        public DateTimeOffset PictureDate { get; init; }
        public YearQuarterMarker YearQuarter { get; init; }

        public PhotoYearQuaterModel(DateTimeOffset date) 
        {
            PictureDate = date;
            YearQuarter = YearQuarterMarker.Create(date);
        }

        public YearQuarterMarkerDto AsDto() => YearQuarter.AsDto() with { TakenAt = PictureDate };
    }
}
