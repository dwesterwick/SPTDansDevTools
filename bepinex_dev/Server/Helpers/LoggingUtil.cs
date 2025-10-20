using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Utils;

namespace DansDevTools.Helpers
{
    [Injectable(InjectionType.Singleton)]
    public class LoggingUtil(ISptLogger<DansDevToolsServer> logger)
    {
        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warning(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }
    }
}