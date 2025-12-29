using DansDevTools.Helpers;
using DansDevTools.Routers.Template;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Utils;

namespace DansDevTools.Routers
{
    [Injectable]
    public class ConfigRouter : AbstractStaticRouter
    {
        private static readonly string[] _routeNames = [ "GetConfig" ];

        public ConfigRouter(LoggingUtil logger, ConfigUtil config, JsonUtil jsonUtil) : base(_routeNames, logger, config, jsonUtil)
        {

        }

        protected override ValueTask<string?> HandleRoute(string routeName, RouterData routerData)
        {
            string json = ConfigUtil.Serialize(Config.CurrentConfig);
            return new ValueTask<string?>(json);
        }
    }
}
