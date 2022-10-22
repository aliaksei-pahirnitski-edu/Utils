using ImageYearSorter.Contract.Dto;
using ImageYearSorter.Utils;
using ImageYearSorter.ValueObjects;
using System.Collections.Immutable;

namespace ImageYearSorter.Models
{
    public sealed class FolderRootModel
    {
        public FolderPath Folder { get; }

        public FolderRootModel(FolderPath folder)
        {
            Folder = folder;
        }
        public FolderRootModel(DirectoryInfo directory) : this(FolderPath.Create(directory.FullName).OkResult)
        {
        }

        public PhotosReportDto GetStatus(IPhotoDateProvider photoDateProvider)
        {
            var directStat = GetStatusDirect(Folder.NormalizedFullPath, photoDateProvider);
            var nestedStat = new FilesStat();
            foreach(var subFolder in Directory.EnumerateDirectories(Folder.NormalizedFullPath))
            {
                // todo
            }

            return new PhotosReportDto(
                Folder.NormalizedFullPath
                , AllYears: directStat.AllYears.OrderBy(x => x).ToList()
                , CountImagesDirect: directStat.Images
                , CountVideosDirect: directStat.Videos
                , CountNotImagesDirect: directStat.Others
                , CountSubfolders: 0
                , CountImagesNested: 0
                , CountVideosNested: 0
                , CountNotImagesNested: 0
                , ImagesByYearQuarter: directStat.ImagesByPrefix.ToImmutableSortedDictionary()
                );
        }

        private FilesStat GetStatusDirect(string subFolder, IPhotoDateProvider photoDateProvider)
        {
            var directStat = new FilesStat();
            foreach (var file in new DirectoryInfo(subFolder).EnumerateFiles())
            {
                var metadataFinder = new FileMetadataModel(file);
                var metadata = metadataFinder.FindMetadata(photoDateProvider);
                if (metadata.IsImage)
                {
                    directStat.Images++;
                } 
                else if (metadata.IsVideo)
                {
                    directStat.Videos++;
                }
                else
                {
                    directStat.Others++;
                }
                if (!directStat.ImagesByPrefix.TryGetValue(metadata.FolderPrefix, out int count))
                {
                    count = 0;
                }
                count++;
                directStat.ImagesByPrefix[metadata.FolderPrefix] = count;

                if (metadata.YearQuarter != null)
                {
                    if (!directStat.AllYears.Contains(metadata.YearQuarter.Year))
                    {
                        directStat.AllYears.Add(metadata.YearQuarter.Year);
                    }
                }
            }
            return directStat;
        }

        private void GetStatusRecursive(string subFolder, IPhotoDateProvider photoDateProvider, FilesStat stat)
        {
            throw new NotImplementedException();
        }

        private class FilesStat
        {
            public int Images;
            public int Videos;
            public int Others;
            public HashSet<int> AllYears = new HashSet<int>();
            public Dictionary<string, int> ImagesByPrefix = new Dictionary<string, int>();
        }
    }
}
