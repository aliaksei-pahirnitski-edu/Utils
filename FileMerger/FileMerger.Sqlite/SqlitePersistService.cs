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
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(fullPath);
            }
            _dbCtx.Database.SetConnectionString($"Data Source={fullPath}");

            var host = ParseHostFromFileName(fullPath);
            if (string.IsNullOrEmpty(host))
            {
                throw new Exception("File name should contain host as sufix in format 'prefix_date.suffix.sqlite'");
            }
            return new SqliteSnapshot(host, _dbCtx.Set<FileEntity>());
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

        private string ParseHostFromFileName(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var suffix = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(suffix) && suffix[0] == '.')
            {
                return suffix[1..];
            }
            return string.Empty;
        }
    }
}
