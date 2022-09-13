namespace CustomForm.Core.Entities
{
    public class FormFieldFormConfiguration
    {
        public long FormConfigurationId { get; set; }

        public virtual FormConfiguration FormConfiguration { get; set; }

        public long FormFieldId { get; set; }

        public virtual FormField FormField { get; set; }

        public string DisplaySectionCode { get; set; }

        public int DisplayOrder { get; set; }

        public bool Display { get; set; }

        public bool IsRequired { get; set; }
    }
}