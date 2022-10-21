namespace ImageYearSorter.ValueObjects;

public record Result<T>(T? Value, IReadOnlyCollection<Invalidation>? Invalidations)
    where T: class
{
    private Result(T Value) : this(Value, Array.Empty<Invalidation>())
    {
    }

    private Result(Invalidation message) : this(null, new Invalidation[] { message })
    {
    }

    private Result(IReadOnlyCollection<Invalidation> invalidations) : this(null, invalidations)
    {
    }

    public bool IsOk() => Invalidations == null || Invalidations.Count() == 0;

    public T OkResult => IsOk() ? Value! : throw new ApplicationException("Not OK, please add check for IsOk");

    public static implicit operator Result<T>(T Value)
    {
        return new Result<T>(Value);
    }
    public static Result<T> Create(T Value)
    {
        return new Result<T>(Value);
    }

    public static Result<T> Error(Invalidation error)
    {
        return new Result<T>(error);
    }
    public static Result<T> Error(IReadOnlyCollection<Invalidation> errors)
    {
        return new Result<T>(errors);
    }
}
