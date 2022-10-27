using FileMerger.Domain.Abstract;
using FileMerger.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace FileMerger.Sqlite
{
    internal class SqliteSnapshot : ISnapshot
    {
        private readonly IQueryable<FileEntity> _allFiles;

        public string Host { get; init; }

        public IDictionary<string, object> Tag { get; init; }

        public SqliteSnapshot(string host, IQueryable<FileEntity> allFiles)
        {
            Tag = new Dictionary<string, object>();
            Host = host;
            _allFiles = allFiles;
        }

        public IEnumerable<ComparableEntity> Items => _allFiles.ToList();

        public IReadOnlyCollection<ComparableEntity> Find(FileEntity file)
        {
            Console.WriteLine("Specific sqlite implementation");
            var withSameHash = _allFiles.Where(x => x.Hash == file.Hash).ToList();
            return withSameHash.Where(x => x.Size == x.Size) // search by shortname?
                .Where(x => x.Host != file.Host || x.FullName != file.FullName) // exclude itself from match
                .ToList();
        }
    }
}
