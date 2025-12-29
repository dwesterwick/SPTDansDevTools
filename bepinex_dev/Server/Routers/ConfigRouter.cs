using DansDevTools.Configuration;
using DansDevTools.Helpers;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Utils;

namespace DansDevTools.Routers
{
    [Injectable]
    public class ConfigRouter : AbstractStaticRouter<ModConfig>
    {
        private static readonly string _routeName = "GetConfig";

        public ConfigRouter(LoggingUtil logger, ConfigUtil config, JsonUtil jsonUtil) : base(_routeName, logger, config, jsonUtil)
        {
            
        }

        protected override ValueTask<ModConfig> HandleRoute(string url, IRequestData info, MongoId sessionId, string? output)
        {
            return new ValueTask<ModConfig>(Config.CurrentConfig);
        }
    }
}
