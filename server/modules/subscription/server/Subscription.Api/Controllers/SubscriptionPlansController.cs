using Core.UI;
using Core.UI.DataTablesExtensions;
using Subscription.Core.Dtos;
using Subscription.Service.Contracts;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Subscription.Api.Controllers
{
    [RoutePrefix("api/subscriptionplans")]
    public class SubscriptionPlansController : ApiController
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly IDataTableService _dataTableService;

        public SubscriptionPlansController(ISubscriptionPlanService subscriptionPlanService, IDataTableService dataTableService)
        {
            _subscriptionPlanService = subscriptionPlanService;
            _dataTableService = dataTableService;
        }

        [HttpPost, Route("datatables")]
        public HttpResponseMessage GetAll([ModelBinder(typeof(DataTableModelBinderProvider))]DefaultDataTablesRequest requestModel)
        {
            var all = _subscriptionPlanService.GetAll();

            var result = _dataTableService.GetResponse(all, requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet, Route("{id}")]
        public HttpResponseMessage GetById(long id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _subscriptionPlanService.GetDto(id));
        }

        [HttpPost, Route("")]
        public HttpResponseMessage Create(SubscriptionPlanDto model)
        {
            _subscriptionPlanService.Create(model);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut, Route("{id}")]
        public HttpResponseMessage Update(long id, SubscriptionPlanDto model)
        {
            _subscriptionPlanService.Update(model, id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete, Route("{id}")]
        public HttpResponseMessage DeleteHttpResponseMessage(long id)
        {
            _subscriptionPlanService.Delete(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
