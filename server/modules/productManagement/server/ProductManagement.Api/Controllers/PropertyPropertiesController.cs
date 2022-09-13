using Core.DynamicProperties.Controllers;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/productProperties")]
    public class PropertyPropertiesController : DynamicPropertiesController<Property, IPropertyPropertyService>
    {
        public PropertyPropertiesController(IPropertyPropertyService dynamicPropService) : base(dynamicPropService)
        {
        }
    }
}
