using Core.Templating.Models;

namespace ProductManagement.Core.EmailTemplates
{
    public class PropertyApplicationFormApprovedTemplate : BaseTemplateModel
    {
        public long PropertyId { get; set; }

        public string PropertyName { get; set; }

        public string ApproveDate { get; set; }

        public string UserName { get; set; }

    }
}