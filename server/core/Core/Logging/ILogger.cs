using System;

namespace Core.Logging
{
    public interface ILogger
    {
        void Info(string message);

        void Warn(Exception exception);
        void Warn(string message, Exception exception);
        void Warn(string message);

        void Debug(string message);

        void Error(Exception exception);
        void Error(string message, Exception exception);
        void Error(string message);

        void Fatal(Exception exception);
        void Fatal(string message, Exception exception);
        void Fatal(string message);
    }
}