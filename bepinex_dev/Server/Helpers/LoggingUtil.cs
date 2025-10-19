using SPTarkov.Server.Core.Models.Utils;

namespace DansDevTools.Helpers
{
    public static class LoggingUtil
    {
        private static ISptLogger<DansDevToolsServer>? logger = null;

        public static void Init(ISptLogger<DansDevToolsServer> logger)
        {
            LoggingUtil.logger = logger;
        }

        public static void LogInfo(string message)
        {
            if (logger == null)
            {
                throw new InvalidOperationException("Logger has not been initialized.");
            }

            logger.Info(message);
        }

        public static void LogWarning(string message)
        {
            if (logger == null)
            {
                throw new InvalidOperationException("Logger has not been initialized.");
            }

            logger.Warning(message);
        }

        public static void LogError(string message)
        {
            if (logger == null)
            {
                throw new InvalidOperationException("Logger has not been initialized.");
            }

            logger.Error(message);
        }
    }
}