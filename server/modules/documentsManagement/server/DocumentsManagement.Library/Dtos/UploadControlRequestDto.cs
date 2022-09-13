namespace DocumentsManagement.Library.Dtos
{
    public class UploadControlRequestDto
    {
        public string Comment { get; set; }
        public long Identifier { get; set; }

        public string FileName { get; set; }
    }
}
