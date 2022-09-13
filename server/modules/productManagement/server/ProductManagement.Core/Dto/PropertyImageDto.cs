namespace ProductManagement.Core.Dto
{
    public class PropertyImageDto
    {
        public long Id { get; set; }

        public string Url { get; set; }

        public bool IsDeleted { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsMainImage { get; set; }
    }
}