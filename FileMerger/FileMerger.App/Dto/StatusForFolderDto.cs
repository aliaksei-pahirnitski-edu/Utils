using FileMerger.Domain.Entity;

namespace FileMerger.App.Dto
{
    public record StatusForFolderDto(bool Exists, FolderEntity Folder);
}
