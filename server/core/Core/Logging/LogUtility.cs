using System;
using System.Text;

namespace Core.Logging
{
    public class LogUtility
    {
        public static string BuildExceptionMessage(Exception exception)
        {

            var logException = exception;
            
            var sb = new StringBuilder();

            try
            {
                sb.AppendLine("\n Error in Path :" + System.Web.HttpContext.Current.Request.Path);

                // Get the QueryString along with the Virtual Path
                sb.AppendLine("\n Raw Url :" + System.Web.HttpContext.Current.Request.RawUrl);
            }
            catch
            {
            }

            // Get the error message
            sb.AppendLine("\n Message :" + logException.Message);

            // Source of the message
            sb.AppendLine("\n Source :" + logException.Source);

            // Stack Trace of the error
            sb.AppendLine("\n Stack Trace :" + logException.StackTrace);

            // Method where the error occurred
            sb.AppendLine("\n TargetSite :" + logException.TargetSite);


            while ((logException = logException.InnerException) != null)
            {
                sb.AppendLine("\n--------------- Inner Exception: ---------------");

                // Get the error message
                sb.AppendLine("\n--------------- Message :" + logException.Message);

                // Source of the message
                sb.AppendLine("\n--------------- Source :" + logException.Source);

                // Stack Trace of the error
                sb.AppendLine("\n--------------- Stack Trace :" + logException.StackTrace);

                // Method where the error occurred
                sb.AppendLine("\n--------------- TargetSite :" + logException.TargetSite);
            }

            return sb.ToString();
        }
    }
}