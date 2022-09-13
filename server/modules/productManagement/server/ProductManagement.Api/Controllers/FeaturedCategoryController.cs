using Membership.Core.Contracts.AuthAttributes;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Permissions;
using ProductManagement.Core.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/featuredCategory")]
    [PermissionAuthorize(PropertyManagementPermissions.AccessManageCategories)]
    public class FeaturedCategoryController : ApiController
    {
        private readonly IFeaturedCategoryService _featuredCategoryService;

        public FeaturedCategoryController(IFeaturedCategoryService featuredCategoryService)
        {
            _featuredCategoryService = featuredCategoryService;
        }

        [HttpGet, Route("")]
        [AllowAnonymous]
        public HttpResponseMessage GetAll()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _featuredCategoryService.GetAllDtos().OrderBy(x => x.DisplayOrder));
        }

        [HttpGet, Route("{featuredCategoryId}")]
        public HttpResponseMessage GetById(long featuredCategoryId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _featuredCategoryService.GetEntity(featuredCategoryId));
        }

        [HttpPost, Route("")]
        public HttpResponseMessage Create(FeaturedCategoryDto model)
        {
            _featuredCategoryService.Create(model);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut, Route("{featuredCategoryId}")]
        public HttpResponseMessage Update(long featuredCategoryId, FeaturedCategoryDto model)
        {
            _featuredCategoryService.Update(model, featuredCategoryId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete, Route("{featuredCategoryId}")]
        public HttpResponseMessage Delete(long featuredCategoryId)
        {
            _featuredCategoryService.Delete(featuredCategoryId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
