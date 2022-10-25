using FileMerger.Domain.Entity;
using FilesHashComparer.Domain.Result;

namespace FileMerger.App.Dto
{
    public record StatusForFileDto(bool Exists, FileEntity File, IReadOnlyCollection<MatchItemResult> Matches);
}
