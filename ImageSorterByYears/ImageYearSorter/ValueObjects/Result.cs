namespace ImageYearSorter.ValueObjects
{
    public record Result<T>(T? Value, IEnumerable<Invalidation>? Invalidations) 
        where T: class
    {
        public Result(T Value) : this(Value, Array.Empty<Invalidation>())
        {
        }

        public Result(Invalidation message) : this(null, new Invalidation[] { message })
        {
        }

        public Result(IEnumerable<Invalidation> invalidations) : this(null, invalidations)
        {
        }

        public bool IsOk() => Invalidations == null || Invalidations.Count() == 0;

        public static implicit operator Result<T>(T Value)
        {
            return new Result<T>(Value);
        }
    }
}
