namespace ImageYearSorter.ValueObjects;

public record Error(Exception Exception)
    : Invalidation(Exception.Message);
