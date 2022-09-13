using System;
using System.IO;

namespace Core.UploadService.FileSystem
{
    public interface IStorageFile
    {
        string GetPath();

        string GetDirectory();

        string GetParentDirectoryName();

        string GetName();
        string GetFullName();
        long GetSize();
        DateTime GetLastUpdated();
        string GetFileType();

        /// <summary>
        /// Creates a stream for reading from the file.
        /// </summary>
        Stream OpenRead();

        byte[] ReadAllBytes();

        string ReadAllText();

        /// <summary>
        /// Creates a stream for writing to the file.
        /// </summary>
        Stream OpenWrite();

        /// <summary>
        /// Creates a stream for writing to the file, and truncates the existing content.
        /// </summary>
        Stream CreateFile();
    }
}