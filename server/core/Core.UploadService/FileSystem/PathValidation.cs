using System;
using System.IO;

namespace Core.UploadService.FileSystem
{
    public static class PathValidation
    {
        /// <summary>
        /// Determines if a path lies within the base path boundaries.
        /// If not, an exception is thrown.
        /// </summary>
        /// <param name="basePath">The base path which boundaries are not to be transposed.</param>
        /// <param name="mappedPath">The path to determine.</param>
        /// <returns>The mapped path if valid.</returns>
        /// <exception cref="ArgumentException">If the path is invalid.</exception>
        public static string ValidatePath(string basePath, string mappedPath)
        {
            bool valid;

            try
            {
                // Check that we are indeed within the storage directory boundaries
                valid = Path.GetFullPath(mappedPath).StartsWith(Path.GetFullPath(basePath), StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                // Make sure that if invalid for medium trust we give a proper exception
                valid = false;
            }

            if (!valid)
            {
                throw new ArgumentException("Invalid path");
            }

            return mappedPath;
        }
    }
}