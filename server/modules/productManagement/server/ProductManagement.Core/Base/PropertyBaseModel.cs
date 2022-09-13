using Core.Database.Entities;
using ProductManagement.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Core.Base
{
    public abstract class PropertyBaseModel : BaseEntity
    {
        public string Name { get; set; }        

        public long? CategoryId { get; set; }

        [Column("Description")]
        public string FullDescription { get; set; }

        public string ShortDescription { get; set; }

        public string ExternalKey { get; set; }

        public string Sku { get; set; }

        /// <summary>
        /// Pack Upc of the product
        /// </summary>
        public string Upc { get; set; }

        /// <summary>
        /// Use for individual items - item UPC
        /// </summary>
        public string UnitUpc { get; set; }

        public long? ParentPropertyId { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }

        [DataType(DataType.Currency)]
        public decimal? UnitPrice { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsActive { get; set; }


        [NotMapped]
        public PropertyStatus Status { get; set; }

        [NotMapped]
        public PropertySellType? PropertySellType { get; set; }

        public long? OwnerId { get; set; }

        public long? LocationId { get; set; }

        public string UnitNumber { get; set; }

        protected PropertyBaseModel()
        {
            Status = PropertyStatus.Draft;
        }
    }
}