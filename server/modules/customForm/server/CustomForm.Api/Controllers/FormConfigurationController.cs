using Core.Exceptions;
using Core.ObjectMapping;
using Core.UI;
using Core.UI.DataTablesExtensions;
using Core.WebApi.ActionFilters;
using Core.WebApi.Extensions;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;
using CustomForm.Core.Services;
using Membership.Core;
using Membership.Core.Contracts.AuthAttributes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace CustomForm.Api.Controllers
{
    [RoutePrefix("api/formConfiguration")]
    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
    public class FormConfigurationController : ApiController
    {
        private readonly IFormConfigurationService _formConfigurationService;
        private readonly IDataTableService _dataTableService;
        private readonly IMembership _membership;
        private readonly IMappingService _mappingService;

        public FormConfigurationController(IFormConfigurationService formConfigurationService,
                                           IDataTableService dataTableService,
                                           IMembership membership,
                                           IMappingService mappingService)
        {
            _formConfigurationService = formConfigurationService;
            _dataTableService = dataTableService;
            _membership = membership;
            _mappingService = mappingService;
        }

        [HttpGet, HttpHead, Route("")]
        public HttpResponseMessage GetAll()
        {
            var formConfigurationDtos = _formConfigurationService.GetAllDtos().ToArray();

            return Request.CreateResponse(HttpStatusCode.OK, formConfigurationDtos,
                                          formConfigurationDtos.Max(x => x.ModifiedDate));
        }

        [HttpPost, Route("datatables")]
        [RequireAuthTokenApi]
        public HttpResponseMessage GetAllForDataTable([ModelBinder(typeof(DataTableModelBinderProvider))]DefaultDataTablesRequest requestModel)
        {
            var forms = _formConfigurationService.GetAll();

            var result = _mappingService.Project<FormConfiguration, FormConfigurationNoField>(forms);

            return Request.CreateResponse(HttpStatusCode.OK, _dataTableService.GetResponse(result, requestModel));
        }

        [HttpGet, HttpHead, Route("{id:long}")]
        public HttpResponseMessage GetById(long id)
        {
            var formConfigurationDto = _formConfigurationService.GetDto(id);
            return Request.CreateResponse(HttpStatusCode.OK, formConfigurationDto, formConfigurationDto.ModifiedDate);
        }

        [HttpGet, HttpHead, Route("{formCode}")]
        public HttpResponseMessage GetByCode(string formCode)
        {
            var formConfig = _formConfigurationService.First(form => form.FormCode == formCode);
            if (formConfig == null)
                throw new BaseNotFoundException<FormConfiguration>();

            return Request.CreateResponse(HttpStatusCode.OK, _formConfigurationService.ToDto(formConfig), formConfig.ModifiedDate);
        }

        [HttpPost, Route("")]
        [RequireAuthTokenApi(OptionalAuthorization = true)]
        public HttpResponseMessage Create(FormConfigurationDto model)
        {
            if (!model.IsSystemConfig)
                model.OwnerId = _membership.UserId;
            _formConfigurationService.Create(model);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut, Route("{id}")]
        [RequireAuthTokenApi]
        public HttpResponseMessage Update(long id, FormConfigurationDto model)
        {
            _formConfigurationService.Update(model, id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete, Route("{id}")]
        public HttpResponseMessage Delete(long id)
        {
            _formConfigurationService.Delete(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}