using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Services;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Core.DynamicProperties.Controllers
{
    public abstract class DynamicPropertiesController<TTargetEntity, TDynamicPropService> : ApiController
        where TTargetEntity : class
        where TDynamicPropService : IDynamicPropertyService
    {
        private readonly TDynamicPropService _dynamicPropService;

        protected DynamicPropertiesController(TDynamicPropService dynamicPropService)
        {
            _dynamicPropService = dynamicPropService;
        }

        [HttpGet, Route("")]
        public virtual HttpResponseMessage GetAllPropertiesForTargetEntity()
        {
            var result = _dynamicPropService.GetDyamicPropertiesForEntity<TTargetEntity>();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet, Route("getDynamicPropSupportedTypes")]
        public virtual HttpResponseMessage GetHasDynamicPropertyEntityTypes()
        {
            var result = _dynamicPropService.GetHasDynamicPropertyEntityTypes();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet, Route("getFilterable")]
        public virtual HttpResponseMessage GetFilterableDynamicProperties(long? configId)
        {
            var result = _dynamicPropService.GetFilterableProperties(configId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet, Route("config/{configId}")]
        public virtual HttpResponseMessage GetAllPropertyPropertiesForCategory(long configId)
        {
            var result = _dynamicPropService.GetDynamicPropertiesForConfig(configId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet, Route("propertyTypes")]
        public virtual HttpResponseMessage GetAllPropertiesType()
        {
            var result = _dynamicPropService.GetAvailablePropertyType();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, Route("")]
        public virtual HttpResponseMessage CreateDynamicProperty(DynamicPropertyModel model)
        {
            _dynamicPropService.CreateDynamicProperty<TTargetEntity>(model);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut, Route("{id:int}")]
        public virtual HttpResponseMessage UpdateDynamicProperty(long id, DynamicPropertyModel model)
        {
            _dynamicPropService.Update<TTargetEntity>(id, model);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpGet, Route("{id:int}")]
        public virtual HttpResponseMessage GetDynamicProperty(long id)
        {
            var property = _dynamicPropService.GetDto<TTargetEntity>(id);
            return Request.CreateResponse(HttpStatusCode.OK, property);
        }

        [HttpPost, Route("{propertyId:int}/assignTo/{configId}")]
        public virtual HttpResponseMessage AssignPropertyToConfig(long propertyId, long configId)
        {
            _dynamicPropService.AssignToConfig(propertyId, configId);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpDelete, Route("{propertyId:int}/assignTo/{configId}")]
        public virtual HttpResponseMessage UnAssignPropertyFromConfig(long propertyId, long configId)
        {
            _dynamicPropService.RemoveFromConfig(propertyId, configId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete, Route("{id:int}")]
        public virtual HttpResponseMessage DeleteDynamicProperty(long id)
        {
            _dynamicPropService.Delete<TTargetEntity>(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}