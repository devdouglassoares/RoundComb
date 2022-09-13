using Core.UploadService.FileSystem;

namespace Core.UploadService
{
    public class UploadResult
    {
        public string FileName { get; set; }

        public string FileUrl { get; set; }

		public string AbsoluteUrl { get; set; }

        public IStorageFile FilePath { get; set; }
    }
}