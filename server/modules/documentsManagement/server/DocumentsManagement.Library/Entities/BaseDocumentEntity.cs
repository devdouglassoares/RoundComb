using System.ComponentModel.DataAnnotations.Schema;
using Core.Database.Entities;

namespace DocumentsManagement.Library.Entities
{
    public class BaseDocumentEntity  : BaseEntity
    {                                                         
        public long FileRecordId { get; set; }

        [ForeignKey("FileRecordId")]
        public virtual FileRecord FileRecord { get; set; }

        public long MasterId { get; set; }
        public string Comment { get; set; }
    }
}