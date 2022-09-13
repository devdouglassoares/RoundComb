namespace CustomForm.Core.Dtos
{
    public class FormFieldFormConfigurationDto
    {
        public long Id { get; set; }

        public long FormConfigurationId { get; set; }

        public long FormFieldId { get; set; }

        public virtual FormFieldDto FormField { get; set; }

        public int DisplayOrder { get; set; }

        public bool Display { get; set; }

        public bool IsRequired { get; set; }
    }
}