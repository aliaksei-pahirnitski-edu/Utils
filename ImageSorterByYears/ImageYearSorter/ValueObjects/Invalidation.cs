namespace ImageYearSorter.ValueObjects
{
    public record Invalidation(string Text, string? Param)
    {
        public Invalidation(string Text) : this(Text, null) { }
        public string GetMessage() => string.Format(Text, Param);
    }
}
