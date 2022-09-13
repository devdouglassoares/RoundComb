using Common.Core.Entities;
using Common.Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Common.Api.Controllers
{
    [RoutePrefix("api/exceptionLogging")]
    public class ExceptionLoggingController : ApiController
    {
        private readonly IExceptionLoggerService _exceptionLoggerService;

        public ExceptionLoggingController(IExceptionLoggerService exceptionLoggerService)
        {
            _exceptionLoggerService = exceptionLoggerService;
        }

        [HttpPost, Route("")]
        public HttpResponseMessage SaveExceptionLog(ExceptionLoggerDto[] exceptions)
        {
            _exceptionLoggerService.LogExceptions(exceptions);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
