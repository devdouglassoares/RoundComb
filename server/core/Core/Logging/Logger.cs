using Core.Logging.Data;
using Core.Logging.Models;
using Core.Logging.Repositories;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Data;

namespace Core.Logging
{
    public class Logger : ILogger
    {
        private readonly ILog _logger;

        public static Logger GetLogger(Type type)
        {
            return new Logger(type);
        }

        public static Logger GetLogger<TType>()
        {
            return new Logger(typeof(TType));
        }

        public Logger(Type type)
        {
            if (!_configured)
                Setup();

            _logger = LogManager.GetLogger(type);
        }

        #region Info Log
        public void Info(string message)
        {
            _logger.Info(message);
        }

        #endregion

        #region Warning Log
        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Warn(Exception exception)
        {
            _logger.Warn(exception);
        }

        public void Warn(string message, Exception exception)
        {
            _logger.Warn(message, exception);
        }
        #endregion

        #region Debug Log
        public void Debug(string message)
        {
            _logger.Debug(message);
        }
        #endregion

        #region Error Log
        public void Error(string message)
        {
            _logger.Error(message);
            SendEmail(message, message);
        }

        public void Error(Exception exception)
        {
            Error(LogUtility.BuildExceptionMessage(exception));
            SendEmail(exception.Message, LogUtility.BuildExceptionMessage(exception));
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception);
            SendEmail(message, LogUtility.BuildExceptionMessage(exception));
        }
        #endregion

        #region Fatal Log
        public void Fatal(string message)
        {
            _logger.Fatal(message);
            SendEmail(message, message);
        }

        public void Fatal(Exception exception)
        {
            Fatal(LogUtility.BuildExceptionMessage(exception));
            SendEmail(exception.Message, LogUtility.BuildExceptionMessage(exception));
        }

        public void Fatal(string message, Exception exception)
        {
            Fatal(LogUtility.BuildExceptionMessage(exception));
            SendEmail(message, LogUtility.BuildExceptionMessage(exception));
        }
        #endregion

        /// <summary>
        /// Send logging email
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void SendEmail(string subject, string body)
        {
        }

        private static bool _configured = false;

        public static void Setup()
        {
            if (_configured)
                return;

            // make sure migration run before setting up the appender
            var loggingContext = new LoggingContext();
            var loggerRepo = new LoggingRepository(loggingContext);
            loggerRepo.GetAll<LogEntry>();

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            var appender = new AdoNetAppender
            {
                ConnectionString = loggingContext.Database.Connection.ConnectionString,
                ConnectionType =
                                   "System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                CommandText = @"
INSERT INTO LogEntry ([Date],[Thread],[Level],[Logger],[Message],[Exception]) 
VALUES (@log_date, @thread, @log_level, @logger, @message, @exception)"
            };



            var logDateParam = new AdoNetAppenderParameter
            {
                DbType = DbType.DateTimeOffset,
                ParameterName = "@log_date",
                Layout = new RawTimeStampLayout()
            };

            var threadParam = new AdoNetAppenderParameter
            {
                ParameterName = "@thread",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%thread"))
            };

            var logLevel = new AdoNetAppenderParameter
            {
                ParameterName = "@log_level",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level"))
            };

            var logger = new AdoNetAppenderParameter
            {
                ParameterName = "@logger",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger"))
            };

            var message = new AdoNetAppenderParameter
            {
                ParameterName = "@message",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%message"))
            };

            var exception = new AdoNetAppenderParameter
            {
                ParameterName = "@exception",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new ExceptionLayout())
            };

            appender.AddParameter(logDateParam);
            appender.AddParameter(threadParam);
            appender.AddParameter(logLevel);
            appender.AddParameter(logger);
            appender.AddParameter(message);
            appender.AddParameter(exception);

            appender.Lossy = false;
            appender.BufferSize = 1;
            appender.ActivateOptions();
            hierarchy.Root.AddAppender(appender);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
            _configured = true;
        }
    }
}
