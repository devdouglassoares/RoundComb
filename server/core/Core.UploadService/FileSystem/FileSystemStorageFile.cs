using System;
using System.IO;

namespace Core.UploadService.FileSystem
{
    public class FileSystemStorageFile : IStorageFile
    {
        private readonly string _path;
        private readonly FileInfo _fileInfo;

        public FileSystemStorageFile(string path, FileInfo fileInfo)
        {
            _path = path;
            _fileInfo = fileInfo;
        }

        #region Implementation of IStorageFile

        public string GetPath()
        {
            return _path;
        }

        public string GetDirectory()
        {
            return _fileInfo.DirectoryName;
        }

        public string GetParentDirectoryName()
        {
            return _fileInfo.Directory.Name;
        }

        public string GetName()
        {
            return _fileInfo.Name;
        }

        public string GetFullName()
        {
            return _fileInfo.FullName;
        }

        public long GetSize()
        {
            return _fileInfo.Length;
        }

        public DateTime GetLastUpdated()
        {
            return _fileInfo.LastWriteTime;
        }

        public string GetFileType()
        {
            return _fileInfo.Extension;
        }

        public Stream OpenRead()
        {
            return new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Read);
        }

        public byte[] ReadAllBytes()
        {
            return File.ReadAllBytes(_fileInfo.FullName);
        }

        public string ReadAllText()
        {
            return File.ReadAllText(_fileInfo.FullName);
        }

        public Stream OpenWrite()
        {
            return new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.ReadWrite);
        }

        public Stream CreateFile()
        {
            return new FileStream(_fileInfo.FullName, FileMode.Truncate, FileAccess.ReadWrite);
        }

        #endregion
    }
}