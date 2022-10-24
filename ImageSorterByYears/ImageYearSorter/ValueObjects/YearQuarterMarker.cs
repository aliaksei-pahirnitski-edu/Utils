using ImageYearSorter.Models;

namespace ImageYearSorter.ValueObjects;

/// <summary>Extended info about date when picture was taken at</summary>
/// <param name="Year">year part of the picture date</param>
/// <param name="Quater">quarter</param>
/// <param name="YearQuaterPrefix">Ex: 2021Q3</param>
public sealed class YearQuarterMarker : ValueObject { 
    public int Year { get; init; }
    public EQuarter Quarter { get; init; }
    public string YearQuaterPrefix { get; init; }

    private YearQuarterMarker(int year, EQuarter quarter, string eqPrefix)
    {
        Year = year;
        Quarter = quarter;
        YearQuaterPrefix = eqPrefix;
    }

    override public string ToString() => YearQuaterPrefix;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return YearQuaterPrefix;
    }

    public static EQuarter ToQuarter(DateTimeOffset date)
    {
        var month = date.Month;
        return month switch
        {
            <= 3 => EQuarter.Q1,
            > 3 and <= 6 => EQuarter.Q2,
            > 6 and <= 9 => EQuarter.Q3,
            > 9 => EQuarter.Q4,
        };
    }

    public static YearQuarterMarker Create(DateTimeOffset date)
    {
        var year = date.Year;
        var quarter = ToQuarter(date);
        var prefixYQ = string.Intern(year + quarter.ToString());
        return new YearQuarterMarker(year, quarter, prefixYQ);
    }
}