using Core.DynamicProperties.Controllers;
using CustomForm.Core.Entities;
using CustomForm.Core.Services;
using System.Web.Http;

namespace CustomForm.Api.Controllers
{
    [RoutePrefix("api/formDynamicProperties")]
    public class ProductPropertiesController : DynamicPropertiesController<FormInstance, IDynamicPropertyFormService>
    {
        public ProductPropertiesController(IDynamicPropertyFormService dynamicPropService) : base(dynamicPropService)
        {
        }
    }
}
