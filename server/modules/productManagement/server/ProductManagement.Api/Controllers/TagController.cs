using Core.WebApi.ActionFilters;
using Core.WebApi.Controllers;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/tags")]
    [ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
    public class TagController : BaseCrudController<Tag, TagDto, ITagService>
    {
        public TagController(ITagService crudService) : base(crudService)
        {
        }

        public override HttpResponseMessage GetAll()
        {
            return GetWithFiltering(string.Empty);
        }

        [HttpGet, Route("query={query}")]
        public HttpResponseMessage GetWithFiltering(string query)
        {
            var all = CrudService.Fetch(tag => string.IsNullOrEmpty(query) || tag.Name.ToLower().Contains(query.ToLower()));

            return Request.CreateResponse(HttpStatusCode.OK, CrudService.ToDtos(all));
        }
    }
}
