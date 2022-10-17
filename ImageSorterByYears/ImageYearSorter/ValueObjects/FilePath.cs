namespace ImageYearSorter.ValueObjects
{
    /// <summary>Wrapper to string, meaning it is valid full path</summary>
    public record FilePath
    {
        public string FullPath { get; init; }
        public FilePath(string FullPath)
        {
            if (!File.Exists(FullPath)) throw new ArgumentException($"Not valid full file path [{FullPath}]", nameof(FullPath));
            this.FullPath = FullPath;
        }

        public static Result<FilePath> Convert(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                return new FilePath(fullPath);
            }
            return new Result<FilePath>(new Invalidation("Not valid full file path"));
        }
    }
}
