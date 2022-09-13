using Core.Logging;
using System.Web.Http.ExceptionHandling;

namespace Core.WebApi
{
    public class CustomizedExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var contextErrorSource = context.ExceptionContext.ControllerContext?.ControllerDescriptor?.ControllerType ?? typeof(CustomizedExceptionLogger);

            var logger = Logger.GetLogger(contextErrorSource);

            logger.Error($"Unhandled exception processing {context.Request.Method} for {context.Request.RequestUri}: {context.Exception}", context.Exception);
        }
    }

}