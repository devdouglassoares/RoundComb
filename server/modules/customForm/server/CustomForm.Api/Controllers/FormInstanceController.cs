using Core.DynamicProperties.Dtos;
using Core.Exceptions;
using Core.WebApi;
using Core.WebApi.ActionFilters;
using Core.WebApi.Controllers;
using Core.WebApi.Extensions;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;
using CustomForm.Core.Services;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace CustomForm.Api.Controllers
{
    [RoutePrefix("api/formInstance")]
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    [ErrorResponseHandler(typeof(InvalidOperationException), HttpStatusCode.BadRequest)]
    [ErrorResponseHandler(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized)]
    public class FormInstanceController : BaseCrudController<FormInstance, FormInstanceDto, IFormInstanceService>
    {
        private readonly IMembership _membership;

        public FormInstanceController(IFormInstanceService crudService, IMembership membership) : base(crudService)
        {
            _membership = membership;
        }

        [RequireAuthTokenApi]
        public override HttpResponseMessage Create(FormInstanceDto model)
        {
            return base.Create(model);
        }

        [RequireAuthTokenApi]
        [HttpGet, HttpHead, Route("mine")]
        public HttpResponseMessage GetCurrentUserFormInstance()
        {
            var result = CrudService.First(x => x.UserId == _membership.UserId && x.FormConfiguration.IsSystemConfig);

            if (result == null)
                throw new BaseNotFoundException<FormInstance>();

            var formInstanceDto = CrudService.ToDto(result);
            return Request.CreateResponse(HttpStatusCode.OK, formInstanceDto, formInstanceDto.ModifiedDate);
        }

        [HttpGet, Route("{formIds}")]
        public HttpResponseMessage GetForms([ModelBinder(typeof(CommaDelimitedArrayModelBinder))]long[] formIds)
        {
            var result = CrudService.Fetch(x => formIds.Contains(x.Id));

            return Request.CreateResponse(HttpStatusCode.OK, CrudService.ToDtos(result));
        }

        [HttpPost, Route("code/{formCode}")]
        public HttpResponseMessage GetFormsByCode(string formCode, Dictionary<long, DynamicPropertyFilterModel> dynamicPropertyFilters)
        {
            var result = CrudService.QueryFormByCode(formCode, dynamicPropertyFilters);

            return Request.CreateResponse(HttpStatusCode.OK, CrudService.ToDtos(result));
        }

        [RequireAuthTokenApi]
        public override HttpResponseMessage Update(long id, FormInstanceDto model)
        {
            return base.Update(id, model);
        }
    }
}
