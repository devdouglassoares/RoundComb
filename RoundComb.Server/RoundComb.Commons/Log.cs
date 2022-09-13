using NLog;
using NLog.Config;
using NLog.Targets;

namespace CertidaoJudicial.Commons
{
    public static class Log
    {
        public static Logger Instance { get; private set; }

        static Log()
        {
            LogManager.ReconfigExistingLoggers();

            Instance = LogManager.GetCurrentClassLogger();
        }
    }
}
