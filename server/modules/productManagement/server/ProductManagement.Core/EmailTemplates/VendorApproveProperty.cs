using Core.Templating.Models;

namespace ProductManagement.Core.EmailTemplates
{
    public class VendorApproveProperty : BaseTemplateModel
    {
        public string UserName { get; set; }
        public string VendorName { get; set; }
    }
}
