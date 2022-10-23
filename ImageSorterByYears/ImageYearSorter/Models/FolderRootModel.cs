using ImageYearSorter.Contract.Dto;
using ImageYearSorter.Utils;
using ImageYearSorter.ValueObjects;
using System.Collections.Immutable;
using System.Diagnostics;

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

        public IReadOnlyCollection<MoveItem> PrepareWork(IPhotoDateProvider photoDateProvider)
        {
            var sw = Stopwatch.StartNew();
            var work = new List<MoveItem>();
            CollectWorkDirect(Folder.NormalizedFullPath, photoDateProvider, work);
            CollectWorkRecursive(Folder.NormalizedFullPath, photoDateProvider, work);
            sw.Stop();
            Console.WriteLine($"Work collected! It took {sw.Elapsed:hh\\:mm\\:ss\\.fff}");
            return work;
        }

        private void CollectWorkDirect(string subFolder, IPhotoDateProvider photoDateProvider, List<MoveItem> work)
        {
            foreach (var file in new DirectoryInfo(subFolder).EnumerateFiles())
            {
                var metadataFinder = new FileMetadataModel(file);
                var metadata = metadataFinder.FindMetadata(photoDateProvider);

                var item = new MoveItem(Folder, metadataFinder.FilePath, metadata.FolderPrefix);
                work.Add(item);
            }
        }

        private void CollectWorkRecursive(string aFolder, IPhotoDateProvider photoDateProvider, List<MoveItem> work)
        {
            foreach (var subFolder in Directory.EnumerateDirectories(aFolder))
            {
                CollectWorkDirect(subFolder, photoDateProvider, work);
                CollectWorkRecursive(subFolder, photoDateProvider, work);
            }
        }

        public PhotosReportDto GetStatus(IPhotoDateProvider photoDateProvider)
        {
            var allWork = PrepareWork(photoDateProvider);

            var statByPrefixes = allWork.GroupBy(x => x.FolderToMoveTo).ToDictionary(x => x.Key, x => x.Count());
            var directWork = allWork.Where(x => Path.GetDirectoryName(x.RelativeFilePath) == string.Empty).ToList();
            var actualWork = allWork.Where(x => !x.IsInCorrectLocation).ToList();

            return new PhotosReportDto(
                Folder.NormalizedFullPath
                , CountDirect: directWork.Count
                , CountAll: allWork.Count
                , CountToMove: actualWork.Count
                , CountCorrectLocation: allWork.Count - actualWork.Count

                , ImagesByYearQuarter: statByPrefixes.ToImmutableSortedDictionary()
                );
        }

        public void Move(IPhotoDateProvider photoDateProvider)
        {
            var allWork = PrepareWork(photoDateProvider);
            var actualWork = allWork.Where(x => !x.IsInCorrectLocation).ToList();

            foreach(var workItem in actualWork)
            {
                var source = Path.Combine(workItem.RootFolder, workItem.RelativeFilePath);
                var destination = Path.Combine(workItem.RootFolder, workItem.NewFilePath());
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                File.Move(source, destination, false);
            }
        }
    }
}
