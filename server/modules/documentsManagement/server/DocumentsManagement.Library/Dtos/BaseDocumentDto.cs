using System;

namespace DocumentsManagement.Library.Dtos
{
    public class BaseDocumentDto 
    {
        public long Id { get; set; }
        public long FileRecordId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public long MasterId { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
        public string Comment { get; set; }
    }
}