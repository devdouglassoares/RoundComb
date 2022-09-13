using Core.Templating.Models;

namespace ProductManagement.Core.EmailTemplates
{
    public class SendPropertyToVendorTemplate : BaseTemplateModel
    {
        public string LandlordName { get; set; }
    }
}
