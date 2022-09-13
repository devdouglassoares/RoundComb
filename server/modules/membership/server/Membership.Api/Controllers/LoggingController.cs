using Core.Logging.Models;
using Core.Logging.Repositories;
using Core.UI;
using Core.UI.DataTablesExtensions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Membership.Api.Controllers
{
    [RoutePrefix("api/loggings")]
    public class LoggingController : ApiController
    {
        private readonly ILoggingRepository _loggingRepository;
        private readonly IDataTableService _dataTableService;

        public LoggingController(ILoggingRepository loggingRepository, IDataTableService dataTableService)
        {
            _loggingRepository = loggingRepository;
            _dataTableService = dataTableService;
        }

        [HttpPost, Route("datatables")]
        public HttpResponseMessage GetLogs([ModelBinder(typeof(DataTableModelBinderProvider))]DefaultDataTablesRequest requestModel)
        {
            var logEntries = _loggingRepository.GetAll<LogEntry>()
                                               .Select(log => new LogEntryModel
                                               {
                                                   Logger = log.Logger,
                                                   Id = log.Id,
                                                   Date = log.Date,
                                                   Level = log.Level,
                                                   Message =
                                                                      log.Message.Length > 30
                                                                          ? log.Message.Substring(0, 30) + "..."
                                                                          : log.Message,
                                                   Thread = log.Thread
                                               });

            return Request.CreateResponse(HttpStatusCode.OK, _dataTableService.GetResponse(logEntries, requestModel));
        }

        [HttpGet, Route("{id}")]
        public HttpResponseMessage GetLogEntry(long id)
        {
            var logEntry = _loggingRepository.Get<LogEntry>(id);

            return Request.CreateResponse(HttpStatusCode.OK, logEntry);
        }
    }
}