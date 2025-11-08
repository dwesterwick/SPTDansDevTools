using DansDevTools.Configuration;
using DansDevTools.Helpers;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Utils;

namespace DansDevTools.Routers
{
    [Injectable]
    public class ConfigRouter : StaticRouter
    {
        private static ConfigUtil? _config;

        public ConfigRouter(ConfigUtil config, JsonUtil jsonUtil) : base(jsonUtil, GetCustomRoutes())
        {
            _config = config;
        }

        private static List<RouteAction> GetCustomRoutes()
        {
            return
            [
                new RouteAction("/DansDevTools/GetConfig", async (url, info, sessionId, output) => await HandleRoute())
            ];
        }

        private static ValueTask<ModConfig> HandleRoute()
        {
            if (_config == null)
            {
                throw new InvalidOperationException("ConfigUtil is not initialized.");
            }

            return new ValueTask<ModConfig>(_config.Config);
        }
    }
}
