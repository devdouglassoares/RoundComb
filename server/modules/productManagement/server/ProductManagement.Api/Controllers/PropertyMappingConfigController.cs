using Core.Exceptions;
using Core.WebApi.ActionFilters;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/propertyImportMappingConfiguration")]
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    public class PropertyImportMappingConfigController : ApiController
    {
        private readonly IPropertyImportMappingService _importMappingService;

        public PropertyImportMappingConfigController(IPropertyImportMappingService importMappingService)
        {
            _importMappingService = importMappingService;
        }

        [HttpGet, Route("")]
        public IHttpActionResult GetAll()
        {
            var result = _importMappingService.GetAll();

            return Ok(result);
        }

        [HttpPost, Route("")]
        public HttpResponseMessage Create(PropertyImportMappingSetDto model)
        {
            model.CreatedDate = DateTimeOffset.Now;

            return Request.CreateResponse(HttpStatusCode.Created, _importMappingService.Create(model));
        }

        [HttpPut, Route("{id}")]
        public HttpResponseMessage Update(long id, PropertyImportMappingSetDto model)
        {
            _importMappingService.Update(model, id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
