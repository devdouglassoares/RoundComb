using Core.UI;
using Core.UI.DataTablesExtensions;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ProductManagement.Api.Controllers
{
    [RoutePrefix("api/properties/{propertyId}")]
    public class PropertyHistoryController : ApiController
    {
        private readonly IPropertyHistoryService _propertyHistoryService;
        private readonly IDataTableService _dataTableService;

        public PropertyHistoryController(IPropertyHistoryService propertyHistoryService, IDataTableService dataTableService)
        {
            _propertyHistoryService = propertyHistoryService;
            _dataTableService = dataTableService;
        }

        [HttpPost, Route("interestHistory")]
        public HttpResponseMessage GetInterestHistories(long propertyId, [ModelBinder(typeof(DataTableModelBinderProvider))]DefaultDataTablesRequest requestModel)
        {
            var records = _propertyHistoryService.GetPropertyHistoryRecordsGroupByUser(propertyId,
                PropertyHistoryType.UserInterested);

            var result = _dataTableService.GetResponse(records, requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, Route("viewHistory")]
        public HttpResponseMessage GetViewHistories(long propertyId, [ModelBinder(typeof(DataTableModelBinderProvider))]DefaultDataTablesRequest requestModel)
        {
            var records = _propertyHistoryService.GetPropertyHistoryRecordsGroupByUser(propertyId,
                PropertyHistoryType.UserVisited);

            var result = _dataTableService.GetResponse(records, requestModel);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
