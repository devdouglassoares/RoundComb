using System;
using System.Configuration;
using System.Linq;
using System.Text;
using RoundComb.Crypt;

namespace RoundComb.Commons.ExceptionHandling
{
    public sealed class ExceptionManager
    {
        /// <summary>
        /// Private Constructor to ensure that the class cannot be instantiated
        /// </summary>
        private ExceptionManager()
        {}

        public static ServiceExceptionMessage HandleException(Type obj, Exception ex)
        {                      
            // Quando o terceiro parametro é null não efetua a encriptação.
            return HandleException(obj, ex, null);
        }
               
        public static ServiceExceptionMessage HandleException(Type obj, Exception receivedException, string key)
        {
            ServiceExceptionMessage serviceException = BuildServiceExceptionMessage(receivedException);

            LogExceptionMaybeEncrypted(obj, receivedException, key);
            
            return serviceException;
        }

        #region Private

        private static void LogExceptionMaybeEncrypted(Type obj, Exception ex, string key)
        {
            string message = MaybeEncryptMessage(key, ex.Message);
            string exceptionJson = MaybeEncryptMessage(key, ex.ToString());

            LogException(obj, message, new Exception(exceptionJson));
        }

        private static void LogException(Type obj, string message, Exception ex)
        {
            try { logInfo(obj, message, ex); }
            catch { }
        }

        private static string MaybeEncryptMessage(string key, string message)
        {
            if (!string.IsNullOrEmpty(key))
            {                    
                string invector = key.Substring(0, 16);
                string encryptedMessage = Encription.EncriptedString(message, key, invector, 256);

                message = encryptedMessage;
            }

            return message;
        }

        private static ServiceExceptionMessage BuildServiceExceptionMessage(Exception ex)
        {
            ServiceExceptionMessage exception = new ServiceExceptionMessage();

            if (ex != null)
            {
                string[] tipoMsg = ex.Message.Split('§');
                if (tipoMsg.Count() > 0)
                {
                    if (tipoMsg[0] == "-2")
                    {
                        exception.Message = tipoMsg[1];
                    }
                    else
                    {
                        exception.Message = "Ocorreu um erro no serviço de comunicações. Por favor repita a operação.";
                    }
                }
                else
                {
                    exception.Message = "Ocorreu um erro no serviço de comunicações. Por favor repita a operação.";
                }
            }
            else
            {
                exception.Message = "Ocorreu um erro no serviço de comunicações. Por favor repita a operação.";
            }

            exception.Reason = exception.Reason = exception.Message;
            exception.Source = ex.GetType().Name;

            return exception;
        }

        private static void logInfo(Type obj, string pMsg, Exception pEx)
        {
            string pathlogs = GetPath();

            string fileName = GetFileName(pathlogs);

            ILog logFile = LogManager.GetLogger(obj);

            LogManager.Configure(fileName, 100, false);

            logFile.Error(pMsg, pEx);

            LogManager.Shutdown();
        }

        private static string GetFileName(string pathlogs)
        {
            string fileName = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(pathlogs);
            sb.Append("RoundComb");
            sb.Append(Convert.ToString(DateTime.Now.Year));
            sb.Append(Convert.ToString(DateTime.Now.Month));
            sb.Append(Convert.ToString(DateTime.Now.Day));
            sb.Append(".log");
            fileName = sb.ToString();

            return fileName;
        }

        private static string GetPath()
        {
            string pathlogs = string.Empty;
            try
            {
                pathlogs = ConfigurationManager.AppSettings["pathlogs"];

                if (pathlogs == null || pathlogs == string.Empty)
                {
                    pathlogs = AppDomain.CurrentDomain.BaseDirectory;
                    pathlogs = pathlogs + @"\Logs\";
                }
            }
            catch
            {
                pathlogs = AppDomain.CurrentDomain.BaseDirectory;
                pathlogs = pathlogs + @"\Logs\";
            }

            return pathlogs;
        }

        #endregion
    }
}