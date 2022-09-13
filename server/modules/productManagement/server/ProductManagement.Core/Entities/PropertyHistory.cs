using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Core.Entities
{
    public enum PropertyHistoryType
    {
        Edited,
        Deleted,
        UserVisited,
        UserInterested
    }

    public class PropertyHistory
    {
        [Key]
        public long Id { get; set; }

        public long PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }

        public long? UserId { get; set; }

        [NotMapped]
        public PropertyHistoryType PropertyHistoryType { get; set; }

        [Column("PropertyHistoryType")]
        public string PropertyHistoryTypeString
        {
            get { return PropertyHistoryType.ToString(); }
            set { PropertyHistoryType = (PropertyHistoryType)Enum.Parse(typeof(PropertyHistoryType), value, true); }
        }

        public string Comment { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? RecordDate { get; set; }
    }
}