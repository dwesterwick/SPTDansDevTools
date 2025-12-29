using BepInEx;
using Comfort.Common;
using DansDevTools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DansDevTools
{
    [BepInPlugin(ModInfo.GUID, ModInfo.MODNAME, ModInfo.MODVERSION)]
    public class DansDevToolsPlugin : BaseUnityPlugin
    {
        protected void Awake()
        {
            Logger.LogInfo("Loading DansDevTools...");

            Singleton<LoggingUtil>.Create(new LoggingUtil(Logger));

            if (ConfigUtil.CurrentConfig.Enabled)
            {
                Singleton<LoggingUtil>.Instance.LogInfo("Loading DansDevTools...enabled");
            }

            Singleton<LoggingUtil>.Instance.LogInfo("Loading DansDevTools...done.");
        }
    }
}
