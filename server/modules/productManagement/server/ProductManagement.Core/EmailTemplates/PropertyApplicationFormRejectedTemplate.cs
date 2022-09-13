using Core.Templating.Models;

namespace ProductManagement.Core.EmailTemplates
{
    public class PropertyApplicationFormRejectedTemplate : BaseTemplateModel
    {
        public long PropertyId { get; set; }

        public string PropertyName { get; set; }

        public string RejectedDate { get; set; }

        public string Comment { get; set; }

        public string UserName { get; set; }
    }
}