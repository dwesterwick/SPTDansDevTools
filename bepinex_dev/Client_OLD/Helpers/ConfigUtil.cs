using Newtonsoft.Json;
using SPT.Common.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DansDevTools.Helpers
{
    public class ConfigUtil
    {
        public static Configuration.ModConfig Config { get; private set; } = null;

        public static Configuration.ModConfig GetConfig()
        {
            string json = RequestHandler.GetJson("/DansDevTools/GetConfig");
            Config = JsonConvert.DeserializeObject<Configuration.ModConfig>(json);

            return Config;
        }
    }
}
