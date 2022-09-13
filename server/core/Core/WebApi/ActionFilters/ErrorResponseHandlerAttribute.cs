using Core.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Core.WebApi.ActionFilters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ErrorResponseHandlerAttribute : ExceptionFilterAttribute
    {
        private readonly Type _exceptionType;
        private readonly HttpStatusCode _httpStatusCode;
        private readonly string _errorMessage;
        public string ErrorCode { get; set; }

        public ErrorResponseHandlerAttribute(Type exceptionType) : this(exceptionType, HttpStatusCode.InternalServerError)
        {
        }

        public ErrorResponseHandlerAttribute(Type exceptionType, HttpStatusCode statusCode, string errorMessage = "")
        {
            _exceptionType = exceptionType;
            _httpStatusCode = statusCode;
            _errorMessage = string.IsNullOrEmpty(errorMessage) ? null : errorMessage;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var logger = Logger.GetLogger(actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerType);
            logger.Error(actionExecutedContext.Exception);

            var exception = actionExecutedContext.Exception;

            var errorMessages = new List<string>();

            errorMessages.Add(exception.Message);

            var ex = exception;
            if (ex.InnerException != null)
            {
                while ((ex = ex.InnerException) != null)
                {
                    errorMessages.Add(ex.Message);
                }
            }

            var exceptionMessage = string.Join(". ", errorMessages);

            if (_exceptionType.IsInstanceOfType(exception) ||
                (_exceptionType.IsGenericType && exception.GetType().IsGenericType && _exceptionType.IsAssignableFrom(exception.GetType().GetGenericTypeDefinition())))
            {
                var response = actionExecutedContext.Request.CreateResponse(_httpStatusCode, new
                {
                    errorCode = ErrorCode,
                    errorMessage = _errorMessage ?? exceptionMessage
                });
                actionExecutedContext.Response = response;
                return;
            }


            if (_exceptionType.IsInstanceOfType(typeof(Exception)))
            {
                var response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    errorCode = ErrorCode,
                    errorMessage = _errorMessage ?? exceptionMessage
                });
                actionExecutedContext.Response = response;
                return;

            }

            base.OnException(actionExecutedContext);
        }
    }
}