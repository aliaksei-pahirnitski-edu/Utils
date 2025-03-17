using ImageYearSorter.Models;

namespace ImageYearSorter.ValueObjects
{
    public sealed class MoveItem : ValueObject
    {
        public string RootFolder { get; init; }
        public string RelativeFilePath { get; init; }
        public string FolderToMoveTo { get; init; }
        public bool IsInCorrectLocation => RelativeFilePath.StartsWith(FolderToMoveTo);

        /// <summary>
        /// When prefix has "!" (ex 2015! or 2016Q2!) it means should not move files
        /// It is when metadata might not be corect and folder was named manually
        /// </summary>
        public bool IsManuallyLocation => HasExclamationMark(RelativeFilePath);
        public bool NeedToBeMoved => !IsManuallyLocation && !IsInCorrectLocation;

        public string NewFilePath() {
            if (IsInCorrectLocation) return RelativeFilePath;
            var relativeDir = Path.GetDirectoryName(RelativeFilePath);
            var addUnderscore = (relativeDir?.Length ?? 0) > 0 ? "_" : "";
            return Path.Combine(FolderToMoveTo + addUnderscore + relativeDir, Path.GetFileName(RelativeFilePath));
        }

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

        /// <summary>
        /// Exceptions manually renamed by user: 2014!..., 2015Q4..., 2017Vid!...
        /// </summary>
        private bool HasExclamationMark(ReadOnlySpan<char> relativeFilePath)
        {
            const char exclamation = '!';

            // updated: if folder starts or ends with ! then do not move it
            if (relativeFilePath.Length < 1) return false;
            if (relativeFilePath[0] == exclamation) return true;
            if (relativeFilePath.IndexOf(exclamation) > 0)
            {
                var dirName = Path.GetDirectoryName(relativeFilePath);
                if (dirName.Length > 0 && dirName[^1] == exclamation) return true;
            }

            // old logic was to name manually like 202X! or 2016Q2!
            if (relativeFilePath.Length < 5) return false;

            bool yearStartsWith19or20 = 
                (relativeFilePath[0] == '1' && relativeFilePath[1] == '9')                            
                || (relativeFilePath[0] == '2' && relativeFilePath[1] == '0');
            if (!yearStartsWith19or20)
            {
                return false;
            }
            if (relativeFilePath[4] == exclamation) return true; // Example 2016! prefix

            if (relativeFilePath.Length < 7) return false;
            if (relativeFilePath[4] != 'Q') return false; // Prefix should be like yearQx.., ex 2021Q1
            if (relativeFilePath[6] == exclamation) return true; // Example 2016Q4! prefix

            if (relativeFilePath.Length < 7 + FileMetadataModel.CVideoPrefix.Length) return false;
            for (int i = 0; i < FileMetadataModel.CVideoPrefix.Length; i++)
            {
                if (relativeFilePath[6 + i] != FileMetadataModel.CVideoPrefix[i]) return false;
            }
            if (relativeFilePath[6 + FileMetadataModel.CVideoPrefix.Length] == exclamation) return true; // Example 2016Q4Vid! prefix

            return false;
        }
    }
}
