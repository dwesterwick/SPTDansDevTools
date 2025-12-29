using DansDevTools.Configuration;
using DansDevTools.Helpers;
using DansDevTools.Routers.Template;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Utils;

namespace DansDevTools.Routers
{
    [Injectable]
    public class ConfigRouter : AbstractStaticRouter<ModConfig>
    {
        private static readonly string[] _routeNames = [ "GetConfig" ];

        public ConfigRouter(LoggingUtil logger, ConfigUtil config, JsonUtil jsonUtil) : base(_routeNames, logger, config, jsonUtil)
        {
            
        }

        protected override ValueTask<ModConfig> HandleRoute(string routeName, RouterData routerData)
        {
            return new ValueTask<ModConfig>(Config.CurrentConfig);
        }
    }
}
