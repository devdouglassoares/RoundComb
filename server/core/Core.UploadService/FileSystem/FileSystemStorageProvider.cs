using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Core.UploadService.FileSystem
{
    public class FileSystemStorageProvider : IStorageProvider
    {
        private readonly string _storagePath;
        private readonly string _virtualPath;
        private readonly string _publicPath;

        public FileSystemStorageProvider()
        {
            var mediaPath = HostingEnvironment.IsHosted
                                ? HostingEnvironment.MapPath("~/Media/") ?? ""
                                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");

            _storagePath = Path.Combine(mediaPath);
            _virtualPath = "~/Media/";

            var appPath = "";
            if (HostingEnvironment.IsHosted)
            {
                appPath = HostingEnvironment.ApplicationVirtualPath;
            }
            if (appPath != null && !appPath.EndsWith("/"))
                appPath = appPath + '/';
            if (appPath != null && !appPath.StartsWith("/"))
                appPath = '/' + appPath;

            _publicPath = appPath + "Media/";
        }


        /// <summary>
        /// Maps a relative path into the storage path.
        /// </summary>
        /// <param name="path">The relative path to be mapped.</param>
        /// <returns>The relative path combined with the storage path.</returns>
        private string MapStorage(string path)
        {
            string mappedPath = string.IsNullOrEmpty(path) ? _storagePath : Path.Combine(_storagePath, path);
            return PathValidation.ValidatePath(_storagePath, mappedPath);
        }

        /// <summary>
        /// Maps a relative path into the public path.
        /// </summary>
        /// <param name="path">The relative path to be mapped.</param>
        /// <returns>The relative path combined with the public path in an URL friendly format ('/' character for directory separator).</returns>
        private string MapPublic(string path)
        {
            return string.IsNullOrEmpty(path) ?
                _publicPath :
                Path.Combine(_publicPath, path).Replace(Path.DirectorySeparatorChar, '/').Replace(" ", "%20");
        }

        public string MapPath(string path)
        {
            if (!FolderExists(path))
                CreateFolder(path);

            var virtualPath = string.IsNullOrEmpty(path) ?
                _virtualPath :
                Path.Combine(_virtualPath, path).Replace(Path.DirectorySeparatorChar, '/').Replace(" ", "%20");

            return HostingEnvironment.MapPath(virtualPath);
        }

        private static string Fix(string path)
        {
            return string.IsNullOrEmpty(path)
                       ? ""
                       : Path.DirectorySeparatorChar != '/'
                             ? path.Replace('/', Path.DirectorySeparatorChar)
                             : path;
        }

        /// <summary>
        /// Checks if the given file exists within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>True if the file exists; False otherwise.</returns>
        public bool FileExists(string path)
        {
            return File.Exists(MapStorage(path));
        }

        /// <summary>
        /// Retrieves the public URL for a given file within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>The public URL.</returns>
        public string GetPublicUrl(string path)
        {
            return MapPublic(path);
        }

        /// <summary>
        /// Retrieves the path within the storage provider for a given public url.
        /// </summary>
        /// <param name="url">The virtual or public url of a media.</param>
        /// <returns>The storage path or <value>null</value> if the media is not in a correct format.</returns>
        public string GetStoragePath(string url)
        {
            if (url.StartsWith(_virtualPath))
            {
                return url.Substring(_virtualPath.Length).Replace('/', Path.DirectorySeparatorChar).Replace("%20", " ");
            }

            if (url.StartsWith(_publicPath))
            {
                return url.Substring(_publicPath.Length).Replace('/', Path.DirectorySeparatorChar).Replace("%20", " "); ;
            }

            return null;
        }

        /// <summary>
        /// Retrieves a file within the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file within the storage provider.</param>
        /// <returns>The file.</returns>
        /// <exception cref="ArgumentException">If the file is not found.</exception>
        public IStorageFile GetFile(string path)
        {
            var fileInfo = new FileInfo(MapStorage(path));
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException(string.Format("File {0} does not exist", path));
            }

            return new FileSystemStorageFile(Fix(path), fileInfo);
        }

        /// <summary>
        /// Lists the files within a storage provider's path.
        /// </summary>
        /// <param name="path">The relative path to the folder which files to list.</param>
        /// <returns>The list of files in the folder.</returns>
        public IEnumerable<IStorageFile> ListFiles(string path)
        {
            var directoryInfo = new DirectoryInfo(MapStorage(path));
            if (!directoryInfo.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} does not exist", path));
            }

            return directoryInfo
                .GetFiles()
                .Where(fi => !IsHidden(fi))
                .Select<FileInfo, IStorageFile>(fi => new FileSystemStorageFile(Path.Combine(Fix(path), fi.Name), fi))
                .ToList();
        }

        /// <summary>
        /// Checks if the given folder exists within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>True if the folder exists; False otherwise.</returns>
        public bool FolderExists(string path)
        {
            return new DirectoryInfo(MapStorage(path)).Exists;
        }

        /// <summary>
        /// Lists the folders within a storage provider's path.
        /// </summary>
        /// <param name="path">The relative path to the folder which folders to list.</param>
        /// <returns>The list of folders in the folder.</returns>
        public IEnumerable<IStorageFolder> ListFolders(string path)
        {
            var directoryInfo = new DirectoryInfo(MapStorage(path));
            if (!directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Create();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(string.Format("The folder could not be created at path: {0}. {1}", path, ex));
                }
            }

            return directoryInfo.GetDirectories()
                                .Where(di => !IsHidden(di))
                                .Select<DirectoryInfo, IStorageFolder>(
                                    di => new FileSystemStorageFolder(Path.Combine(Fix(path), di.Name), di))
                                .ToList();
        }

        /// <summary>
        /// Tries to create a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be created.</param>
        /// <returns>True if success; False otherwise.</returns>
        public bool TryCreateFolder(string path)
        {
            try
            {
                // prevent unnecessary exception
                var directoryInfo = new DirectoryInfo(MapStorage(path));
                if (directoryInfo.Exists)
                {
                    return false;
                }

                CreateFolder(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be created.</param>
        /// <exception cref="ArgumentException">If the folder already exists.</exception>
        public void CreateFolder(string path)
        {
            var directoryInfo = new DirectoryInfo(MapStorage(path));
            if (directoryInfo.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} already exists", path));
            }

            Directory.CreateDirectory(directoryInfo.FullName);
        }

        /// <summary>
        /// Deletes a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be deleted.</param>
        /// <exception cref="ArgumentException">If the folder doesn't exist.</exception>
        public void DeleteFolder(string path)
        {
            var directoryInfo = new DirectoryInfo(MapStorage(path));
            if (!directoryInfo.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} does not exist", path));
            }

            directoryInfo.Delete(true);
        }

        /// <summary>
        /// Renames a folder in the storage provider.
        /// </summary>
        /// <param name="oldPath">The relative path to the folder to be renamed.</param>
        /// <param name="newPath">The relative path to the new folder.</param>
        public void RenameFolder(string oldPath, string newPath)
        {
            var sourceDirectory = new DirectoryInfo(MapStorage(oldPath));
            if (!sourceDirectory.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} does not exist", oldPath));
            }

            var targetDirectory = new DirectoryInfo(MapStorage(newPath));
            if (targetDirectory.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} already exists", newPath));
            }

            Directory.Move(sourceDirectory.FullName, targetDirectory.FullName);
        }

        /// <summary>
        /// Deletes a file in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be deleted.</param>
        /// <exception cref="ArgumentException">If the file doesn't exist.</exception>
        public void DeleteFile(string path)
        {
            var fileInfo = new FileInfo(MapStorage(path));
            if (!fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", path));
            }

            fileInfo.Delete();
        }

        /// <summary>
        /// Renames a file in the storage provider.
        /// </summary>
        /// <param name="oldPath">The relative path to the file to be renamed.</param>
        /// <param name="newPath">The relative path to the new file.</param>
        public void RenameFile(string oldPath, string newPath)
        {
            var sourceFileInfo = new FileInfo(MapStorage(oldPath));
            if (!sourceFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", oldPath));
            }

            var targetFileInfo = new FileInfo(MapStorage(newPath));
            if (targetFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} already exists", newPath));
            }

            File.Move(sourceFileInfo.FullName, targetFileInfo.FullName);
        }

        /// <summary>
        /// Creates a file in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be created.</param>
        /// <exception cref="ArgumentException">If the file already exists.</exception>
        /// <returns>The created file.</returns>
        public IStorageFile CreateFile(string path)
        {
            var fileInfo = new FileInfo(MapStorage(path));
            if (fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} already exists", fileInfo.Name));
            }

            // ensure the directory exists
            var dirName = Path.GetDirectoryName(fileInfo.FullName);
            if (dirName != null && !Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            File.WriteAllBytes(fileInfo.FullName, new byte[0]);

            return new FileSystemStorageFile(Fix(path), fileInfo);
        }

        /// <summary>
        /// Tries to save a stream in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be created.</param>
        /// <param name="inputStream">The stream to be saved.</param>
        /// <returns>True if success; False otherwise.</returns>
        public bool TrySaveStream(string path, Stream inputStream)
        {
            try
            {
                SaveStream(path, inputStream);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves a stream in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be created.</param>
        /// <param name="inputStream">The stream to be saved.</param>
        /// <exception cref="ArgumentException">If the stream can't be saved due to access permissions.</exception>
        public void SaveStream(string path, Stream inputStream)
        {
            // Create the file.
            // The CreateFile method will map the still relative path
            var file = CreateFile(path);

            var outputStream = file.OpenWrite();
            var buffer = new byte[8192];
            for (;;)
            {

                var length = inputStream.Read(buffer, 0, buffer.Length);
                if (length <= 0)
                    break;
                outputStream.Write(buffer, 0, length);
            }
            outputStream.Dispose();
        }

        /// <summary>
        /// Combines to paths.
        /// </summary>
        /// <param name="path1">The parent path.</param>
        /// <param name="path2">The child path.</param>
        /// <returns>The combined path.</returns>
        public string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public static bool IsHidden(FileSystemInfo di)
        {
            return (di.Attributes & FileAttributes.Hidden) != 0;
        }
    }
}