using Core.UI;
using Core.UI.DataTablesExtensions;
using Membership.Core.Contracts.AuthAttributes;
using Subscription.Service.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Subscription.Api.Controllers
{
    [RoutePrefix("api/invoice")]
    [RequireAuthTokenApi]
    public class InvoiceController : ApiController
    {
        private readonly IInvoiceServices _invoiceServices;
        private readonly IDataTableService _dataTableService;

        public InvoiceController(IInvoiceServices invoiceServices, IDataTableService dataTableService)
        {
            _invoiceServices = invoiceServices;
            _dataTableService = dataTableService;
        }

        [HttpGet, Route("myinvoices")]
        public HttpResponseMessage GetForCurrentUser()
        {
            var result = _invoiceServices.GetInvoicesForCurrentUser();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, Route("myinvoices/datatable")]
        public HttpResponseMessage GetForCurrentUser([ModelBinder(typeof(DataTableModelBinderProvider))]DefaultDataTablesRequest requestModel)
        {
            var result = _invoiceServices.GetInvoicesForCurrentUser().AsQueryable();
            return Request.CreateResponse(HttpStatusCode.OK, _dataTableService.GetResponse(result, requestModel));
        }
    }
}