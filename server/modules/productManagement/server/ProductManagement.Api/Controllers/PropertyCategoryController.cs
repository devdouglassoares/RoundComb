using Core.Exceptions;
using Core.UI;
using Core.UI.DataTablesExtensions;
using Core.WebApi.ActionFilters;
using Core.WebApi.Extensions;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Permissions;
using ProductManagement.Core.Services;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/propertyCategories")]
    [PermissionAuthorize(PropertyManagementPermissions.AccessManageCategories)]
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    [ErrorResponseHandler(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized)]
    public class PropertyCategoryController : ApiController
    {
        private readonly IPropertyCategoryService _categoryService;
        private readonly IDataTableService _dataTableService;
        private readonly IMembership _membership;

        public PropertyCategoryController(IPropertyCategoryService categoryService, IDataTableService dataTableService,
            IMembership membership)
        {
            _categoryService = categoryService;
            _dataTableService = dataTableService;
            _membership = membership;
        }

        [HttpPost, Route("datatables")]
        public HttpResponseMessage GetAllWithPagination([ModelBinder(typeof(DataTableModelBinderProvider))]DefaultDataTablesRequest requestModel)
        {
            var result = _categoryService.GetAllDtos();

            var response = _dataTableService.GetResponse(result, requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet, HttpHead, Route("")]
        [AllowAnonymous]
        public HttpResponseMessage GetAll()
        {
            var result = _categoryService.GetAllDtos();

            return Request.CreateResponse(HttpStatusCode.OK, result, result.Max(x => x.ModifiedDate));
        }

        [HttpGet, Route("HomePage")]
        [AllowAnonymous]
        public HttpResponseMessage GetAllForHomePage()
        {
            var result = _categoryService.GetAll().Where(x => x.DisplayOnHomePage).OrderBy(x => x.DisplayOrder);

            return Request.CreateResponse(HttpStatusCode.OK, _categoryService.ToDtos(result));
        }

        [HttpGet, HttpHead, Route("tree")]
        [AllowAnonymous]
        public HttpResponseMessage GetAllAsTree()
        {
            var result = _categoryService.GetAllAsTree();

            return Request.CreateResponse(HttpStatusCode.OK, _categoryService.ToDtos(result),
                                          _categoryService.GetAll().Max(x => x.ModifiedDate));
        }

        [HttpGet, Route("{categoryId}")]
        [AllowAnonymous]
        public HttpResponseMessage GetById(long categoryId)
        {
            var category = _categoryService.GetDto(categoryId);

            return Request.CreateResponse(HttpStatusCode.OK, category);
        }

        [HttpPost, Route("")]
        public HttpResponseMessage Create(PropertyCategoryDto model)
        {
            _categoryService.Create(model);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPost, Route("createOrUpdate")]
        [AllowAnonymous]
        public HttpResponseMessage CreateOrUpdate(PropertyCategoryDto model)
        {
            _categoryService.CreateOrUpdate(model);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut, Route("{categoryId}")]
        public HttpResponseMessage Update(long categoryId, PropertyCategoryDto model)
        {
            _categoryService.Update(model, categoryId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPut, Route("bulkUpdate")]
        public HttpResponseMessage UpdateCategoryPosition(IEnumerable<PropertyCategoryDto> model)
        {
            _categoryService.Update(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete, Route("{categoryId}")]
        public HttpResponseMessage Delete(long categoryId)
        {
            _categoryService.Delete(categoryId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
