using Core.UploadService.FileSystem;

namespace Core.UploadService
{
    public class FileUploaded
    {
        public string FileName { get; set; }

        public string FolderPath { get; set; }

        public string FilePath { get; set; }
    }

    public class NonExistsImageDimessionFileRequested
    {
        public IStorageFile OriginalFile { get; set; }

        public int RequestedWidth { get; set; }
    }
}