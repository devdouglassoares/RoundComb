using Core.Templating.Models;

namespace ProductManagement.Core.EmailTemplates
{
    public class PropertyApplicationFormSubmittedTemplate : BaseTemplateModel
    {
        public long PropertyId { get; set; }

        public string PropertyName { get; set; }

        public string SubmittedDate { get; set; }

        public string Comment { get; set; }

        public string PropertyOwnerName { get; set; }

        public string ApplicantName { get; set; }
    }
}