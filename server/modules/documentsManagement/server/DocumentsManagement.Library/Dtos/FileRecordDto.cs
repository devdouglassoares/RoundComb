using System;

namespace DocumentsManagement.Library.Dtos
{
    public class FileRecordDto
    {
        public long Id { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public DateTimeOffset? UploadedDate { get; set; }
        public string UploadedBy { get; set; }
    }
}