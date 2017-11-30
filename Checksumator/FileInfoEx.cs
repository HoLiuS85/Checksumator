using System.IO;

namespace Checksumator
{
    class FileInfoEx
    {
        private FileInfo _fileInfo;
        private string _Hash;

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

        public string Checksum
        {
            get { return _Hash; }
            set { _Hash = value; }
        }

    }
}
