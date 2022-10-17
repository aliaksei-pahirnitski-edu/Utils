namespace ImageYearSorter.ValueObjects
{
    /// <summary>Wrapper to string, meaning it is valid full path</summary>
    public record FolderPath
    {
        public string FullPath { get; init; }
        public FolderPath(string FullPath)
        {
            if (!Directory.Exists(FullPath)) throw new ArgumentException($"Not valid folder [{FullPath}]", nameof(FullPath));
            this.FullPath = FullPath;
        }

        public static Result<FolderPath> Convert(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                return new FolderPath(fullPath);
            }
            return new Result<FolderPath>(new Invalidation("Not valid full folder", fullPath));
        }
    }
}
