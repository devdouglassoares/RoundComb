namespace ProductManagement.Core.Entities
{
    public class PropertyImage 
    {
        public long Id { get; set; }

        public string Url { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Property Property { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsMainImage { get; set; }
    }
}