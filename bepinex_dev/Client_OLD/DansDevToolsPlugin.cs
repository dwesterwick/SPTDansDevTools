using BepInEx;
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

            LoggingUtil.Logger = Logger;

            ConfigUtil.GetConfig();
            if (ConfigUtil.Config.Enabled)
            {
                Logger.LogInfo("Loading DansDevTools...enabled");
            }

            Logger.LogInfo("Loading DansDevTools...done.");
        }
    }
}
