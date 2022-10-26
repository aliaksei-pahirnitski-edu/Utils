using FileMerger.Domain.Abstract;
using FileMerger.Domain.Entity;
using FileMerger.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FileMerger.Sqlite
{
    internal class SqlitePersistService : IPersist
    {
        private readonly FilesSnapshotDbContext _dbCtx;

        public SqlitePersistService(FilesSnapshotDbContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        public ISnapshot ReadFromFile(string fullPath)
        {
            throw new NotImplementedException();
        }

        public string SaveToFile(ISnapshot data, string fullPath)
        {
            if (File.Exists(fullPath))
            {
                // throw error?
                Console.WriteLine($"SQLITE DB ALREADY EXISTS! [{fullPath}]");
                var ext = Path.GetExtension(fullPath);
                var len = fullPath.Length - ext.Length;
                var withoutExt = fullPath[0..len];
                int i = 0;
                do
                {
                    i++;
                    fullPath = withoutExt + '_' + i + ext;
                } while (File.Exists(fullPath));
            }
            // ext from: RelationalDatabaseFacadeExtensions.SetConnectionString()
            _dbCtx.Database.SetConnectionString($"Data Source={fullPath}");
            _dbCtx.Database.EnsureCreated();

            var onlyFiles = data.Items.Select(x => x as FileEntity).Where(x => x != null);
            var filesDbSet = _dbCtx.Set<FileEntity>();
            foreach (var chunk in onlyFiles.Chunk(10))
            {
                foreach (var fileItem in chunk)
                {
                    filesDbSet.Add(fileItem!);
                }
                _dbCtx.SaveChanges();
            }
            return fullPath;
        }

        public string SuggestFileName(ISnapshot data)
        {
            var prefix = "new";
            if (data is TreeSnapshot treeSnapshot)
            {
                prefix = treeSnapshot.Root.ShortName;
            }
            return $"{prefix}_{DateTime.Now:yyyyMMdd}.{Environment.MachineName}.sqlite";
        }
    }
}
