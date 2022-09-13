using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Core.UploadService.FileSystem
{
    public interface IVirtualPathProvider : ISingletonDependency
    {
        string Combine(params string[] paths);
        string ToAppRelative(string virtualPath);
        string MapPath(string virtualPath);

        bool FileExists(string virtualPath);
        bool TryFileExists(string virtualPath);
        Stream OpenFile(string virtualPath);
        StreamWriter CreateText(string virtualPath);
        Stream CreateFile(string virtualPath);
        DateTime GetFileLastWriteTimeUtc(string virtualPath);
        string GetFileHash(string virtualPath);
        string GetFileHash(string virtualPath, IEnumerable<string> dependencies);
        void DeleteFile(string virtualPath);

        bool DirectoryExists(string virtualPath);
        void CreateDirectory(string virtualPath);
        string GetDirectoryName(string virtualPath);
        void DeleteDirectory(string virtualPath);

        IEnumerable<string> ListFiles(string path);
        IEnumerable<string> ListDirectories(string path);
    }

    public class DefaultVirtualPathProvider : IVirtualPathProvider
    {
        public virtual string GetDirectoryName(string virtualPath)
        {
            try
            {
                var directoryName = Path.GetDirectoryName(virtualPath);
                return directoryName != null ? directoryName.Replace(Path.DirectorySeparatorChar, '/') : "";
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public virtual IEnumerable<string> ListFiles(string path)
        {
            return HostingEnvironment.VirtualPathProvider
                                     .GetDirectory(path)
                                     .Files
                                     .OfType<VirtualFile>()
                                     .Select(f => VirtualPathUtility.ToAppRelative(f.VirtualPath));
        }

        public virtual IEnumerable<string> ListDirectories(string path)
        {
            return HostingEnvironment.VirtualPathProvider
                                     .GetDirectory(path)
                                     .Directories
                                     .OfType<VirtualDirectory>()
                                     .Select(d => VirtualPathUtility.ToAppRelative(d.VirtualPath));
        }

        public virtual string Combine(params string[] paths)
        {
            return Path.Combine(paths).Replace(Path.DirectorySeparatorChar, '/');
        }

        public virtual string ToAppRelative(string virtualPath)
        {
            if (IsMalformedVirtualPath(virtualPath))
                return null;

            try
            {
                var result = VirtualPathUtility.ToAppRelative(virtualPath);

                return !result.StartsWith("~/") ? null : result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public virtual Stream OpenFile(string virtualPath)
        {
            return HostingEnvironment.VirtualPathProvider.GetFile(virtualPath).Open();
        }

        public virtual StreamWriter CreateText(string virtualPath)
        {
            return File.CreateText(MapPath(virtualPath));
        }

        public virtual Stream CreateFile(string virtualPath)
        {
            return File.Create(MapPath(virtualPath));
        }

        public virtual DateTime GetFileLastWriteTimeUtc(string virtualPath)
        {
            return File.GetLastWriteTimeUtc(MapPath(virtualPath));
        }

        public string GetFileHash(string virtualPath)
        {
            return GetFileHash(virtualPath, new[] { virtualPath });
        }

        public string GetFileHash(string virtualPath, IEnumerable<string> dependencies)
        {
            return HostingEnvironment.VirtualPathProvider.GetFileHash(virtualPath, dependencies);
        }

        public virtual void DeleteFile(string virtualPath)
        {
            File.Delete(MapPath(virtualPath));
        }

        public virtual string MapPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }

        public virtual bool FileExists(string virtualPath)
        {
            return HostingEnvironment.VirtualPathProvider.FileExists(virtualPath);
        }

        public virtual bool TryFileExists(string virtualPath)
        {
            if (IsMalformedVirtualPath(virtualPath))
                return false;

            try
            {
                return FileExists(virtualPath);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public virtual bool DirectoryExists(string virtualPath)
        {
            return HostingEnvironment.VirtualPathProvider.DirectoryExists(virtualPath);
        }

        public virtual void CreateDirectory(string virtualPath)
        {
            Directory.CreateDirectory(MapPath(virtualPath));
        }

        public virtual void DeleteDirectory(string virtualPath)
        {
            Directory.Delete(MapPath(virtualPath));
        }

        /// <summary>
        /// We want to reject path that contains ".." going outside of the application root.
        /// ToAppRelative does that already, but we want to do the same while avoiding exceptions.
        /// 
        /// Note: This method doesn't detect all cases of malformed paths, it merely checks
        ///       for *some* cases of malformed paths, so this is not a replacement for full virtual path
        ///       verification through VirtualPathUtilty methods.
        ///       In other words, !IsMalformed does *not* imply "IsWellformed".
        /// </summary>
        public bool IsMalformedVirtualPath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                return true;

            if (virtualPath.IndexOf("..", StringComparison.Ordinal) < 0)
                return false;

            virtualPath = virtualPath.Replace(Path.DirectorySeparatorChar, '/');
            var rootPrefix = virtualPath.StartsWith("~/") ? "~/" : virtualPath.StartsWith("/") ? "/" : "";
            if (string.IsNullOrEmpty(rootPrefix))
                return false;

            var terms = virtualPath.Substring(rootPrefix.Length).Split('/');
            var depth = 0;
            foreach (var term in terms)
            {
                if (term == "..")
                {
                    if (depth == 0)
                    {
                        return true;
                    }
                    depth--;
                }
                else
                {
                    depth++;
                }
            }

            return false;
        }
    }
}