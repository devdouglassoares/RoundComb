namespace ProductManagement.Core.Settings
{
    public class PropertyImportMappingConfig
    {
        public string FromField { get; set; }

        public string ToField { get; set; }

        public bool ImportAsExternalKey { get; set; }

        public bool ImportAsImage { get; set; }

        public string ImageUrlPrefix { get; set; }

        public bool ImportAsDateTime { get; set; }

        public string DateTimeFormat { get; set; }
    }
}