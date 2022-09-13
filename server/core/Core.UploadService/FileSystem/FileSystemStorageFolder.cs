using System;
using System.IO;

namespace Core.UploadService.FileSystem
{
    public class FileSystemStorageFolder : IStorageFolder
    {
        private readonly string _path;
        private readonly DirectoryInfo _directoryInfo;

        public FileSystemStorageFolder(string path, DirectoryInfo directoryInfo)
        {
            _path = path;
            _directoryInfo = directoryInfo;
        }

        #region Implementation of IStorageFolder

        public string GetPath()
        {
            return _path;
        }

        public string GetName()
        {
            return _directoryInfo.Name;
        }

        public DateTime GetLastUpdated()
        {
            return _directoryInfo.LastWriteTime;
        }

        public long GetSize()
        {
            return GetDirectorySize(_directoryInfo);
        }

        public IStorageFolder GetParent()
        {
            if (_directoryInfo.Parent != null)
            {
                return new FileSystemStorageFolder(Path.GetDirectoryName(_path), _directoryInfo.Parent);
            }
            throw new ArgumentException(string.Format("Directory {0} does not have a parent directory", _directoryInfo.Name));
        }

        #endregion

        private static long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            long size = 0;

            var fileInfos = directoryInfo.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (!FileSystemStorageProvider.IsHidden(fileInfo))
                {
                    size += fileInfo.Length;
                }
            }
            var directoryInfos = directoryInfo.GetDirectories();
            foreach (var dInfo in directoryInfos)
            {
                if (!FileSystemStorageProvider.IsHidden(dInfo))
                {
                    size += GetDirectorySize(dInfo);
                }
            }

            return size;
        }
    }
}