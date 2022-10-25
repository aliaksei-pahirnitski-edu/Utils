using System.Security.Cryptography;
using FileMerger.Domain.Abstract;
using FileMerger.Domain.Entity;

namespace FilesHashComparer.Scan
{
    public class HashScanner : IScanner
    {
        private string[] DefaultIgnore = new[] { "node_modules", "bin", "Debug" };

        public IList<string> IgnoreFolders { get; }

        public HashScanner(/*IList<string>? ignoreFolders = null*/) //IOption config...
        {
            IList<string>? ignoreFolders = null;
            IgnoreFolders = ignoreFolders ?? DefaultIgnore;
            for (int i = 0; i < IgnoreFolders.Count; i++)
            {
                IgnoreFolders[i] = IgnoreFolders[i].ToLower();
            }
        }

        public FileEntity ScanFile(string fullPath)
        {
            using (var md5 = MD5.Create())
            {
                var finfo = new FileInfo(fullPath);
                return ScanFile(finfo, md5);
            }
        }

        private FileEntity ScanFile(FileInfo finfo, MD5 md5)
        {
            var result = new FileEntity(finfo.FullName, Environment.MachineName)
            {
                ShortName = finfo.Name,
                Size = finfo.Length,
            };

            try
            {
                // file can be locked
                result.Hash = HashHelper.MakeHash(finfo.FullName, md5);
            }
            catch (IOException exc)
            {
                var debug = exc;
                // throw;
                result.Hash = "Ecxeption" + Guid.NewGuid(); // NOTE: reverted xc->cx intentionally to more easily find
            }

            return result;
        }

        public FolderEntity ScanFolder(string fullPath)
        {
            // not needed - as indirectly applide inside DirectoryInfo / FileInfo
            // fullPath = Path.GetFullPath(fullPath);
            using (var md5 = MD5.Create())
            {
                var rootDir = new DirectoryInfo(fullPath);
                return ScanFolder(rootDir, md5);
            }
        }

        private FolderEntity ScanFolder(DirectoryInfo rootDir, MD5 md5)
        {
            var directChildren = new List<ComparableEntity>();
            var rootFolder = new FolderEntity(rootDir.FullName, Environment.MachineName)
            {
                Children = directChildren,
                ShortName = rootDir.Name,
            };

            var lowerDirName = rootDir.Name.ToLower();
            if (IgnoreFolders != null && IgnoreFolders.Any(x => x == lowerDirName))
            {
                // ignore children
                rootFolder.Size = -1;
                rootFolder.Hash = "ignored";
                return rootFolder;
            }

            foreach (var fileInfo in rootDir.EnumerateFiles())
            {
                var childItem = ScanFile(fileInfo, md5);
                childItem.Parent = rootFolder;
                directChildren.Add(childItem);
            }

            foreach (var childDir in rootDir.EnumerateDirectories())
            {
                var childItem = ScanFolder(childDir, md5);
                childItem.Parent = rootFolder;
                directChildren.Add(childItem);
            }

            rootFolder.Size = directChildren.Select(x => x.Size).DefaultIfEmpty(0).Sum();
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    foreach (var child in directChildren)
                    {
                        writer.WriteLine(child.Hash);
                    }
                    writer.Flush();

                    ms.Position = 0;
                    rootFolder.Hash = HashHelper.MakeHash(ms, md5);
                }
            }

            return rootFolder;
        }
    }
}
