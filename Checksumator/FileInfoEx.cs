using System.IO;

namespace Checksumator
{
    class FileInfoEx
    {
        private FileInfo _fileInfo;
        private Checksum _checksum;

        public FileInfoEx(string fileName)
        {
            _fileInfo = new FileInfo(fileName);
        }

        public DirectoryInfo Directory { get { return _fileInfo.Directory; } }
        public string DirectoryName { get { return _fileInfo.DirectoryName; } }
        public bool Exists { get { return _fileInfo.Exists; } }
        public string Extension { get { return _fileInfo.Extension; } }
        public string FullName { get { return _fileInfo.FullName; } }
        public long Length { get { return _fileInfo.Length; } }
        public string Name { get { return _fileInfo.Name; } }
        public Checksum Hash
        {
            set { _checksum = value; }
            get { return _checksum; }
        }
    }

    class Checksum
    {
        private string _hash;
        private algo _algo;

        public string Hash { get { return _hash; } }
        public algo Algo { get { return _algo; } }

        public Checksum(algo algo, string hash)
        {
            _algo = algo;
            _hash = hash;
        }
    }

    enum algo
    {
        MD5 =0,
        SHA1 = 1,
        SHA2 = 2,
        SHA3 = 3,
        CRC32 = 4,
        CRC64 = 5
    }
}
