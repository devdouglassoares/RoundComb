//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Configuration;
//using System.Web.Http;
//using System.Web.Http.ModelBinding;
//using Core.Exceptions;
//using Core.UI;
//using Core.UI.DataTablesExtensions;
//using Core.WebApi.ActionFilters;
//using Membership.Core;
//using Membership.Core.Contracts;
//using Membership.Core.Contracts.AuthAttributes;
//using Membership.Core.Dto;
//using Membership.Core.Exceptions;
//
//namespace Membership.Api.Controllers
//{
//    [RoutePrefix("clientCompany")]
//    [RequireAuthTokenApi]
//    [ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
//    public class ClientCompanyController : ApiController
//    {
//        private readonly IClientCompanyService _clientCompanyService;
//        private readonly IDataTableService _dataTableService;
//
//        public ClientCompanyController(IClientCompanyService clientCompanyService, IDataTableService dataTableService)
//        {
//            _clientCompanyService = clientCompanyService;
//            _dataTableService = dataTableService;
//        }
//
//        [HttpGet]
//        public HttpResponseMessage GetAllClientCompany()
//        {
//            var result = _clientCompanyService.GetAll();
//
//            return Request.CreateResponse(HttpStatusCode.OK, result);
//        }
//
//        [HttpPost, Route("datatables")]
//        public HttpResponseMessage GetAllClientCompanyForDatatable([ModelBinder(typeof(DataTableModelBinderProvider))] DefaultDataTablesRequest requestModel)
//        {
//            var myOriginalDataSet = _clientCompanyService.GetAll().ToList();
//
//            var result = _dataTableService.GetResponse(myOriginalDataSet, requestModel);
//
//            return Request.CreateResponse(HttpStatusCode.OK, result);
//        }
//
//        [HttpGet, Route("{clientCompanyId}")]
//        public HttpResponseMessage GetClientCompany(long clientCompanyId)
//        {
//            return Request.CreateResponse(HttpStatusCode.OK, _clientCompanyService.GetById(clientCompanyId));
//        }
//
//        [HttpPost]
//        [ErrorResponseHandler(typeof(UserEmailAlreadyInUsedException), HttpStatusCode.BadRequest)]
//        [ErrorResponseHandler(typeof(CompanyNameInUsedException), HttpStatusCode.BadRequest)]
//        public HttpResponseMessage CreateClientCompany(ClientCompanyWithOwnerDto model)
//        {
//            if (string.IsNullOrEmpty(model.MainContactUser.Password))
//            {
//                model.MainContactUser.Password =
//                    WebConfigurationManager.AppSettings[MembershipConstant.C_DEFAULT_TEMP_PASSWORD_KEY];
//            }
//            _clientCompanyService.CreateClientCompany(model);
//
//            return Request.CreateResponse(HttpStatusCode.Created);
//        }
//
//        [HttpPut, Route("{clientCompanyId}")]
//        public HttpResponseMessage UpdateClientCompany(long clientCompanyId, ClientCompanyWithOwnerDto model)
//        {
//            _clientCompanyService.Update(clientCompanyId, model);
//
//            return Request.CreateResponse(HttpStatusCode.OK);
//        }
//
//        [HttpDelete, Route("{clientCompanyId}")]
//        public HttpResponseMessage DeleteClientCompany(long clientCompanyId)
//        {
//            _clientCompanyService.Delete(clientCompanyId);
//
//            return Request.CreateResponse(HttpStatusCode.OK);
//        }
//    }
//}
