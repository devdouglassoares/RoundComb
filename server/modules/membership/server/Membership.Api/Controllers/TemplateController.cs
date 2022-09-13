using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Templating.Models;
using Core.Templating.Services;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/templates")]
    public class TemplateController : ApiController
    {
        protected readonly ITemplateService TemplateService;

        public TemplateController(ITemplateService templateService)
        {
            TemplateService = templateService;
        }

        [HttpGet, Route("")]
        public HttpResponseMessage GetAvailableTemplates()
        {
            return Request.CreateResponse(HttpStatusCode.OK, TemplateService.GetAllAvailableTemplates());
        }

        [HttpGet, Route("{fullTypeName}")]
        public HttpResponseMessage GetTemplate(string fullTypeName)
        {
            return Request.CreateResponse(HttpStatusCode.OK, TemplateService.LoadTemplate(fullTypeName));
        }

        [HttpPost, Route("{fullTypeName}")]
        public HttpResponseMessage SaveTemplate(string fullTypeName, TemplateModelDto model)
        {
            TemplateService.SaveTemplate(fullTypeName, model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}