namespace ImageYearSorter.ValueObjects
{
    public sealed class MoveItem : ValueObject
    {
        public string RootFolder { get; init; }
        public string RelativeFilePath { get; init; }
        public string FolderToMoveTo { get; init; }
        public bool IsInCorrectLocation => RelativeFilePath.StartsWith(FolderToMoveTo);
        public string NewFilePath => IsInCorrectLocation
            ? RelativeFilePath
            : Path.Combine(FolderToMoveTo, RelativeFilePath);

        public MoveItem(FolderPath rootPath, FilePath filePath, string folderPrefix)
        {
            RootFolder = rootPath.NormalizedFullPath;
            RelativeFilePath = Path.GetRelativePath(RootFolder, filePath.NormalizedFullPath);
            FolderToMoveTo = folderPrefix;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return RelativeFilePath;
        }
    }
}
