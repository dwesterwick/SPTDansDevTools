using Newtonsoft.Json;
using SPT.Common.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DansDevTools.Helpers
{
    public static class ConfigUtil
    {
        public static Configuration.ModConfig CurrentConfig
        {
            get
            {
                if (_config == null)
                {
                    GetConfig();
                }

                return _config!;
            }
        }

        private static Configuration.ModConfig? _config;

        private static void GetConfig()
        {
            string routeName = RouterHelpers.GetRoutePath("GetConfig");
            string json = RequestHandler.GetJson(routeName);
            Configuration.ModConfig? config = JsonConvert.DeserializeObject<Configuration.ModConfig>(json);

            if (config == null)
            {
                throw new InvalidOperationException("Could not deserialize config file");
            }

            _config = config;
        }
    }
}
