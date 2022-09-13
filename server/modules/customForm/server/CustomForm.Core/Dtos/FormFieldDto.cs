using Core.DynamicProperties.Models;

namespace CustomForm.Core.Dtos
{
    public class FormFieldDto
    {
        public long Id { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public string LocalizationKey { get; set; }

        public bool UseTranslation
        {
            get { return !string.IsNullOrEmpty(LocalizationKey); }
        }

        public PropertyType PropertyType { get; set; }


        public string UploadEndpointUrl { get; set; }

        public bool MultipleUpload { get; set; }

        public string[] AvailableOptions { get; set; }
    }
}