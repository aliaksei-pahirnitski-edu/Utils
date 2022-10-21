using ImageYearSorter.Contract.Dto;
using ImageYearSorter.ValueObjects;

namespace ImageYearSorter.Models
{
    public sealed class FolderRootModel
    {
        public FolderPath Folder { get; }

        public FolderRootModel(FolderPath folder)
        {
            Folder = folder;
        }

        public PhotosReportDto GetStatus()
        {
            throw new NotImplementedException();
        }

        private PhotosReportDto GetStatusDirect(string subFolder)
        {
            throw new NotImplementedException();
        }
        private PhotosReportDto GetStatusRecursive(string subFolder)
        {
            throw new NotImplementedException();
        }
    }
}
