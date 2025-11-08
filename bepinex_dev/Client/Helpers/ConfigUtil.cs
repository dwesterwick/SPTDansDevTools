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
        public static Configuration.ModConfig Config
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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private static Configuration.ModConfig _config;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        private static void GetConfig()
        {
            string json = RequestHandler.GetJson("/DansDevTools/GetConfig");
            Configuration.ModConfig? config = JsonConvert.DeserializeObject<Configuration.ModConfig>(json);

            if (config == null)
            {
                throw new InvalidOperationException("Could not deserialize config file");
            }

            _config = config;
        }
    }
}
