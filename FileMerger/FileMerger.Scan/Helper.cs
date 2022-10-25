using System.Security.Cryptography;
using System.Text;

namespace FilesHashComparer.Scan
{
    static class HashHelper
    {
        public static string MakeHash(string filename)
        {
            using (var md5 = MD5.Create())
            {
                var hash = MakeHash(filename, md5);
                return hash;
            }
        }

        public static string MakeHash(string filename, MD5 md5)
        {
            using (var stream = File.OpenRead(filename))
            {
                var hash = MakeHash(stream, md5);
                return hash;
            }
        }

        public static string MakeHash(Stream stream, MD5 md5)
        {
            var bytes = md5.ComputeHash(stream);
            var hash = ToX2Str(bytes);
            return hash;
        }

        private static string ToX2Str(byte[] bytes)
        {
            var result = new StringBuilder();
            foreach (byte b in bytes) result.Append(b.ToString("x2"));
            return result.ToString();
        }
    }
}