using Core.Events;
using Core.UploadService.FileSystem;
using System;
using System.IO;
using System.Web;

namespace Core.UploadService
{
    public interface IUploadService : IDependency
    {
        /// <summary>
        /// Upload <see cref="file"/> to specified <see cref="folderPath"/> with specified <see cref="fileName"/>
        /// </summary>
        /// <param name="folderPath">Folder to save the file</param>
        /// <param name="fileName">Name of the file to save</param>
        /// <param name="file">The <see cref="Stream"/> of the file</param>
        /// <returns>Publish URL of the saved file</returns>
        UploadResult UploadFile(string folderPath, string fileName, Stream file);

        UploadResult UploadFile(string folderPath, HttpPostedFileBase file);
    }

    public class UploadService : IUploadService
    {
        private readonly IStorageProvider _storageProvider;
        private readonly IEventPublisher _eventPublisher;

        public UploadService(IStorageProvider storageProvider, IEventPublisher eventPublisher)
        {
            _storageProvider = storageProvider;
            _eventPublisher = eventPublisher;
        }

        public UploadResult UploadFile(string folderPath, string fileName, Stream file)
        {
            var storeFileName = DateTimeOffset.Now.ToString("yyyyMMdd-HHmmss-") + fileName;
            var filePath = _storageProvider.Combine(folderPath, storeFileName);
            _storageProvider.SaveStream(filePath, file);

            _eventPublisher.Publish(new FileUploaded { FolderPath = folderPath, FileName = storeFileName, FilePath = filePath });

            return new UploadResult
            {
                FileUrl = _storageProvider.GetPublicUrl(filePath),
                FileName = fileName,
                FilePath = _storageProvider.GetFile(filePath)
            };
        }

        public UploadResult UploadFile(string folderPath, HttpPostedFileBase file)
        {
            var fileName = file.FileName;
            return UploadFile(folderPath, fileName, file.InputStream);
        }
    }
}